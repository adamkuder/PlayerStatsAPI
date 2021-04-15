using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlayerStatsAPI.Authorization;
using PlayerStatsAPI.Entities;
using PlayerStatsAPI.Middleware;
using PlayerStatsAPI.Migrations;
using PlayerStatsAPI.Models;
using PlayerStatsAPI.Models.Validators;
using PlayerStatsAPI.Services;
using PlayerStatsAPI.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var autheticationSettings = new AutheticationSettings();

            Configuration.GetSection("Authentication").Bind(autheticationSettings);

            services.AddSingleton(autheticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = true;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = autheticationSettings.JwtIssuer,
                    ValidAudience = autheticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(autheticationSettings.JwtKey)),
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasPlayerStats", builder => builder.RequireClaim("PlayerStats"));
                options.AddPolicy("Atleast18", builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
            });

            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<PlayerStatsDbContext>();
            services.AddScoped<PlayerStatsSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IPlayerStatsService, PlayerStatsService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
            services.AddScoped<IValidator<PlayerStatsQuery>, PlayerStatsQueryValidator>();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder =>
                builder.AllowAnyMethod().AllowAnyHeader().WithOrigins(Configuration["AllowedOrigins"]));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PlayerStatsSeeder seeder)
        {
            app.UseCors("FrontEndClient");
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlayerStats API");
            });

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
