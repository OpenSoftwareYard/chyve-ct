using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
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
    options.UseNpgsql(builder.Configuration["ConnectionString"] ?? "Host=localhost;Database=postgres;Username=postgres;Password=root;Include Error Detail=true")
);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<IZoneService, ZoneService>();
builder.Services.AddScoped<IGenericRepository<Node>, GenericRepository<Node>>();
builder.Services.AddScoped<INodeService, NodeService>();

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
