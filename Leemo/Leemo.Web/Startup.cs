using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using TPSS.Common;
using Leemo.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using Leemo.Web.Filters;

namespace Leemo.Web
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
            services.AddMvc();
            services.AddSession(options=> {
                options.IdleTimeout = TimeSpan.FromHours(3);
            });

            services.AddSingleton(c => Configuration);

            services.AddScoped<AppSettings>();
            services.Configure<AppSettings>
               (options => Configuration.GetSection(Constants.AppSettings).Bind(options));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<SessionManager>();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddScoped<ActiveRouteTagHelper>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = Constants.WebConstants.Urls.WEB_AccountLogin;
                    options.LogoutPath = Constants.WebConstants.Urls.WEB_AccountLogout;
                });
            services.AddControllersWithViews();
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
                app.UseExceptionHandler(Constants.WebConstants.Urls.WEB_ErrorIndexPage);
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = "/data"
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseStatusCodePagesWithRedirects(Constants.WebConstants.Urls.WEB_Route_Error_Codes);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
