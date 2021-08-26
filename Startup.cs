using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IserviceColections_MapWhen.AppEndpoints;
using IserviceColections_MapWhen.DBContextLayer;
using IserviceColections_MapWhen.Middleware;
using IserviceColections_MapWhen.Sevices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace IserviceColections_MapWhen
{
    public class Startup
    {
        public readonly IConfiguration _configuration;
       
        public Startup(IConfiguration configuration )
        {
            _configuration = configuration;
            
        }
  
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSession();
            services.AddSingleton<OneMiddleware>();
            services.AddSingleton<AppDBContext>();
            services.AddSingleton<OneGetEndPoints>();
            services.AddDbContext<AppDBContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("StudentContextConnection"))
            );
            
            var testOption = _configuration.GetSection("TestOptions");
            services.Configure<TestOptions>(testOption);
            services.AddSingleton<AppSeviceStudents>();
            services.AddSingleton<IServiceCollection, ServiceCollection>((provider) => {
                var c = services;
                return (ServiceCollection)c;
            });
            services.AddSingleton<IConfigurationBuilder, ConfigurationBuilder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            /*
            app.Run(async(context) => {

                var stringBuilder = new StringBuilder();
                stringBuilder.Append("<tr><th>Tên</th><th>Lifetime</th><th>Tên đầy đủ</th></tr>");
                foreach (var service in _serviceDescriptors)
                {
                    string tr = service.ServiceType.Name.ToString().HtmlTag("td") +
                    service.Lifetime.ToString().HtmlTag("td") +
                    service.ServiceType.FullName.HtmlTag("td");
                    stringBuilder.Append(tr.HtmlTag("tr"));
                }

                string htmlallservice = stringBuilder.ToString().HtmlTag("table", "table table-bordered table-sm");
                string html = HtmlHelper.HtmlDocument("Các dịch vụ", (htmlallservice));

                await context.Response.WriteAsync(html);

            });
            */
            //app.UseMiddleware<OneMiddleware>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMethods("/",new[] { "Get", "POST" }, async (context) =>
                {
                    var oneGetEndPoints = context.RequestServices.GetService<OneGetEndPoints>();
                    await oneGetEndPoints.InvokeAsync(context);

                });
                endpoints.MapGet("/Form", async (context) =>
                {
                    var oneGetEndPoints = context.RequestServices.GetService<OneGetEndPoints>();
                    await oneGetEndPoints.DisPlayForm(context);

                });
                endpoints.MapGet("/ShowOptions", async (context) => {

                    var configuration = context.RequestServices.GetService<IOptions<TestOptions>>().Value;

                    await context.Response.WriteAsync(configuration.option_key2.k2);

                });
            });
        }
    }

   
}
