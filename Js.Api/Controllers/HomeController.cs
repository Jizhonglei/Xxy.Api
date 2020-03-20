using Js.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Js.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
     //   private readonly IAppUserService _appUserService;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="groupService"></param>
        //public HomeController(IAppUserService appUserService)
        //{
        //    _appUserService = appUserService;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {


            // Js.Domain.Template.DomainHelper.MakeDomain("WebUserToken", "WebUserToken", "用户登录凭证表", false, true);
            //Js.Domain.Template.DbSessionFactory.MakeDbSession();

            // _appUserService.Getlist();
            return Redirect("/swagger/ui/index");
        }
    }
}
