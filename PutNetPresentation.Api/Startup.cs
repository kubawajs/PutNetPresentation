using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using PutNetPresentation.Core.Repositories;
using PutNetPresentation.Infrastructure.Mapping;
using PutNetPresentation.Infrastructure.Repositories;
using PutNetPresentation.Infrastructure.Services;
using PutNetPresentation.Infrastructure.Services.Abstractions;

namespace PutNetPresentation.Api
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
            // Add Automapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            // Add services
            services.AddTransient<IUserService, UserService>();

            // Add repositories
            services.AddTransient<IUserRepository, UserRepository>();

            // Add caching
            services.AddResponseCaching();
            services.AddMemoryCache();

            // Versioning
            services.AddApiVersioning(config =>
            {
                // Will provide the different api version which is available for the client
                config.ReportApiVersions = true;

                // this configuration will allow the api to automaticaly take api_version=1.0 in case it was not specify
                config.AssumeDefaultVersionWhenUnspecified = true;

                // We are giving the default version of 1.0 to the api
                config.DefaultApiVersion = ApiVersion.Default; // new ApiVersion(1, 0);
            });

            // Swagger config
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PutNetPresentation.Api", 
                    Version = "v1",
                    Description = "Example .NET 5 REST API",
                    Contact = new OpenApiContact
                    {
                        Name = "Jakub Wajs",
                        Email = "j.wajs@emakina.pl",
                        Url = new Uri("https://www.linkedin.com/in/jakub-wajs/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PutNetPresentation.Api v1");
                    c.RoutePrefix = "api";
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
