using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtNews.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArtNews
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IdentityOptions>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireLowercase = false;
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequiredLength = 1;

            });
            services.AddAuthentication();



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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
            app.UseCookiePolicy();

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=adminSignin}/{id?}");
            });
          //  var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
          //  var userManager = services.GetRequiredService<UserManager<ArtNewsUser>>();

             initRoles(serviceProvider).Wait();
        }

        private async Task initRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ArtNewsUser>>();

            if (await roleManager.RoleExistsAsync("admin") == false)
            {
                var role = new IdentityRole("admin");
                await roleManager.CreateAsync(role);
            }

            var admin = await userManager.FindByNameAsync("admin@gmail.com");
            if (admin == null)
            {
                var adminUser = new ArtNewsUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com"
                };
                await userManager.CreateAsync(adminUser, "admin");
                admin = adminUser;
            }
            if (await userManager.IsInRoleAsync(admin, "admin") == false)
            {
              await  userManager.AddToRoleAsync(admin, "admin");
            }

        }
    }
}
