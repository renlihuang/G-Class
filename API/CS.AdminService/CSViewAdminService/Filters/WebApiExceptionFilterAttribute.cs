﻿
using CSViewAdminService.Core;
using CSViewAdminService.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CSViewAdminService.Utility.LoggerHelper;

namespace CSViewAdminService.Filters
{
    /// <summary>
    /// api请求异常日志类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute, IActionFilter
    {

        /// <summary>
        /// 控制器中的操作执行之前调用此方法
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items.Add("params", context.ActionArguments);
        }

        /// <summary>
        /// 控制器中的操作执行之后调用此方法
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do nothing
        }
        /// <summary>
        /// 控制器中的操作异常调用此方法
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public override Task OnExceptionAsync(ExceptionContext actionExecutedContext)
        {

            if (actionExecutedContext.Exception != null)
            {
                ///取消操作导致的异常忽略
                if (actionExecutedContext.Exception is OperationCanceledException)
                {
                }
                else
                {
                    WriteErrorAsync(actionExecutedContext);
                }
            }
            HttpResponseResultModel<string> result = new HttpResponseResultModel<string>();
            result.HttpStatusCode = (HttpStatusCode)actionExecutedContext.HttpContext.Response.StatusCode;
            result.IsSuccess = false;
            result.ErrorMessage = "出现异常,请稍后重试";
            result.ExceptionMessage = actionExecutedContext.Exception.ToString();
            actionExecutedContext.Result = new ObjectResult(result);
            return base.OnExceptionAsync(actionExecutedContext);
        }

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="exceptionContext"></param>
        /// <returns></returns>
        private async Task WriteErrorAsync(ExceptionContext exceptionContext)
        {
            WebApiExceptionLogModel logModel = new WebApiExceptionLogModel();
            //获取Action 参数
            
            var items = exceptionContext.HttpContext.Items;
            logModel.ExecuteStartTime = DateTime.Parse(items["executeStartTime"].ToString());
            logModel.ExecuteEndTime = DateTime.Now;
            IDictionary<string, object> actionArguments = null;
            if (items.ContainsKey("params"))
            {
                actionArguments = (IDictionary<string, object>)items["params"];
            }
            logModel.ActionParams = new Dictionary<string, object>(actionArguments);
            logModel.HttpRequestHeaders = exceptionContext.HttpContext.Request.Headers.ToString();
            logModel.HttpRequestPath = exceptionContext.HttpContext.Request.Path;
            logModel.HttpMethod = exceptionContext.HttpContext.Request.Method;
            logModel.ActionName = ((ControllerActionDescriptor)exceptionContext.ActionDescriptor).ActionName;
            logModel.ControllerName = ((ControllerActionDescriptor)exceptionContext.ActionDescriptor).ControllerName;
            logModel.TotalSeconds = (logModel.ExecuteEndTime - logModel.ExecuteStartTime).TotalSeconds;
            logModel.ExceptionMessage = exceptionContext.Exception.ToString();
            logModel.IP = CommonHttpContext.Current.Connection.RemoteIpAddress.ToString();
            logModel.StatusCode = exceptionContext.HttpContext.Response.StatusCode;
            //// 使用MongoDB的话，WebApiExceptionLogModel 需要继承下MongoDBBaseField
            // MongoDbHelper<WebApiExceptionLogModel> helper = new MongoDbHelper<WebApiExceptionLogModel>();
            // helper.Insert(logModel);
            var json = JsonHelper.SerializeObject(logModel);
            //   WriteLogHelper.WriteLogsAsync(json, logModel.ControllerName);
            MyLogger.Logger.Error(json + "\r\n");
        }
    }
}
