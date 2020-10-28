// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Burbolka LLC">
//   © Burbolka LLC 2020
// </copyright>
// <summary>
//   Defines the Startup type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RouteBuilder.Web
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using RouteBuilder.Common.Interfaces.Services;
    using RouteBuilder.Services.DroneFinder;
    using RouteBuilder.Services.DroneFinder.Models;
    using RouteBuilder.Services.LocationFinder;
    using RouteBuilder.Services.LocationFinder.Models;
    using RouteBuilder.Services.StoreFinder;
    using RouteBuilder.Services.StoreFinder.Models;

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// The configure services.
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            services.Configure<List<DroneLocation>>(
                options => this.Configuration.GetSection("DroneLocation").Bind(options));
            services.Configure<List<StoreLocation>>(
                options => this.Configuration.GetSection("StoreLocation").Bind(options));
            services.Configure<List<LocationSetting>>(
                options => this.Configuration.GetSection("ClientLocation").Bind(options));

            services.AddOptions();

            services.AddScoped<IDroneFinder, DroneFinder>();
            services.AddScoped<IStoreFinder, StoreFinder>();
            services.AddScoped<ILocationFinder, LocationFinder>();

            services.AddCors();
        }

        /// <summary>
        /// The configure.
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
