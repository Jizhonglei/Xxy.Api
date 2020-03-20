using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using IFramework.Base;
using IFramework.Infrastructure;
using IFramework.Ioc;
using Js.Domain;
using Js.Web.Models;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Js.Api
{
    /// <summary>
    /// 程序启动
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        protected void Application_Start()
        {
            //初始化配置文件
            AppSetting.Init();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Assembly controllerAss = Assembly.Load("Js.Api");
            IocRegister.Instance.BuilderHandler += build =>
            {
                build.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
                build.RegisterType<UserStateManager>().As<IUserManagerProvider>();
                build.RegisterControllers(controllerAss);
                build.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
                build.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
                build.RegisterWebApiModelBinderProvider();

            };
            // 注册api容器需要使用HTTPConfiguration对象
            HttpConfiguration config = GlobalConfiguration.Configuration;
            IocRegister.Instance.Registers("Js.");
            // api的控制器对象由autofac来创建
            config.DependencyResolver = new AutofacWebApiDependencyResolver(IocRegister.Instance.Container);
            // 控制器对象由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(IocRegister.Instance.Container));
            
        }
    }
}
