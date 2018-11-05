﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WechatBusiness.Api.Commons;
using WechatBusiness.Api.ViewModels;

namespace WechatBusiness.Api.Filter
{
    public class ApiActionFilter : IActionFilter
    {
        private LogModel logmodel = new LogModel();

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var httpContext = context.HttpContext;
            var stopwach = httpContext.Items["StopwachKey"] as Stopwatch;
            stopwach.Stop();

            var controller = context.Controller as ControllerBase;
            logmodel.ResultValue = JsonConvert.SerializeObject(context.Result);
            logmodel.InputTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var time = stopwach.Elapsed;
            logmodel.ExecTime = time.TotalSeconds.ToString();
            if (time.TotalSeconds > 5)
            {
                //添加日志
                Log4NetProvider.Info(context.ActionDescriptor, JsonConvert.SerializeObject(logmodel));
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //验证数据
            if (!context.ModelState.IsValid)
            {
                var modelState = context.ModelState.FirstOrDefault(f => f.Value.Errors.Any());
                string errorMsg = modelState.Value.Errors.First().ErrorMessage;
                throw new Exception(errorMsg);
            }

            //日志
            var controller = context.Controller as ControllerBase;
            logmodel.Id = Guid.NewGuid().ToString();
            logmodel.Namespace = controller.GetType().Namespace;
            logmodel.ClassName = controller.GetType().Name;
            logmodel.MethodName = controller.HttpContext.Request.GetAbsoluteUri();//string.Join("/", controller.RouteData.Values.Values);
            logmodel.Parameter = JsonConvert.SerializeObject(controller.HttpContext.Request.Query);
            logmodel.LogType = "1";
            logmodel.Ip = controller.HttpContext.Connection.RemoteIpAddress.ToString();
            logmodel.Source = "web";

            var stopwach = new Stopwatch();
            stopwach.Start();
            context.HttpContext.Items.Add("StopwachKey", stopwach);
        }
    }

    //获取ip
    public static class HttpRequestExtensions
    {
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }
    }
}