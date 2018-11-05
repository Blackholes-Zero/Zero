using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WechatBusiness.Api.ViewModels;

namespace WechatBusiness.Api.Commons
{
    public class AopInterceptor : IInterceptor
    {
        private LogModel logmodel = new LogModel();

        public void Intercept(IInvocation invocation)
        {
            logmodel.Id = Guid.NewGuid().ToString();
            logmodel.Namespace = invocation.TargetType.Namespace;
            logmodel.ClassName = invocation.TargetType.Name;
            logmodel.MethodName = invocation.Method.Name;
            logmodel.Parameter = JsonConvert.SerializeObject(invocation.Arguments); //invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())
            logmodel.LogType = "1";
            logmodel.Ip = "";
            logmodel.Source = "web";

            Stopwatch sw = new Stopwatch();
            sw.Start();
            invocation.Proceed();
            sw.Stop();
            TimeSpan tspan = sw.Elapsed;

            try
            {
                logmodel.ResultValue = JsonConvert.SerializeObject(invocation.ReturnValue);
            }
            catch (Exception)
            {
                logmodel.ResultValue = invocation.ReturnValue.ToString();
            }

            logmodel.InputTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            logmodel.ExecTime = tspan.Milliseconds.ToString();

            Log4NetProvider.Info(invocation.TargetType, JsonConvert.SerializeObject(logmodel));
        }
    }
}