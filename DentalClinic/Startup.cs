using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DentalClinic.Models;
using DentalClinic.Data;
using Microsoft.EntityFrameworkCore;
using DentalClinic.Controllers;

namespace DentalClinic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        //    var builder = new ConfigurationBuilder()
        //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //.AddJsonFile("config.json", optional: true, reloadOnChange: true);
        //    Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var sqlConnectionString = Configuration.GetConnectionString("DentalClinicConnection");

            services.AddDbContext<ClinicContext>(options =>
                options.UseMySql(
                    sqlConnectionString
                )
            );

            services.AddTransient(typeof(ProfilesService));
            services.AddTransient(typeof(PatientProfileController));
            //services.Add(new ServiceDescriptor(typeof(DentalClinicContext), new DentalClinicContext(Configuration.GetConnectionString("DentalClinicConnection"))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
