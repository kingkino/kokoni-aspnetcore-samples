using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using kokoni_aspnetcore_samples.Models;
using kokoni_aspnetcore_samples.Models.Tutorial2;
using kokoni_aspnetcore_samples.MiddleWare;

namespace kokoni_aspnetcore_samples
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                connString = Configuration["ConnectionString:MvcMovieContext"];
            }
            else
            {
                connString = Configuration.GetConnectionString("SQLAZURECONNSTR_MvcMovieContext");
                // connString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_MvcMovieContext");
            }
        }

        public IConfigurationRoot Configuration { get; }

        private string connString { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddDbContext<MvcMovieContext>(options => options.UseSqlServer(connString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // basic認証の追加
            if (env.IsDevelopment())
            {
                // app.UseMiddleware<BasicAuthentication>(Configuration["basicAuthentication:basicUser"], Configuration["basicAuthentication:basicPassword"]);
            }
            else
            {
                app.UseMiddleware<BasicAuthentication>(Environment.GetEnvironmentVariable("basicAuthentication:basicUser"), Environment.GetEnvironmentVariable("basicAuthentication:basicPassword"));
            }

            app.UseStaticFiles();

            app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Landing}/{action=Index}/{id?}");
            });

            SeedData.Initialize(app.ApplicationServices);
        }
    }
}
