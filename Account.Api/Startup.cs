using Account.Application;
using Account.Domain;
using Account.Infrastructure;
using Account.Infrastructure.Repositories;
using Bus.DependencyInjection;
using Core;
using IntegrationLog.Domain;
using IntegrationLog.Infrastructure;
using MassTransit;
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
using System;
using System.Collections.Generic;

namespace Account.Api
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Account.Api", Version = "v1" });
            });
            services.AddMediatR(typeof(MediatrHandler));
            services.AddSingleton<IMongoClient, MongoClient>(_ =>
                new MongoClient(Configuration.GetConnectionString("Mongo"))
            );

            services.AddMassTransitBus(Configuration,
                new Dictionary<string, Func<IServiceProvider, Action<IReceiveEndpointConfigurator>>>
                {
                    ["Account.Api.CustomerCreated"] = provider => ep =>
                    {
                        ep.Consumer<CustomerCreatedIntegrationEventConsumer>(provider);
                    }
                },
                c => { c.AddConsumer<CustomerCreatedIntegrationEventConsumer>(); });

            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();
            services.AddTransient(typeof(IUnitOfWork<Domain.Account>), typeof(AccountDbContext));

            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IIntegrationEventLogRepository, IntegrationEventLogRepository>();

            DomainBsonSettings.AddBsonSettings();
            DomainEventsBsonSettings.AddBsonSettings();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}