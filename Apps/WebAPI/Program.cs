using ChyveClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
using Security;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' followed by space and JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<ChyveContext>(options =>
    options.UseNpgsql(
        builder.Configuration["ConnectionString"] ?? "Host=localhost;Database=postgres;Username=postgres;Password=root;Include Error Detail=true",
        o => o.MapEnum<ZoneStatus>("zone_status")
    )
);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ChyveClient.Client>(sp =>
{
    var projectPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
    return new Client(builder.Configuration["PrivateKey"]!, projectPath!);
});
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<IZoneService, ZoneService>();
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IPatRepository, PatRepository>();
builder.Services.AddScoped<IPatService, PatService>();
builder.Services.AddScoped<IGenericRepository<Organization>, GenericRepository<Organization>>();
builder.Services.AddScoped<IGenericService<Organization, OrganizationDTO>, GenericService<Organization, OrganizationDTO>>();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/healthz");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ChyveContext>();

    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
