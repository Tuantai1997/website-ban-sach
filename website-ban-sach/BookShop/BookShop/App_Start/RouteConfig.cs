using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
      new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
          //  routes.MapRoute(
          //     name: "Product Category",
          //     url: "san-pham/{metatitle}-{cateId}",
          //     defaults: new { controller = "Book", action = "Category", id = UrlParameter.Optional },
          //     namespaces: new[] { "BookShop.Controllers" }
          // );
          //  routes.MapRoute(
          //    name: "Product Detail",
          //    url: "chi-tiet/{metatitle}-{id}",
          //    defaults: new { controller = "Book", action = "Detail", id = UrlParameter.Optional },
          //    namespaces: new[] { "BookShop.Controllers" }
          //);
            routes.MapRoute(
             name: "Tags",
             url: "tag/{tagId}",
             defaults: new { controller = "Content", action = "Tag", id = UrlParameter.Optional },
             namespaces: new[] { "BookShop.Controllers" }
         );
            routes.MapRoute(
             name: "Content Detail",
             url: "tin-tuc/{metatitle}-{id}",
             defaults: new { controller = "Content", action = "Detail", id = UrlParameter.Optional },
             namespaces: new[] { "BookShop.Controllers" }
         );
            routes.MapRoute(
             name: "News",
             url: "tin-tuc",
             defaults: new { controller = "Content", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "BookShop.Controllers" }
              );
            routes.MapRoute(
              name: "Search",
              url: "tim-kiem",
              defaults: new { controller = "Book", action = "Search", id = UrlParameter.Optional },
              namespaces: new[] { "BookShop.Controllers" }
               );
            routes.MapRoute(
                name: "Book Category",
                url: "san-pham/{metatitle}-{Ca_id}",
                defaults: new { controller = "Book", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
            );
            routes.MapRoute(
                name: "Author",
                url: "san-pham/{metatitle}-{Au_id}",
                defaults: new { controller = "Book", action = "Author", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
            );
            routes.MapRoute(
                name: "Producer",
                url: "san-pham/{metatitle}-{Pro_id}",
                defaults: new { controller = "Book", action = "Producer", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
            );
            routes.MapRoute(
                name: "Book Detail",
                url: "chi-tiet/{metatitle}-{id}",
                defaults: new { controller = "Book", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
            );
            routes.MapRoute(
               name: "Cart",
               url: "gio-hang",
               defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "BookShop.Controllers" }
           );
            routes.MapRoute(
               name: "Payment",
               url: "thanh-toan",
               defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
               namespaces: new[] { "BookShop.Controllers" }
           );
            routes.MapRoute(
             name: "About",
             url: "gioi-thieu",
             defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "BookShop.Controllers" }
         );
           routes.MapRoute(
            name: "Contact",
            url: "lien-he",
            defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "BookShop.Controllers" }
          );
            routes.MapRoute(
                name: "Add Cart",
                url: "them-gio-hang",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
            );
            routes.MapRoute(
                name: "Payment Success",
                url: "hoan-thanh",
                defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
            );
            routes.MapRoute(
                name: "Payment Fail",
                url: "loi-mua-hang",
                defaults: new { controller = "Cart", action = "Fail", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
            );
            routes.MapRoute(
              name: "Register",
              url: "dang-ky",
              defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
              namespaces: new[] { "BookShop.Controllers" }
          );
            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
                );
            routes.MapRoute(
               name: "Tai Khoan",
               url: "tai-khoan",
               defaults: new { controller = "User", action = "UserProfile", id = UrlParameter.Optional },
               namespaces: new[] { "BookShop.Controllers" }
               );
            routes.MapRoute(
              name: "Login Admin",
              url: "dang-nhap-admin",
              defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "BookShop.Areas.Admin.Controllers" }
              );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BookShop.Controllers" }
            );
           
        }
    }
}
