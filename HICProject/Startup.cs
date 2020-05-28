using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HICProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.Models.HIC;

namespace HICProject
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PatientContext>(options =>
                options.UseSqlServer(connection));

            services.ConfigureApplicationCookie(option => option.LoginPath = "/Account/Login");

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<PatientContext>();

            /* services.AddMvc(options =>
             {
                 var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                 options.Filters.Add(new AuthorizeFilter(policy));
             });*/

            services.AddControllersWithViews();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "514763153128-dje598jhm7n880d7uvkcseopt7odaebg.apps.googleusercontent.com";
                    options.ClientSecret = "V0uXqj7Oet5knaQB-5zEE6qJ";
                }).AddFacebook(options =>
                {
                    options.ClientId = "3120125901364714";
                    options.ClientSecret = "d41f41fa7e2967e5f325c7a26ff2b8a5";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Start}/{id?}");
            });
        }
    }
}