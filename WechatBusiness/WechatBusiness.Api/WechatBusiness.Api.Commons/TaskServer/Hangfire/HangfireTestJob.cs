using System;
using System.Collections.Generic;
using System.Text;

namespace WechatBusiness.Api.Commons.TaskServer.Hangfire
{
    public class HangfireTestJob
    {
        public void Execute()
        {
            Log4NetProvider.Error(typeof(HangfireTestJob), "HangfireTestJob开始定时任务了，现在时间是：" + DateTime.Now);
        }
    }
}