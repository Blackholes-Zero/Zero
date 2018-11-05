using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Auth.AspnetCore.DataAccess.AppSetts;
using Auth.AspnetCore.DataAccess.Dappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passport.AspnetCore.com.IdentityConfig;

namespace Passport.AspnetCore.com
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<SqlConnection>(p => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<DapperAdminUsers>();

            #region Identity

            /*identityServer4 注入*/
            // 采用自己的用户和角色表，用 Dapper 进行访问
            //services.AddIdentity<IdentityUserInfo, IdentityRoleInfo>()
            //.AddUserStore<IdentityUserStore>()
            //.AddRoleStore<IdentityRoleStore>()
            //.AddDefaultTokenProviders();

            // 很多采用的是下面这个方法，本文没有这样用
            //services.AddTransient<IUserStore<IdentityUserInfo>, IdentityUserStore>();
            //services.AddTransient<IRoleStore<IdentityRoleInfo>, IdentityRoleStore>();
            //services.AddTransient<SqlConnection>(p => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddTransient<DapperAdminUser>();

            // IdentityServer4 默认的登录地址是：/account/login
            // 如果不想使用默认的地址，可以将上面一段改为如下配置
            //services.AddIdentityServer(opts =>
            // {
            // opts.UserInteraction = new UserInteractionOptions
            // {
            // LoginUrl = "你想要的地址，默认：/account/login",
            // LoginReturnUrlParameter = "你想要的返回页的参数名，默认：returnUrl"
            // };
            // })
            // .AddDeveloperSigningCredential(filename: "tmpKey.rsa")
            // .AddInMemoryIdentityResources(SSOConfig.IdentityResources)
            // .AddInMemoryApiResources(SSOConfig.GetApiResources(section))
            // .AddInMemoryClients(SSOConfig.GetClients(section))
            // .AddAspNetIdentity<MKUserInfo>();

            // 此处是防止 CSRF 攻击的 Token 相关的名称（不采用默认名称）
            //services.AddAntiforgery(opts => {
            //    opts.Cookie.Name = "_mk_x_c_token";
            //    opts.FormFieldName = "_mk_x_f_token";
            //    opts.HeaderName = "_mk_x_h_token";
            //});

            //services.AddCors(option =>
            //{
            //    option.AddPolicy("default", builder =>
            //    {
            //        builder.AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowAnyOrigin()
            //        .AllowCredentials();
            //    });
            //});

            #endregion Identity

            var config = Configuration.GetSection("IdentityServer").Get<IdentityServer>();

            services.AddIdentityServer()
            .AddDeveloperSigningCredential(filename: "tempkey.rsa")
            .AddInMemoryIdentityResources(Configs.GetIdentityResources())
            .AddInMemoryApiResources(Configs.GetApiResources(config))
            .AddInMemoryClients(Configs.GetClients(config))
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddProfileService<ProfileService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles().UseIdentityServer();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}