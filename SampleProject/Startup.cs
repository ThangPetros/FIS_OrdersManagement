using SampleProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Thinktecture;
using TrueSight.Common;
using Z.EntityFramework.Extensions;
using Newtonsoft.Json;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using SampleProject.Handlers.Configuration;

namespace SampleProject
{
      public class MyDesignTimeService : DesignTimeService { }
      public class Startup
      {

            public IConfiguration Configuration { get; }
            public Startup(IHostEnvironment env)
            {
                  var builder = new ConfigurationBuilder()
                  .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true)
                  .AddEnvironmentVariables();

                  Configuration = builder.Build();
                  _ = License.EfExtension;
            }

            // This method gets called by the runtime. Use this method to add services to the container.
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            public void ConfigureServices(IServiceCollection services)
            {
                  //services.AddControllers();
                  _ = DataEntity.InformationResource;
                  _ = DataEntity.WarningResource;
                  _ = DataEntity.ErrorResource;
                  services.AddControllers().AddNewtonsoftJson(
                      options =>
                      {
                            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                            options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                            options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                            options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK";
                      });

                  //services.AddSingleton<IRedisStore, RedisStore>();
                  services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
                  services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
                  services.AddSingleton<IRabbitManager, RabbitManager>();
                  services.AddHostedService<ConsumeRabbitMQHostedService>();
                  services.AddDbContext<DataContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DataContext"), sqlOptions =>
                   {
                         sqlOptions.AddTempTableSupport();
                   }));

                  EntityFrameworkManager.ContextFactory = context =>
                  {
                        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DataContext"));
                        DataContext DataContext = new DataContext(optionsBuilder.Options);
                        return DataContext;
                  };

			Assembly[] assemblies = new[] { Assembly.GetAssembly(typeof(IServiceScoped)),
		    Assembly.GetAssembly(typeof(Startup)) };
			services.Scan(scan => scan
			.FromAssemblies(assemblies)
			.AddClasses(classes => classes.AssignableTo<IServiceScoped>())
			     .AsImplementedInterfaces()
			     .WithScopedLifetime());
		}

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                  if (env.IsDevelopment())
                  {
                        app.UseDeveloperExceptionPage();
                  }

                  app.UseHttpsRedirection();

                  app.UseRouting();

                  app.UseAuthorization();

                  app.UseEndpoints(endpoints =>
                  {
                        endpoints.MapControllers();
                  });
            }
      }
}
/*using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Thinktecture;
using TrueSight.Common;
using Z.EntityFramework.Extensions;

namespace SampleProject
{
    public class MyDesignTimeService : DesignTimeService { }
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
            _ = License.EfExtension;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<DataContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DataContext"), sqlOptions =>
              {
                  sqlOptions.AddTempTableSupport();
              }));
            EntityFrameworkManager.ContextFactory = context =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DataContext"));
                DataContext DataContext = new DataContext(optionsBuilder.Options);
                return DataContext;
            };
                  Assembly[] assemblies = new[] { Assembly.GetAssembly(typeof(IServiceScoped)),
                Assembly.GetAssembly(typeof(Startup)) };
                  services.Scan(scan => scan
                  .FromAssemblies(assemblies)
                  .AddClasses(classes => classes.AssignableTo<IServiceScoped>())
                       .AsImplementedInterfaces()
                       .WithScopedLifetime());
            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}*/

