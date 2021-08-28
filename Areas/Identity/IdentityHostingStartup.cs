using System;
using System.Threading.Tasks;
using ArtNews.Areas.Identity.Data;
using ArtNews.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ArtNews.Areas.Identity.IdentityHostingStartup))]
namespace ArtNews.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DBNews>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DBNewsConnection")));

                services.AddDefaultIdentity<ArtNewsUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<DBNews>();
                services.ConfigureApplicationCookie(x =>
                {
                    x.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = y =>
                        {
                            y.Response.Redirect("Login/adminSignin");
                            return Task.CompletedTask;
                        }
                    };
                });
            });
        }
    }
}