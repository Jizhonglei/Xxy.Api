using IFramework.Ioc;
using Js.Api.App_Start.Skip;
using Js.Api.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Js.Api.App_Start.Filters
{
    /// <summary>
    /// WebApi请求前验证
    /// </summary>
    public class WebApiAuthorizeAttribute : AuthorizeAttribute
    {
        private const string _SIGN = "Sign";
        private const string _TOKEN = "Token";

        /// <summary>
        /// 请求前验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Method.Method == "POST") //Ajax请求
            {
                //判断是否需要签名验证
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<SIgnAttribute>();
                bool isSIgn = attributes.Any(a => a is SIgnAttribute);
                //当没有加上skip的api都需要验证签名
                if (!isSIgn)
                {
                    #region 获取请求参数，通过读取流的方式
                    //获取请求参数，通过读取流的方式
                    var inputStream = actionContext.Request.Content.ReadAsStreamAsync().Result;
                    var reader = new System.IO.StreamReader(inputStream);
                    var result = reader.ReadToEnd();
                    actionContext.Request.Content.ReadAsStreamAsync().Result.Position = 0;
                    var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                    #endregion

                    #region 验证签名
                    //验证签名
                    if (obj[_SIGN] != null && !string.IsNullOrEmpty(obj[_SIGN].ToString()))
                    {
                        //var serverSign = JieclSKB.Utilities.Helper.SigningHelper.CreateSign(JsonConvert.SerializeObject(obj));
                        //if (serverSign != obj[_SIGN].ToString())
                        //    throw new Exception("签名错误");
                    }
                    else
                    {
                        //  throw new Exception("签名错误");
                    }
                    #endregion

                    #region 验证token
                    //验证token
                    var loginAttributes = actionContext.ActionDescriptor.GetCustomAttributes<ApiLoginAttribute>();
                    bool isLogin = loginAttributes.Any(c => c is ApiLoginAttribute);
                    if (!isLogin)
                    {
                        if (obj[_TOKEN] != null && !string.IsNullOrEmpty(obj[_TOKEN].ToString()))
                        {
                            var userService = IocManager.Resolve<Domain.IWebUserService>();
                            var info = userService.GetUserInfo(obj[_TOKEN].ToString());
                            //var merchant = PayLoginInfoLogic.Instance.GetMerchantInfo(obj[_TOKEN].ToString());
                            //if (merchant == null)
                            //    throw new Exception("Token验证失败");
                            //else
                            //    ApiOperateContext.Current.SetLoginUser(merchant);
                        }
                        else
                        {
                            throw new Exception("无效的token");

                        }
                    }
                    #endregion
                }

            }
        }
    }
}