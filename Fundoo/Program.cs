using BussinessLayer.Interface;
using BussinessLayer.Notes.Command;
using BussinessLayer.Notes.Query;
using BussinessLayer.Service;
using BussinessLayer.User.Command;
using BussinessLayer.User.Query;
using Fundoo.ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middleware.Security;
using Middleware.Services.Cache;
using Middleware.Services.Email;
using NLog.Web;
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

            //Configure NLog
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserBL, UserBL>();
            builder.Services.AddScoped<IUserRL, UserRL>();
            builder.Services.AddScoped<INoteBL, NoteBL>();
            builder.Services.AddScoped<INoteRL, NoteRL>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<CustomPasswordHasher>();

            // Register Handler
            builder.Services.AddScoped<IRegisterUserCommandHandler, RegisterUserCommandHandler>();
            builder.Services.AddScoped<ILoginUserQueryHandler, LoginUserQueryHandler>();
            builder.Services.AddScoped<IForgotPasswordCommandHandler, ForgotPasswordCommandHandler > ();
            builder.Services.AddScoped<IResetPasswordCommandHandler, ResetPasswordCommandHandler>();
            builder.Services.AddScoped<IAddNoteCommandHandler, AddNoteCommandHandler>();
            builder.Services.AddScoped<IViewNotesQueryHandler, ViewNotesQueryHandler>();
            builder.Services.AddScoped<IViewNoteByIdQueryHandler , ViewNoteByIdQueryHandler>();
            builder.Services.AddScoped<IEditNoteCommandHandler, EditNoteCommandHandler>();
            builder.Services.AddScoped<IDeleteNoteCommandHandler, DeleteNoteCommandHandler>();
            builder.Services.AddScoped<IArchiveNoteCommandHandler, ArchiveNoteCommandHandler>();

            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.AddDbContext<FundooContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("FundooConnection"));
            });

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddHttpClient();

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

            //Redis register
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "Fundoo";
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseExceptionHandler();

            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.   

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}