using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetCore.Zero.Server.Jobs
{
    public class TestJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TestJob));

        public async Task Execute(IJobExecutionContext context)
        {
            logger.Info("SampleJob running...");
            await Task.Delay(TimeSpan.FromSeconds(5));
            logger.Info("SampleJob run finished.");
        }
    }
}