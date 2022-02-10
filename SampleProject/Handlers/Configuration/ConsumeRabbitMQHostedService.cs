using SampleProject.Handlers.Configuration;
using SampleProject.Helper;
using SampleProject.Models;
using SampleProject.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace SampleProject.Handlers.Configuration
{
      public class ConsumeRabbitMQHostedService : BackgroundService
      {
            private IModel _channel;
            private readonly DefaultObjectPool<IModel> _objectPool;
            private IServiceProvider ServiceProvider;
            internal List<IHandler> Handlers = new List<IHandler>();
            public ConsumeRabbitMQHostedService(
                IPooledObjectPolicy<IModel> objectPolicy,
                IConfiguration Configuration,
                IServiceProvider ServiceProvider)
            {
                  if (StaticParams.EnableExternalService)
                  {
                        this.ServiceProvider = ServiceProvider;
                        string exchangeName = "exchange";
                        _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount);
                        _channel = _objectPool.Get();
                        _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false);
                        Dictionary<string, object> arguments = new Dictionary<string, object>();
                        arguments.Add("x-single-active-consumer", true);
                        _channel.QueueDeclare(StaticParams.ModuleName, true, false, false, arguments);

                        List<Type> handlerTypes = typeof(ConsumeRabbitMQHostedService).Assembly.GetTypes()
                            .Where(x => typeof(Handler).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract)
                            .ToList();
                        foreach (Type type in handlerTypes)
                        {
                              Handler handler = (Handler)Activator.CreateInstance(type);
                              Handlers.Add(handler);
                        }

                        foreach (IHandler handler in Handlers)
                        {
                              handler.QueueBind(_channel, StaticParams.ModuleName, exchangeName);
                        }
                        _channel.BasicQos(0, 1, false);
                  }
            }

            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                  if (StaticParams.EnableExternalService)
                  {
                        stoppingToken.ThrowIfCancellationRequested();

                        var consumer = new EventingBasicConsumer(_channel);
                        consumer.Received += async (ch, ea) =>
                        {
                              // received message  
                              var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                              var routingKey = ea.RoutingKey;
                              // handle the received message  
                              try
                              {
                                    string ModuleName = StaticParams.ModuleName;
                                    IDictionary<string, object> Headers = ea.BasicProperties.Headers;
                                    if (Headers?.ContainsKey("ModuleName") == true)
                                    {
                                          byte[] array = (byte[])Headers["ModuleName"];
                                          ModuleName = System.Text.Encoding.UTF8.GetString(array);
                                    }
                                    await HandleMessage(ModuleName, routingKey, content);
                              }
                              catch (Exception)
                              {

                              }
                              _channel.BasicAck(ea.DeliveryTag, false);
                        };

                        consumer.Shutdown += OnConsumerShutdown;
                        consumer.Registered += OnConsumerRegistered;
                        consumer.Unregistered += OnConsumerUnregistered;
                        consumer.ConsumerCancelled += OnConsumerCancelled;

                        _channel.BasicConsume(StaticParams.ModuleName, false, consumer);
                  }
                  return Task.CompletedTask;
            }

            private async Task HandleMessage(string ModuleName, string routingKey, string content)
            {
                  using var scope = ServiceProvider.CreateScope();
                  foreach (IHandler handler in Handlers)
                  {
                        handler.ServiceProvider = scope.ServiceProvider;
                  }
                  List<string> path = routingKey.Split(".").ToList();
                  if (path.Count < 1)
                        throw new Exception();
                  foreach (IHandler handler in Handlers)
                  {
                        if (path.Any(p => p == handler.Name))
                        {
                              await handler.Handle(routingKey, content);
                        }
                  }
            }

            private void OnConsumerCancelled(object sender, ConsumerEventArgs e) { }
            private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
            private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
            private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
      }
}
