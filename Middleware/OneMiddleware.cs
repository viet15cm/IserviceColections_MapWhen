using IserviceColections_MapWhen.Sevices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IserviceColections_MapWhen.Middleware
{

    public class OneMiddleware : IMiddleware
    {
        public readonly TestOptions _Options;
        public readonly AppSeviceStudents _appSeviceStudents;
        public readonly IConfiguration _configuration;
        //public readonly IServiceProvider _serviceProvider;
        public readonly IServiceCollection _serviceDescriptors;
        public readonly IConfigurationBuilder _configurationBuilder;
        public IConfigurationRoot _configurationRoot { get; set; }



        public OneMiddleware(IOptions<TestOptions> options ,
            AppSeviceStudents appSeviceStudents , 
            IConfiguration configuration,
            IServiceCollection serviceDescriptors,
            IConfigurationBuilder  configurationBuilder 

            )
        {
            _Options = options.Value;
            _appSeviceStudents = appSeviceStudents;
            _configuration = configuration;
            _serviceDescriptors = serviceDescriptors;
            _configurationBuilder = configurationBuilder;


        }
        
        public async Task  InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await DisPlayTableStudents(context);

            //await context.Response.WriteAsync(_configuration.GetConnectionString("StudentContextConnection"));

            await DisPlayTableIServiceColection(context);

            //await DisplayJson(context);


        }
        public async Task DisPlayTableStudents(HttpContext context)
        {
            var sb = new StringBuilder();
            
            var listStudents = await _appSeviceStudents.ReadStudents();
            sb.Append(("Id".th() + "Name".th() + "Age".th()).tr());
            foreach (var item in listStudents)
            {
                sb.Append((item.Id.ToString().td() + item.Name.ToString().td() + item.Age.ToString().td()).tr());
            }

            string info = ("Danh Sach Students ".HtmlTag("h2") + sb.ToString().table("table table-sm table-bordered")).HtmlTag("div", "container");

            string html = HtmlHelper.HtmlDocument("TableStudent", info);

            await context.Response.WriteAsync(html);
        }

        public async Task DisPlayTableIServiceColection(HttpContext context)
        {
            
            var sb = new StringBuilder();
            sb.Append(("Ten".th() + "LifeTime".th() + "Ten Day Du".th()).tr());
            foreach (var item in _serviceDescriptors)
            {
                sb.Append((item.ServiceType.Name.ToString().td() +
                    item.Lifetime.ToString().td() +
                    item.ServiceType.FullName.ToString().td()).tr());
            }

            string table = (("Danh Sach Cac Sevice Dang Ki".HtmlTag("h2") + sb.ToString().
                table("table table-sm table-bordered")).
                HtmlTag("div", "container"));
            string html = HtmlHelper.HtmlDocument("TableIseviceColection", table);
            await context.Response.WriteAsync(html);
        }

        public async Task DisplayJson(HttpContext context)
        {
           
            _configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            _configurationBuilder.AddJsonFile("FileJson/CauHinh.json");
            _configurationRoot = _configurationBuilder.Build();
            var st = _configurationRoot.GetSection("ConnectionStrings").GetSection("StudentContextConnection").Value;
            await context.Response.WriteAsync(st);
        }
    }
}
