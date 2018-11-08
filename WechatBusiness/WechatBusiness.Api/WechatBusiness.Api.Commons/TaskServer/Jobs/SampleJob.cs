using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Text;

namespace WechatBusiness.Api.Commons.TaskServer.Jobs
{
    public class SampleJob : BaseJob, IJob
    {
        public void Execute()
        {
            //Trace.WriteLine("开始定时任务了，现在时间是：" + DateTime.Now);

            Log4NetProvider.Error(typeof(SampleJob), "开始定时任务了，现在时间是：" + DateTime.Now);
        }
    }
}