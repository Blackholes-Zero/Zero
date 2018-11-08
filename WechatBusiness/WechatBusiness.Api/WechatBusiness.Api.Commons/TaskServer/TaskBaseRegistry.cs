﻿using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Text;
using WechatBusiness.Api.Commons.TaskServer.Jobs;

namespace WechatBusiness.Api.Commons.TaskServer
{
    public class TaskBaseRegistry : Registry
    {
        public TaskBaseRegistry()
        {
            // 立即执行每两秒一次的计划任务。（指定一个时间间隔运行，根据自己需求，可以是秒、分、时、天、月、年等。）
            Schedule<SampleJob>().ToRunNow().AndEvery(2).Seconds();

            //// 延迟一个指定时间间隔执行一次计划任务。（当然，这个间隔依然可以是秒、分、时、天、月、年等。）
            //Schedule<Demo>().ToRunOnceIn(5).Seconds();

            //// 在一个指定时间执行计划任务（最常用。这里是在每天的下午 1:10 分执行）
            //Schedule(() => Trace.WriteLine("It's 1:10 PM now.")).ToRunEvery(1).Days().At(13, 10);

            //// 立即执行一个在每月的星期一 3:00 的计划任务（可以看出来这个一个比较复杂点的时间，它意思是它也能做到！）
            //Schedule<Demo>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

            //// 在同一个计划中执行两个（多个）任务
            //Schedule<Demo>().AndThen<Demo>().ToRunNow().AndEvery(5).Minutes();
        }
    }
}