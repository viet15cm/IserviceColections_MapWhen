﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IserviceColections_MapWhen
{
    public static class HtmlHelper
    {
        /// <summary>
        /// Phát sinh trang HTML
        /// </summary>
        /// <param name="title">Tiêu đề trang</param>
        /// <param name="content">Nội dung trong thẻ body</param>
        /// <returns>Trang HTML</returns>
        public static string HtmlDocument(string title, string content)
        {
            return $@"
                    <!DOCTYPE html>
                    <html>
                        <head>
                            <meta charset=""UTF-8"">
                            <title>{title}</title>
                            <!-- CSS only -->
                            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.1.0/dist/css/bootstrap.min.css"" 
                                    rel=""stylesheet"" 
                                    integrity=""sha384-KyZXEAg3QhqLMpG8r+8fhAXLRk2vvoC2f3B09zVXn8CA5QIVfZOJ3BCsw2P0p/We"" 
                                    crossorigin=""anonymous"">
                        </head>
                        <body>
                            {content}
                        </body>
                    </html>";
        }


        /// <summary>
        /// Phát sinh HTML thanh menu trên, menu nào  active phụ thuộc vào URL mà request gủi đến
        /// </summary>
        /// <param name="menus">Mảng các menu item, mỗi item có cấu trúc {url, lable}</param>
        /// <param name="request">HttpRequest</param>
        /// <returns></returns>

        public static string MenuTop(object[] menus, HttpRequest request)
        {

            var menubuilder = new StringBuilder();
            menubuilder.Append("<ul class=\"navbar-nav\">");
            foreach (dynamic menu in menus)
            {
                string _class = "nav-item";
                // Active khi request.PathBase giống url của menu
                if (request.Path == menu.url) _class += " active";
                menubuilder.Append($@"
                                <li class=""{_class}"">
                                    <a class=""nav-link"" href=""{menu.url}"">{menu.label}</a>
                                </li>
                                ");
            }
            menubuilder.Append("</ul>\n");

            string menuhtml = $@"
                    <div class=""container"">
                        <nav class=""navbar navbar-expand-lg navbar-light bg-light"">
                            <a class=""navbar-brand"" href=""/"">Quoc Viet IT</a>
                            <button class=""navbar-toggler"" type=""button""
                                data-toggle=""collapse"" data-target=""#my-nav-bar""
                                aria-controls=""my-nav-bar"" aria-expanded=""false"" aria-label=""Toggle navigation"">
                                <span class=""navbar-toggler-icon""></span>
                            </button>
                            <div class=""collapse navbar-collapse"" id=""navbarNav"">
                                { menubuilder}
                            </div>
                    </nav></div>";



            return menuhtml;
        }

        /// <summary>
        /// Những menu item mặc định cho trang
        /// </summary>
        /// <returns>Mảng các menuitem</returns>
        public static object[] DefaultMenuTopItems()
        {
            return new object[] {
              new {
                  url = "/Admin/RequestInfo",
                  label = "Request"
              },
              new {
                  url = "/Admin/Form",
                  label = "Form"
              }
              ,
              new {
                  url = "/Admin/Encoding",
                  label = "Encoding"
              },
              new {
                  url = "/Admin/Cookies",
                  label = "Cookies"
              },
              new {
                  url = "/Admin/Json",
                  label = "JSON"
              }
          };
        }

        public static string HtmlTrangchu()
        {
            return $@"
          <div class=""container"">
            <div class=""jumbotron"">
                <h1 class=""display-4"">Đây là một trang Web .NET Core</h1>
                <p class=""lead"">Trang Web này xây dựng trên nền tảng  <code>.NET Core</code>,
                chưa sử dụng kỹ thuật MVC - nhằm mục đích học tập.
                Mã nguồn trang này tại <a target=""_blank""
                    href=""https://github.com/xuanthulabnet/learn-cs-netcore/blob/master/ASP_NET_CORE/03.RequestResponse/"">
                    Mã nguồn Ví dụ</a>
                
                </p>
                <hr class=""my-4"">
                <p><code>.NET Core</code> là một hệ thống chạy đa nền tảng (Windows, Linux, macOS)</p>
                <a class=""btn btn-danger btn-lg"" href=""https://xuanthulab.net/lap-trinh-c-co-ban/"" role=""button"">Xem thêm</a>
            </div>
        </div>
         ";

        }

        // Mở rộng String, phát sinh thẻ HTML với nội dụng là String
        // Ví dụ: 
        // "content".HtmlTag() => <p>content</p>
        // "content".HtmlTag("div", "text-danger") => <div class="text-danger">content</div>
        public static string HtmlTag(this string content, string tag = "p", string _class = null)
        {
            string cls = (_class != null) ? $" class=\"{_class}\"" : null;
            return $"<{tag + cls}>{content}</{tag}>";
        }
        public static string td(this string content, string _class = null)
        {
            return content.HtmlTag("td", _class);
        }
        public static string tr(this string content, string _class = null)
        {
            return content.HtmlTag("tr", _class);
        }
        public static string th(this string content, string _class = null)
        {
            return content.HtmlTag("th", _class);
        }
        public static string table(this string content, string _class = null)
        {
            return content.HtmlTag("table", _class);
        }

        public static string a(this string content, string href = "#", string _class = null, string tag = "a")
        {
            string cls = (_class != null) ? $" class=\"{_class}\"" : null;
            string hf = (href != "#") ? $"href =\"{href}\"" : "#";
            return $"<{tag + cls + hf }>{content}</{tag}>";
        }
    }
}