using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.DTOs;
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

        services.AddScoped<ChyveClient.Client>();
        services.AddScoped<IZoneRepository, ZoneRepository>();
        services.AddScoped<INodeRepository, NodeRepository>();
        services.AddScoped<INodeService, NodeService>();
        services.AddScoped<IZoneService, ZoneService>();
        services.AddScoped<PlaceZoneHandler>();

        services.AddAutoMapper(typeof(MappingProfile));

    }).Build();


await host.RunAsync();
