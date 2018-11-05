using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using WechatBusiness.Api.Commons;
using WechatBusiness.IService;

namespace WechatBusiness.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<IBaseService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                //.PropertiesAutowired()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(AopInterceptor)); ;
        }
    }
}