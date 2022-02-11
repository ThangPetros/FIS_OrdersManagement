using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Extensions;
using SampleProject.Models;
using TrueSight.Common;

namespace SampleProject.Tests
{
	public class CommonTests
	{
		protected DataContext DataContext;
		protected ServiceProvider provider;
		public CommonTests()
		{
			/*_ = License.EfExtension;

			var config = new ConfigurationBuilder()
			   .AddJsonFile("appsettings.json")
			   .Build();
			string connectionString = config.GetConnectionString("DataContext");
			var options = new DbContextOptionsBuilder<DataContext>()
			    .UseSqlServer(connectionString)
			    .Options;
			DataContext = new DataContext(options);
			EntityFrameworkManager.ContextFactory = DbContext => new DataContext(options);*/
		}
		public void Init()
		{
			_ = License.EfExtension;

			var config = new ConfigurationBuilder()
			   .AddJsonFile("appsettings.json")
			   .Build();
			string connectionString = config.GetConnectionString("DataContext");
			var options = new DbContextOptionsBuilder<DataContext>()
			    .UseSqlServer(connectionString)
			    .Options;
			DataContext = new DataContext(options);
			EntityFrameworkManager.ContextFactory = DbContext => new DataContext(options);
		}
		public async Task Clean()
		{
			DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
			DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'");
			DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT(''?'', RESEED, 0)'");
			DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
		}
		public static Guid CreateGuid(string name)
		{
			MD5 md5 = MD5.Create();
			Byte[] myStringBytes = ASCIIEncoding.Default.GetBytes(name);
			Byte[] hash = md5.ComputeHash(myStringBytes);
			return new Guid(hash);
		}
	}
}
