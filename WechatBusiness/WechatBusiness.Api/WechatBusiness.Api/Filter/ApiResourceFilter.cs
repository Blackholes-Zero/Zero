using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NetCore.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatBusiness.Api.ViewModels;
using WechatBusiness.Api.ViewModels.ResultModels;

namespace WechatBusiness.Api.Filter
{
    public class ApiResourceFilter : IResourceFilter
    {
        private readonly IConfiguration _configuration;

        public ApiResourceFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //throw new NotImplementedException();
            var keyValuePairs = new List<KeyValuePair<string, object>>();
            try
            {
                if (context.HttpContext.Request.Method.ToLower().Equals("post"))
                {
                    if (context.HttpContext.Request?.Form.Count >= 0)
                    {
                        context.HttpContext.Request.Form?.ToList().ForEach(
                      p =>
                      {
                          keyValuePairs.Add(new KeyValuePair<string, object>(p.Key, p.Value));
                      });
                    }
                }
                else
                {
                    if (context.HttpContext.Request?.Query.Count >= 0)
                    {
                        context.HttpContext.Request.Query?.ToList().ForEach(
                      p =>
                      {
                          keyValuePairs.Add(new KeyValuePair<string, object>(p.Key, p.Value));
                      });
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            finally
            {
                //keyValuePairs.AddRange(context.RouteData.Values?.ToList());
                //keyValuePairs.AddRange(context.ActionDescriptor.Parameters);
            }
            if (!ValidateSign(keyValuePairs))
            {
                var result = ApiResultBase.GetInstance(ResultCode.SignValidateFail);
                context.Result = new JsonResult(result);
                //context.HttpContext.Response.Redirect("/api/Error/ResultException?resultcode=1000&message=Validate Sign Fail");
                return;//终止当前请求
            }
        }

        #region 生成签名

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="keyValuePairs"></param>
        private bool ValidateSign(List<KeyValuePair<string, object>> keyValuePairs)
        {
            if (keyValuePairs != null)
            {
                var keyList = new List<string>() { "sign" };
                //创建签名
                var KeyValue = string.Concat(
                string.Join("&",
                keyValuePairs.Where(p => !keyList.Where(c => c == p.Key.ToLower()).Any())
                .OrderBy(p => p.Key)
                .Select(p => string.Concat(p.Key.ToLower(), "=", p.Value)
                ).ToArray()), "&key=", _configuration.GetSection("ApiAccessSettings:Key").Value);
                var sign = EncryptDecrypt.EncryptMD5(KeyValue).ToUpper();
                var signVal = keyValuePairs.FirstOrDefault(p => keyList.Where(c => c == p.Key.ToLower()).Any()).Value;
                return sign.Equals(signVal);
            }
            return false;
        }

        #endregion 生成签名
    }
}