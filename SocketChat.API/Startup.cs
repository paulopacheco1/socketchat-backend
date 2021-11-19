using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocketChat.API.AccessContexts;
using SocketChat.API.Filters;
using SocketChat.API.Middlewares;
using SocketChat.API.SocketsHandlers;
using SocketChat.API.SocketsManager;
using SocketChat.Application.Commands;
using SocketChat.Domain.Providers;
using SocketChat.Domain.Repositories;
using SocketChat.Infrastructure.Auth;
using SocketChat.Infrastructure.Persistence;
using SocketChat.Infrastructure.Persistence.EFCore;
using System;

namespace SocketChat.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("AppDB");
            services.AddDbContext<EFDataContext>(options => options.UseSqlServer(connectionString));

            services.AddMediatR(typeof(Command<>));

            services.AddControllers(options => options.Filters.Add(typeof(ValidationFilter)))
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUsuarioCommandValidator>());

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(Configuration["CORS"].Split(","));
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.WithExposedHeaders("Content-Disposition");
                    });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header usando o esquema Bearer."
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
                             new string[] {}
                     }
                 });
            });

            services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));
            services
              .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,

                      ValidIssuer = Configuration["JwtOptions:Issuer"],
                      ValidAudience = Configuration["JwtOptions:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey
                    (Convert.FromBase64String(Configuration["JwtOptions:AccessSecret"]))
                  };
              });

            services.AddHttpContextAccessor();
            services.AddSingleton<IAuthServiceProvider, AuthService>();
            services.AddScoped<IAccessContextProvider, HttpAccessContextProvider>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocketChat.API v1");
                c.RoutePrefix = String.Empty;
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseWebSockets();
            app.MapSockets("/chat", serviceProvider.GetService<ChatSocketHandler>());
        }
    }
}
