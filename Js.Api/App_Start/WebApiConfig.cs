using Js.Api.App_Start.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Js.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            //注册api验证
            config.Filters.Add(new WebApiAuthorizeAttribute());
            //注册异常接管
            config.Filters.Add(new WebApiErrorAttribute());
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
