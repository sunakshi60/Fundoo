using BussinessLayer.Interface;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System.Text;

namespace FundooPractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserBL, UserBL>();
            builder.Services.AddScoped<IUserRL, UserRL>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddDbContext<FundooContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("FundooConnection"));
            });
            builder.Services.AddEndpointsApiExplorer();

            // CORS Implementation
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("https://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            // Swagger configuration 
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token"
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

            builder.Services.AddHttpClient<IMessageClient, MessageClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7222/");
            });

            // Jwt configuration
            var jwtSettings = builder.Configuration.GetSection("Jwt");

            var key = jwtSettings["Key"];

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

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key!)
                    )
                };
            });


            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Configure the HTTP request pipeline.

            

            //app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
