﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using EmailSender.Implementations;
using EmailSender.Interfaces;
using System;
using IdentityServer4;
using IdentityProvider.Data;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityProvider
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsProduction())
            {
              services.AddApplicationInsightsTelemetry();
            }
            services.AddControllersWithViews();
       
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["SqlConnection"]));

            services.AddDatabaseDeveloperPageExceptionFilter();      

            services.AddSingleton<IEmailNotifierConfig>(new EmailNotifierConfig
            {
                //there should be some config 
            });

            services.AddScoped<IEmailNotifier>(provider => new SendGridEmailNotifier(provider.GetRequiredService<IEmailNotifierConfig>().EmailApiKey));
            

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryApiScopes(Config.Scopes)
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            var rsa = new RsaKeyService(Environment, TimeSpan.FromDays(30));
            services.AddSingleton<RsaKeyService>(provider => rsa);
            builder.AddSigningCredential(rsa.GetKey(), IdentityServerConstants.RsaSigningAlgorithm.RS512);
            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}