using IserviceColections_MapWhen.Model;
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

namespace IserviceColections_MapWhen.AppEndpoints
{
    public class OneGetEndPoints
    {
        public readonly TestOptions _Options;
        public readonly AppSeviceStudents _appSeviceStudents;
        public readonly IConfiguration _configuration;
        //public readonly IServiceProvider _serviceProvider;
        public readonly IServiceCollection _serviceDescriptors;
        public readonly IConfigurationBuilder _configurationBuilder;
        public IConfigurationRoot _configurationRoot { get; set; }



        public OneGetEndPoints(IOptions<TestOptions> options,
            AppSeviceStudents appSeviceStudents,
            IConfiguration configuration,
            IServiceCollection serviceDescriptors,
            IConfigurationBuilder configurationBuilder

            )
        {
            _Options = options.Value;
            _appSeviceStudents = appSeviceStudents;
            _configuration = configuration;
            _serviceDescriptors = serviceDescriptors;
            _configurationBuilder = configurationBuilder;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await DisPlayTableStudents(context);

            //await context.Response.WriteAsync(_configuration.GetConnectionString("StudentContextConnection"));

            //await DisPlayTableIServiceColection(context);

            //await DisplayJson(context);


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
        public async Task DisPlayForm(HttpContext context)
        {
           
            var methol = "";
            var id = "";
            var st = new Students();
            var action = "";
            Microsoft.Extensions.Primitives.StringValues Methol;
            Microsoft.Extensions.Primitives.StringValues Id;
            if (context.Request.Query.TryGetValue("Methol", out Methol))
            {
                methol = Methol.FirstOrDefault();
                
            }
            if(context.Request.Query.TryGetValue("Id", out Id))
            {
                id = Id.FirstOrDefault();
                st = await _appSeviceStudents.GetStudents(new Guid(id));
                action = $"?Id={id}";
                methol = "POST";
            }
            else
            {
                st = Request(context);
            }
           
            var htmlForm = File.ReadAllText("Form.html").HtmlTag("div", "container");
            var formatform = string.Format(htmlForm, methol, action, st.Name, st.Age);
            string html = HtmlHelper.HtmlDocument("Form", formatform);
    
            await context.Response.WriteAsync(html);
        }



        //Get put post delete Table Stdents
        public Students Request(HttpContext httpContext)
        {
            Students students = null;
            Microsoft.Extensions.Primitives.StringValues Id;
            if (httpContext.Request.Method == "POST" &&
                httpContext.Request.Query.TryGetValue("Id", out Id))
            {
                var id = Id.FirstOrDefault();

                IFormCollection _form = httpContext.Request.Form;
                students = new Students()
                {
                    Id = new Guid(id),
                    Name = _form["Name"].FirstOrDefault() ?? "",
                    Age = int.Parse(_form["Age"].FirstOrDefault())
                };
            }
            else if (httpContext.Request.Method == "POST")
            {
                IFormCollection _form = httpContext.Request.Form;
                students = new Students()
                {
                    Name = _form["Name"].FirstOrDefault() ?? "",
                    Age = int.Parse(_form["Age"].FirstOrDefault())
                };
            }

           return students ?? new Students()
            {
                
                Name = "",
                Age = 0,
            };

        }
        public async Task DisPlayTableStudents(HttpContext context)
        {
            Microsoft.Extensions.Primitives.StringValues Id;
            if (context.Request.Method == "POST" &&
                context.Request.Query.TryGetValue("Id", out Id))
            {
                var st = Request(context);
                await _appSeviceStudents.Rename(st);
            }
            else if (context.Request.Method == "POST")
            {

                var st = Request(context);
                await _appSeviceStudents.InsertStudent(st);
            }

            else if (context.Request.Query.TryGetValue("Id", out Id))
            {
                string queryVal = Id.FirstOrDefault();     
                await _appSeviceStudents.DeleteStudent(new Guid(queryVal));
            }

            var sb = new StringBuilder();
            var listStudents = await _appSeviceStudents.ReadStudents();
            sb.Append(("Id".th() + "Name".th() + "Age".th()) + "Options".tr());
            foreach (var item in listStudents)
            {
                sb.Append((item.Id.ToString().td() +
                    item.Name.ToString().td() +
                    item.Age.ToString().td() +
                    "Delete".a($"?id={item.Id}", "btn btn-danger").td()+
                    "Rename".a($"/Form?Methol=POST&Id={item.Id}", "btn btn-danger").td())
                   .tr());
            }
            var buttonAdd = "Add".a($"/Form?Methol=POST", "btn btn-danger").HtmlTag("div", "container");
            string info = ("Danh Sach Students ".HtmlTag("h2") + sb.ToString().table("table table-sm table-bordered")).HtmlTag("div", "container");

            string html = HtmlHelper.HtmlDocument("TableStudent", info + buttonAdd);

            await context.Response.WriteAsync(html);
        }
    }
}
