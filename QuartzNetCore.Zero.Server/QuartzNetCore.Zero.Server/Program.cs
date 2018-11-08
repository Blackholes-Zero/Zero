using log4net.Config;
using QuartzNetCore.Zero.Server.Service;
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
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

            HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();

                x.SetDescription("测试服务调度");
                x.SetDisplayName("QuartzNetCore.Zero.Server");
                x.SetServiceName("QuartzNetCore.Zero.Server");

                x.Service(factory =>
                {
                    QuartzServer server = QuartzServerFactory.CreateServer();
                    server.Initialize().GetAwaiter().GetResult();
                    return server;
                });
            });
            Console.WriteLine("Hello World!");
        }
    }
}