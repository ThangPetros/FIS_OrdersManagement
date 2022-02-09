using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SampleProject.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using TrueSight.Common;

namespace SampleProject.Handlers.Configuration
{
      public interface IRabbitManager
      {
            void PublishList<T>(List<T> message, string routeKey) where T : DataEntity;
            void PublishSingle<T>(T message, string routeKey) where T : DataEntity;
      }
      public class RabbitManager : IRabbitManager
      {
            private readonly DefaultObjectPool<IModel> _objectPool;

            public RabbitManager(IPooledObjectPolicy<IModel> objectPolicy)
            {
                  _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 16);
            }

            public void PublishList<T>(List<T> message, string routeKey) where T : DataEntity
            {
                  if (!StaticParams.EnableExternalService)
                        return;
                  if (message == null)
                        return;

                  var channel = _objectPool.Get();

                  try
                  {
                        channel.ExchangeDeclare("exchange", ExchangeType.Topic, true, false, null);

                        var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish("exchange", routeKey, properties, sendBytes);
                  }
                  catch (Exception ex)
                  {
                        throw ex;
                  }
                  finally
                  {
                        _objectPool.Return(channel);
                  }
            }

            public void PublishSingle<T>(T message, string routeKey) where T : DataEntity
            {
                  if (message == null)
                        return;

                  List<T> list = new List<T> { message };
                  PublishList(list, routeKey);
            }
      }
}
