using System.Data;
using System.Text;
using FluentValidation;
using GameStore.PackingService.Common.Filters;
using GameStore.PackingService.Features.DTOs;
using GameStore.PackingService.Features.Services;
using GameStore.PackingService.Features.Services.Interfaces;
using GameStore.PackingService.Features.Validators;
using GameStore.PackingService.Infrastructure.Data;
using GameStore.PackingService.Infrastructure.Repositories;
using GameStore.PackingService.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Swagger com suporte a JWT Bearer
        builder.Services.AddEndpointsApiExplorer();

        // FluentValidation
        builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
        builder.Services.AddScoped<IValidator<PackingDto>, PackingDtoValidator>();
        builder.Services.AddScoped<IValidator<UsedBoxDto>, UsedBoxDtoValidator>();
        builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();

        // Dapper context
        builder.Services.AddSingleton<GameStoreContext>();
        builder.Services.AddScoped<IDbConnection>(sp =>
            new SqlConnection(builder.Configuration.GetConnectionString("SqlConnection")));

        // Inje��o de depend�ncias
        builder.Services.AddScoped<PackService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        // Autentica��o JWT
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
            };
        });

        // Controllers com filtro global de exce��es
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "GameStore API",
                Version = "v1",
                Description = "API da GameStore"
            });

            // Configura autentica��o JWT no Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Informe o token JWT usando o esquema: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
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
                        },
                        Scheme = "Bearer",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            c.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}
