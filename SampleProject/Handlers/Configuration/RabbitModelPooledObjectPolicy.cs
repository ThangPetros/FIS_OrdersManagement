using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleProject.Handlers.Configuration
{
	public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
	{
		private readonly IConnection _connection;

		public RabbitModelPooledObjectPolicy(IConfiguration Configuration)
		{
			var factory = new ConnectionFactory
			{
				HostName = Configuration["RabbitConfig:Hostname"],
				UserName = Configuration["RabbitConfig:Username"],
				Password = Configuration["RabbitConfig:Password"],
				VirtualHost = Configuration["RabbitConfig:VirtualHost"],
				Port = int.Parse(Configuration["RabbitConfig:Port"]),
			};

			// create connection  
			_connection = factory.CreateConnection();
			_connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

		}

		public IModel Create()
		{
			return _connection.CreateModel();
		}

		public bool Return(IModel obj)
		{
			if (obj.IsOpen)
			{
				return true;
			}
			else
			{
				obj?.Dispose();
				return false;
			}
		}
		private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }
	}
}
