using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;

namespace MyCourse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddMvc(options =>{

                var homeProfile = new CacheProfile();
                //homeProfile.Location = Configuration.GetValue<ResponseCacheLocation>("ResponseCache:Home:Location");
                //homeProfile.Duration = Configuration.GetValue<int>("ResponseCache:Home:Duration");
                //oppure
                Configuration.Bind("ResponseCache:Home",homeProfile);
                options.CacheProfiles.Add("Home", homeProfile);
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<ICourseService,AdoDotNetCourseService>();
            services.AddTransient<ICachedCourseService,MemoryCacheCourseService>();
            //services.AddTransient<ICourseService,EFCoreCourseService>();
            services.AddTransient<IDatabaseAccessor,SqliteDatabaseAccessor>();

            //services.AddScoped<MyCourseDbContext>();
            //AddDbContext aggiunge il dbcontext come addscoped 
            //services.AddDbContext<MyCourseDbContext>();
            services.AddDbContextPool<MyCourseDbContext>(optionsBuilder =>{
//                var connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");
            var connectionString = Configuration.GetConnectionString("Default");
                optionsBuilder.UseSqlite(connectionString);
            });

            //Options
            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<CoursesOptions>(Configuration.GetSection("Courses"));
            services.Configure<MemoryCacheOptions>(Configuration.GetSection("MemoryCache"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if(env.IsProduction())
            {
                app.UseHttpsRedirection();                
            }
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                lifetime.ApplicationStarted.Register(()=>
                {
                    string filePath = Path.Combine(env.ContentRootPath,"bin/reload.txt" );
                    File.WriteAllText(filePath,DateTime.Now.ToString());
                });
            }
            else{
                app.UseExceptionHandler("/Error");
    
            }
            app.UseStaticFiles();
            app.UseResponseCaching();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routeBuilder =>{
                routeBuilder.MapRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
