using AspNetCoreRateLimit;
using Cantina.Application.Common;
using Cantina.Application.Services;
using CantinaAPI.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Serilog;
using System.Text;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var isprod = environment.Contains(Environments.Production, StringComparison.InvariantCultureIgnoreCase);
LogManager.Setup().LoadConfigurationFromFile(isprod? "nlog.config" : "nlog.debug.config");
var logger = LogManager.GetCurrentClassLogger();


try
{
    logger.Warn("Nlog application satrt");
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASNETCORE_ENVIRONMENT")}.json", optional: true);
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddProblemDetails();

    Console.WriteLine("JWT Key: " + builder.Configuration["Jwt:Key"]);

    // Add services to the container.
    
    builder.Services.AddControllers();
    builder.Services.AddDbContext<CantinaContext>(options =>
        options.UseSqlite(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sqliteOptions => sqliteOptions.MigrationsAssembly("Cantina.Infrastructure"))

    );
    builder.Services.AddScoped<CantinaContext>();


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
    // Add Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Jwt:Authority"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Authority"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), // Secret key used to sign the token
                RoleClaimType = "Roles"
            };
        });


    // Add Authorization
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("FrontEnd", policy => policy.RequireClaim("Roles", "FrontEnd"));
        options.AddPolicy("Admin", policy => policy.RequireClaim("Roles", "Admin"));
    });

    // Add Swagger
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Cantina", Version = "v1" });

        // Add security definition for Bearer token
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        // Add security requirement
        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });

    builder.Services.AddInfrastructure();
    builder.Services.AddScoped<IDishService, DishService>();
    builder.Services.AddScoped<IDrinkService, DrinkService>();
    builder.Services.AddScoped<IReviewService, ReviewService>();

    builder.Services.AddMediatR(typeof(Cantina.Application.Application.Commands.CreateDishCommand).Assembly);
    builder.Services.AddMediatR(typeof(Cantina.Application.Queries.Dishes.Handlers.GetDishesQueryHandler).Assembly);

    builder.Services.AddMediatR(typeof(Program).Assembly);
    builder.Services.AddLogging(loggingBuilder =>
        loggingBuilder.AddSerilog(dispose: true));

    // Rate limiting
    builder.Services.AddMemoryCache();
    builder.Services.Configure<IpRateLimitOptions>(options =>
    {
        options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 5
        }
    };
    });
    builder.Services.AddInMemoryRateLimiting();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
  
    var app = builder.Build();
 

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cantina V1");
        });
    }
    app.UseCors("AllowFrontend");
    app.UseExceptionHandler();
    app.UseStatusCodePages();

    app.UseHttpsRedirection();
    app.UseMiddleware<BearerTokenMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}

catch (Exception ex)
{
    logger.Error(ex, "Stopped program beacuse of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}



