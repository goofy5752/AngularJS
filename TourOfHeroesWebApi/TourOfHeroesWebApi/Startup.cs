﻿// ReSharper disable StringLiteralTypo
namespace TourOfHeroesWebApi
{
    using NLog;
    using System;
    using System.Text;
    using System.IO;
    using TourOfHeroesCommon;
    using System.Reflection;
    using TourOfHeroesData;
    using TourOfHeroesData.Common;
    using TourOfHeroesData.Common.Contracts;
    using TourOfHeroesData.Models;
    using TourOfHeroesData.Seeder;
    using TourOfHeroesData.Seeder.Contracts;
    using TourOfHeroesMapping.Mapping;
    using TourOfHeroesDTOs.HeroDtos;
    using TourOfHeroesServices;
    using TourOfHeroesServices.Contracts;
    using Controllers.Validator;
    using Controllers.Validator.Contracts;
    using TourOfHeroesServices.RealTimeHub;
    using GlobalErrorHandling.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<TourOfHeroesDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TourOfHeroesDbContext>();

            services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 4;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 3;
                }
            );

            //Jwt Authentication

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            //repository service

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            //entity services

            services.AddTransient<ISeeder, Seeder>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IHeroService, HeroService>();
            services.AddTransient<IHistoryService, HistoryService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();

            //validator service

            services.AddTransient<IUserValidator, UserValidator>();

            //real time application service

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ISeeder seeder)
        {
            AutoMapperConfig.RegisterMappings(typeof(PageResultDTO<>).GetTypeInfo().Assembly);

            //seed application roles

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<TourOfHeroesDbContext>())
                {
                    context.Database.EnsureCreated();

                    if (!context.Roles.Any())
                    {
                        context.Roles.Add(new IdentityRole
                        {
                            Name = GlobalConstants.AdminRole,
                            NormalizedName = GlobalConstants.AdminRole.ToUpper()
                        });

                        context.Roles.Add(new IdentityRole
                        {
                            Name = GlobalConstants.EditorRole,
                            NormalizedName = GlobalConstants.EditorRole.ToUpper()
                        });

                        context.Roles.Add(new IdentityRole
                        {
                            Name = GlobalConstants.UserRole,
                            NormalizedName = GlobalConstants.UserRole.ToUpper()
                        });

                        context.SaveChanges();
                    }
                }
            }

            seeder.SeedDatabase();

            app.UseDeveloperExceptionPage();

            app.UseCors(options =>
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(Configuration["ApplicationSettings:Client_URL"]));

            app.UseSignalR(routes =>
            {
                routes.MapHub<CommentHub>("/api/comments");
                routes.MapHub<CommentHub>("/api/blog");
                routes.MapHub<ProfileImageHub>("/api/profile");
            });

            app.ConfigureCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseHsts();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}