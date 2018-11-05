using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using WechatBusiness.Api.Commons;
using WechatBusiness.DataSource;
using WechatBusiness.Entities;
using WechatBusiness.IRepository;
using WechatBusiness.Repository.RepositoryBase;
using WechatBusiness.Repository.UnitOfWork;

namespace WechatBusiness.Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfUnitOfWork<EfDbContext>>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<DatabaseFactory>()
                .As<IDatabaseFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<IBaseRepository>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                //.PropertiesAutowired()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(AopInterceptor)); ;
        }
    }
}