using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Bus.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static void AddMassTransitBus(this IServiceCollection services, IConfiguration configuration,
            Dictionary<string, Func<IServiceProvider, Action<IReceiveEndpointConfigurator>>> endpoints = default,
            Action<IServiceCollectionBusConfigurator> configureBusServices = null)
        {
            var serviceBusSettings = services.AddSettings<ServiceBusSettings>(configuration);
            services.AddMassTransit(configurator =>
            {
                configureBusServices?.Invoke(configurator);
                configurator.UsingRabbitMq((context, factoryConfigurator) =>
                {
                    var rabbitmqEndpoint = new Uri(serviceBusSettings.Host);
                    factoryConfigurator.Host(rabbitmqEndpoint, h =>
                    {
                        h.Username(serviceBusSettings.AccessKey);
                        h.Password(serviceBusSettings.AccessSecret);
                    });
                    if (endpoints == default) return;
                    foreach (var (endpoint, endpointConfiguration) in endpoints)
                    {
                        if (string.IsNullOrEmpty(endpoint))
                        {
                            factoryConfigurator.ReceiveEndpoint(configure =>
                            {
                                endpointConfiguration(context)(configure);
                            });
                        }
                        else
                        {
                            factoryConfigurator.ReceiveEndpoint(endpoint, endpointConfiguration(context));
                        }
                    }

                });
            });
            services.AddHostedService<MassTransitHostedService>();
        }

        public static T AddSettings<T>(this IServiceCollection services, IConfiguration configuration) where T : class
        {
            var section = configuration.GetSection(typeof(T).Name);
            var settings = section.Get<T>();
            services.Configure<T>(section);
            return settings;
        }
    }
}