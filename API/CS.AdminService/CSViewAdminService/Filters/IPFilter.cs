﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSViewAdminService.Filters
{
    /// <summary>
    /// 请求过滤器，以确保请求IP在信任列表中
    /// </summary>
    public class IPFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 控制器中的操作执行之前调用此方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
         {
            //if (!AppsettingsConfig.RquestAccess(actionContext))
            //{
            //    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
            //    {
            //        Content = new StringContent("警告！非法请求！")
            //    };
            //    return;
            //}
            base.OnActionExecuting(actionContext);
        }
    }
}
