using log4net.Config;
using Quartz;
using Quartz.Impl;
using QuartzNetCore.Zero.Server.Jobs;
using QuartzNetCore.Zero.Server.Service;
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using Topshelf;

namespace QuartzNetCore.Zero.Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //ServiceBase[] services = new ServiceBase[] { new WinService() };
            //ServiceBase.Run(services);
            // TODO: 在此处添加代码以启动服务。
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            //HostFactory.Run(x =>
            //{
            //    x.RunAsLocalSystem();

            //    x.SetDescription("测试服务调度");
            //    x.SetDisplayName("QuartzNetCore.Zero.Server");
            //    x.SetServiceName("QuartzNetCore.Zero.Server");

            //    x.Service(factory =>
            //    {
            //        QuartzServer server = QuartzServerFactory.CreateServer();
            //        server.Initialize().GetAwaiter().GetResult();
            //        return server;
            //    });
            //});

            Task<IScheduler> taskScheduler;
            IScheduler scheduler1;
            //调度器工厂
            ISchedulerFactory factory1 = new StdSchedulerFactory();

            //创建一个调度器
            taskScheduler = factory1.GetScheduler();
            scheduler1 = taskScheduler.Result;

            IJobDetail job = JobBuilder.Create<TestJob>().WithIdentity("job1", "group1").Build();

            //3、创建一个触发器

            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("trigger1", "group1")
            //    .WithCronSchedule("0/5 * * * * ?")     //5秒执行一次
            //    .Build();

            //3.1另外一种触发器
            ISimpleTrigger trigger1 = (ISimpleTrigger)TriggerBuilder.Create()
                   .WithIdentity("trigger1", "group1")
                   .StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()).Build();

            //4、将任务与触发器添加到调度器中
            //scheduler1.ScheduleJob(job, trigger);
            scheduler1.ScheduleJob(job, trigger1);
            //5、开始执行
            scheduler1.Start();

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}