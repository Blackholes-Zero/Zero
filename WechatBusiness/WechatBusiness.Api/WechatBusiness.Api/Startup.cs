using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Castle.DynamicProxy;
using IdentityServer4.AccessTokenValidation;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using NetCore.Framework.Cache;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using WechatBusiness.Api.AutoMappingProfiles;
using WechatBusiness.Api.Commons;
using WechatBusiness.Api.Commons.AppSetting;
using WechatBusiness.Api.Controllers;
using WechatBusiness.Api.Filter;
using WechatBusiness.DataSource;
using WechatBusiness.IRepository;
using WechatBusiness.IService;
using WechatBusiness.Repository.RepositoryBase;
using WechatBusiness.Repository.UnitOfWork;
using WechatBusiness.Service;

namespace WechatBusiness.Api
{
    public class Startup
    {
        //IConfiguration configuration
        public Startup(IHostingEnvironment env)
        {
            //Configuration = configuration;
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddJsonFile("Configs/autofac.json")
               .AddEnvironmentVariables();
            Configuration = builder.Build();
            //log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(Directory.GetCurrentDirectory() + @"/Configs/log4net.config"));
            ContainerRepository.Log4NetRepository = logRepository;
        }

        public IConfigurationRoot Configuration { get; }

        public IContainer Container { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            /*ef*/
            services.AddDbContext<EfDbContext>(option => option.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//配置sqlserver
            //services.AddScoped<IUnitOfWork, UnitOfWork<EfDbContext>>();//注入UOW依赖，确保每次请求都是同一个对象
            //依赖注入 若出现重复注册的问题，请使用AddTransient
            //services.AddTransient<IDatabaseFactory, DatabaseFactory>();

            /*注入httpContext*/
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IConfigurationRoot>(Configuration);

            /*跨域访问*/

            #region 跨域

            var urls = Configuration.GetSection("CoresSettings:Urls").Value.Split(',');
            services.AddCors(options =>
            options.AddPolicy("CoresDomain", blder =>
             blder.WithOrigins(urls)
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowAnyOrigin()
             .AllowCredentials())
             );

            #endregion 跨域

            //identityServer4
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetSection("IdentityServerSettings:Authority").Value;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = Configuration.GetSection("IdentityServerSettings:ApiName").Value;
                });

            /*https://www.cnblogs.com/daxnet/p/6181366.html*/
            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Netcore.Api接口文档",
                    Description = "RESTful API for Netcore.Api",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Alvin_Su", Email = "939391793@qq.com", Url = "" }
                });

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "WechatBusiness.Api.xml");
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();
                c.IgnoreObsoleteActions();
                //identityServer4
                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "password",
                    //AuthorizationUrl = "http://localhost:5006",
                    TokenUrl = string.Concat(Configuration.GetSection("IdentityServerSettings:Authority").Value.TrimEnd('/')
                    , Configuration.GetSection("IdentityServerSettings:TokenUrl").Value),//  "http://auth.handnear.com/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "Api", "secret" }
                    },
                });
                c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
            });

            //Area
            var baseController = typeof(BaseController);
            var controllerAssembly = baseController.GetTypeInfo().Assembly;
            services.AddMvc(options =>
            {
                options.Filters.Add<ApiResourceFilter>();
                options.Filters.Add<ApiExceptionFilter>();
                options.Filters.Add<ApiActionFilter>();
                options.Filters.Add(new ApiAuthorizationFilter());

                //支持原始输入
                options.InputFormatters.Insert(0, new RawRequestBodyFormatter());

                //xml
                //options.ReturnHttpNotAcceptable = true;
                //options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                //options.InputFormatters.Add(new XmlSerializerInputFormatter());
                //options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                //options.Conventions.Add(new CommandParameterBindingConvention());
            }).ConfigureApplicationPartManager(m =>
            {
                var feature = new ControllerFeature();
                m.ApplicationParts.Add(new AssemblyPart(controllerAssembly));
                m.PopulateFeature(feature);
                services.AddSingleton(feature.Controllers.Select(t => t.AsType()).ToArray());
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //默认返回json为小写，设置后不会变换model字段的大小写
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //automapper
            services.AddAutoMapper(typeof(IProfile));

            //redis
            var redisAppSet = Configuration.GetSection("RedisSettings").Get<RedisSettings>();
            if (redisAppSet.IsEnabled)
            {
                var redisOption = new RedisCacheOptions()
                {
                    Configuration = redisAppSet.Connection,
                    InstanceName = redisAppSet.InstanceName
                };
                services.AddSingleton<ICacheHelper>(new RedisCacheHelper(redisOption, redisAppSet.Database ?? 0));
            }
            else//MemoryCache
            {
                services.AddSingleton<ICacheHelper>(new MemoryCacheHelper(new MemoryCacheOptions()));
            }

            //autofac
            var builder = new ContainerBuilder();

            //builder.RegisterType<AccountService>().As<IAccountService>();
            //builder.RegisterGeneric(typeof(EfRepositoryBase<>)).As(typeof(IRepository<>)).InstancePerDependency();//注册仓储泛型
            builder.Populate(services);
            var module = new ConfigurationModule(Configuration);
            builder.Register(c => new AopInterceptor());
            builder.RegisterModule(module);
            this.Container = builder.Build();

            return new AutofacServiceProvider(this.Container); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //config
            ContainerRepository.ConfigurationRepository = Configuration;

            /*
             DI资料：https://www.cnblogs.com/xishuai/p/asp-net-core-ioc-di-get-service.html
             */
            ContainerRepository.AppServicesRepository = app.ApplicationServices;

            app.UseHttpsRedirection();

            //注入，文件路径更换
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(filePath),
                RequestPath = new PathString("/src")
            });

            //Mappings.RegisterMappings();
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.AddProfile<MappingProfile>();
            //});

            //跨域访问
            app.UseCors("CoresDomain");

            //IdentityServer4
            app.UseAuthentication();
            app.Use((context, next) =>
            {
                var user = context.User;

                context.Response.StatusCode = user.Identity.IsAuthenticated ? 200 : 401;

                return next.Invoke();
            });

            //Swagger
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCore.Api API V1");
                //c.ShowRequestHeaders();
            });

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //   name: "{area:exists}/",
                //   template: "{area:exists}/{controller=Default}/{action=Index}/{id?}");

                routes.MapRoute(
                   name: "Default",
                   template: "api/{controller}/{action}/{id?}",
                   defaults: new { controller = "Values", action = "Get" }
                   );
            });

            //autofac
            appLifetime.ApplicationStopped.Register(() => this.Container.Dispose());
        }
    }
}