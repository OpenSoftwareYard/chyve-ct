using EventBus;
using EventBus.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
using RabbitMQ.Client;
using Scheduler;
using Scheduler.EventHandlers;
using Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((host, builder) =>
    {
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json", optional: true);
        builder.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true);
        builder.AddEnvironmentVariables();
        builder.AddCommandLine(args);
    })
    .ConfigureServices((host, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddDbContext<ChyveContext>(options =>
            options.UseNpgsql(host.Configuration["ConnectionString"]),
            ServiceLifetime.Transient, ServiceLifetime.Transient
        );
        services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
            var factory = new ConnectionFactory()
            {
                HostName = host.Configuration["EventBusConnection"],
                DispatchConsumersAsync = true
            };

            if (!string.IsNullOrEmpty(host.Configuration["EventBusUserName"]))
            {
                factory.UserName = host.Configuration["EventBusUserName"];
            }

            if (!string.IsNullOrEmpty(host.Configuration["EventBusPassword"]))
            {
                factory.Password = host.Configuration["EventBusPassword"];
            }

            var retryCount = 5;
            if (!string.IsNullOrEmpty(host.Configuration["EventBusRetryCount"]) && int.TryParse(host.Configuration["EventBusRetryCount"], out int parsedRetryCount))
            {
                retryCount = parsedRetryCount;
            }

            return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
        });
        services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
        {
            var subscriptionClientName = host.Configuration["SubscriptionClientName"];
            var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
            var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
            var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

            var retryCount = 5;
            if (!string.IsNullOrEmpty(host.Configuration["EventBusRetryCount"]) && int.TryParse(host.Configuration["EventBusRetryCount"], out int parsedRetryCount))
            {
                retryCount = parsedRetryCount;
            }

            return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubscriptionsManager, retryCount, subscriptionClientName);
        });

        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        services.AddTransient<PlaceZoneIntegrationEventHandler>();

        services.AddScoped<IZoneRepository, ZoneRepository>();
        services.AddScoped<IGenericRepository<Node>, GenericRepository<Node>>();
        services.AddScoped<INodeService, NodeService>();
        services.AddScoped<IZoneService, ZoneService>();
        services.AddAutoMapper(typeof(MappingProfile));

    }).Build();


await host.RunAsync();
