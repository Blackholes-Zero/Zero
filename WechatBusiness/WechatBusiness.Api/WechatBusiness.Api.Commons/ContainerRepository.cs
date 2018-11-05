using log4net.Repository;
using Microsoft.Extensions.Configuration;
using System;

namespace WechatBusiness.Api.Commons
{
    public class ContainerRepository
    {
        public static ILoggerRepository Log4NetRepository { get; set; }

        public static IConfiguration ConfigurationRepository { get; set; }

        public static IServiceProvider AppServicesRepository { get; set; }
}
}
