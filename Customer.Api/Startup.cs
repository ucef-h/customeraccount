using Bus.DependencyInjection;
using Core;
using Customer.Application;
using Customer.Domain;
using Customer.Infrastructure;
using Customer.Infrastructure.Repositories;
using IntegrationLog.Domain;
using IntegrationLog.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Customer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                })
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        JsonConvert.DefaultSettings = () => options.SerializerSettings;
                    }
                );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer.Api", Version = "v1" });
            });

            services.AddMediatR(typeof(MediatrHandler));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddSingleton<IMongoClient, MongoClient>(_ =>
                new MongoClient(Configuration.GetConnectionString("Mongo"))
            );

            services.AddMassTransitBus(Configuration);

            services.AddTransient<RequestTransient>();
            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();
            services.AddTransient(typeof(IUnitOfWork<Domain.Customer>), typeof(CustomerDbContext));
            services.AddTransient<ICustomerIntegrationEventService, CustomerIntegrationEventService>();

            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<IIntegrationEventLogRepository, IntegrationEventLogRepository>();

            DomainEventsBsonSettings.AddBsonSettings();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}