﻿
using CSViewAdminService.Utility;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSViewAdminService.Utility.LoggerHelper;

namespace CSViewAdminService.Filters
{
    /// <summary>
    /// api 日志跟踪类
    /// </summary>
    public class WebApiTrackerAttribute : ActionFilterAttribute
    {
        
        /// <summary>
        /// 控制器方法执行之前执行此方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Items.Add("executeStartTime", DateTime.Now);
            WriteLogAsync(context);
            return base.OnActionExecutionAsync(context, next);

        }

        /// <summary>
        /// 控制器操作结果执行之前调用此方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }

        /// <summary>
        /// 控制器操作结果执行之后调用此方法
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private async Task WriteLogAsync(ActionExecutingContext actionContext)
        {
            var items = actionContext.HttpContext.Items;
            DateTime executeStartTime = DateTime.Parse(items["executeStartTime"].ToString());
             WebApiLogModel logModel = new WebApiLogModel();
            logModel.ExecuteStartTime = executeStartTime;
            logModel.ExecuteEndTime = DateTime.Now;
            //获取Action 参数
            logModel.ActionParams = new Dictionary<string, object>(actionContext.ActionArguments);
            logModel.HttpRequestHeaders = actionContext.HttpContext.Request.Headers.ToString();
            logModel.HttpRequestPath = actionContext.HttpContext.Request.Path;
            logModel.HttpMethod = actionContext.HttpContext.Request.Method;
            logModel.ActionName = ((ControllerActionDescriptor)actionContext.ActionDescriptor).ActionName;
            logModel.ControllerName = ((ControllerActionDescriptor)actionContext.ActionDescriptor).ControllerName;
            logModel.TotalSeconds = (logModel.ExecuteEndTime - logModel.ExecuteStartTime).TotalSeconds;
            logModel.IP = CommonHttpContext.Current.Connection.RemoteIpAddress.ToString();
            //// 使用MongoDB的话，WebApiLogModel 需要继承下MongoDBBaseField
            //   MongoDbHelper<WebApiLogModel> helper = new MongoDbHelper<WebApiLogModel>();
            //  helper.Insert(logModel);
            //var json = JsonHelper.SerializeObject(logModel);
            //MyLogger.Logger.Information(json + "\r\n");

        }
    }
}
