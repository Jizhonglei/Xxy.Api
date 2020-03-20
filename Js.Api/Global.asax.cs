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
    /// ��������
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// ִ�з���
        /// </summary>
        protected void Application_Start()
        {
            //��ʼ�������ļ�
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
            // ע��api������Ҫʹ��HTTPConfiguration����
            HttpConfiguration config = GlobalConfiguration.Configuration;
            IocRegister.Instance.Registers("Js.");
            // api�Ŀ�����������autofac������
            config.DependencyResolver = new AutofacWebApiDependencyResolver(IocRegister.Instance.Container);
            // ������������autofac������
            DependencyResolver.SetResolver(new AutofacDependencyResolver(IocRegister.Instance.Container));
            
        }
    }
}
