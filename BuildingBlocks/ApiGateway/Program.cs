using ApiGateway.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Persistence.Data;
using Persistence.DTOs;
using Security;
using Services;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(c =>
{
    c.AddDefaultPolicy(p => p.SetIsOriginAllowed((host) => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("chyve-key", options =>
{
    options.Authority = "https://chyve-dev.eu.auth0.com/";
    options.Audience = "https://chyve-ct.opensoftwareyard.com";
    options.MapInboundClaims = false;
}).AddScheme<AuthenticationSchemeOptions, PatAuthenticationHandler>("PAT", _ => { });

builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IPatRepository, PatRepository>();
builder.Services.AddScoped<IPatService, PatService>();
builder.Services.AddDbContext<ChyveContext>(options =>
    options.UseNpgsql(
        builder.Configuration["ConnectionString"] ?? "Host=localhost;Database=postgres;Username=postgres;Password=root;Include Error Detail=true"
    )
);

builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Configuration.AddJsonFile("ocelot.json");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI();
}

app.UseCors();

app.UseHttpsRedirection();

var conf = new OcelotPipelineConfiguration()
{
    PreErrorResponderMiddleware = async (ctx, next) =>
    {
        if (ctx.Request.Path.Equals(new PathString("/healthz")))
        {
            await ctx.Response.WriteAsync("ok");
        }
        else
        {
            await next.Invoke();
        }
    }
};

app.UseOcelot(conf).Wait();

app.Run();

