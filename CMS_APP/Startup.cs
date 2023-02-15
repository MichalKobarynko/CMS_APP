using CMS_APP.Data;
using CMS_APP.FunctionalServices;
using CMS_APP.Infrastructure.Identity;
using CMS_APP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CMS_APP
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
            /*---------------------------------------------------------------------*/
            /*                          Database                                   */
            /*---------------------------------------------------------------------*/
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<DataProtectionKeysContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DataProtectionKeysContext")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            /*---------------------------------------------------------------------*/
            /*                          Identity                                   */
            /*---------------------------------------------------------------------*/
            services.AddTransient<IFunctionalSvc, FunctionalSvc>();
            services.Configure<AdminUserOptions>(Configuration.GetSection("AdminUserOptions"));
            services.Configure<AppUserOptions>(Configuration.GetSection("AppUserOptions"));

            var identityDefaultOptionsConfiguration = Configuration.GetSection("IdentityDefaultOptions");
            services.Configure<IdentityDefaultOptions>(identityDefaultOptionsConfiguration);
            var identityDefaultOptions = identityDefaultOptionsConfiguration.Get<IdentityDefaultOptions>();

            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //Password
                options.Password.RequireDigit = identityDefaultOptions.PasswordRequireDigit;
                options.Password.RequiredLength = identityDefaultOptions.PasswordRequireLength;
                options.Password.RequireNonAlphanumeric = identityDefaultOptions.PasswordRequireAlphanumeric;
                options.Password.RequireUppercase = identityDefaultOptions.PasswordRequireUppercase;
                options.Password.RequireLowercase = identityDefaultOptions.PasswordRequireLowercase;
                options.Password.RequiredUniqueChars = identityDefaultOptions.PasswordRequireUniqueChars;
                //Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identityDefaultOptions.LockoutDefaultLockoutTimeSpanInMinutes);
                options.Lockout.MaxFailedAccessAttempts = identityDefaultOptions.LockoutMaxFailedAccessAttempts;
                options.Lockout.AllowedForNewUsers = identityDefaultOptions.LockoutAllowedForNewUsers;
                //User settings
                options.User.RequireUniqueEmail = identityDefaultOptions.UserRequireUniqueEmail;
                //Email confirmation
                options.SignIn.RequireConfirmedEmail = identityDefaultOptions.SignInRequireConfirmedEmail;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();


            /*---------------------------------------------------------------------*/
            /*                          Authentication                             */
            /*---------------------------------------------------------------------*/
            services.AddAuthentication()
                .AddIdentityServerJwt();
            services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
