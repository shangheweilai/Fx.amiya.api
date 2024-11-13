using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fx.Amiya.Background.Api.Filters;
using Fx.Amiya.Background.Api.Vo.CallRecord;
using Fx.Amiya.Common;
using Fx.Amiya.Core.Services;
using Fx.Amiya.SyncFeishuMultidimensionalTable;
using Fx.Amiya.SyncOrder.TikTok;
using Fx.Amiya.SyncOrder.Tmall;
using Fx.Amiya.SyncOrder.WeChatVideo;
using Fx.Amiya.SyncOrder.WeiFenXiao;
using Fx.Authentication;
using Fx.Authorization;
using Fx.Identity.Core;
using Fx.Infrastructure.DataAccess.Mongodb.Standard;
using Fx.Open.Infrastructure.Web;
using Fx.Open.Infrastructure.Web.Filters;
using Fx.Open.Infrastructure.Web.Middleware;
using Fx.Open.Infrastructure.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fx.Amiya.Background.Api
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
            services.AddControllers().AddFxValidateModelFilter();
            

            services.AddFxSwagger("Fx.Amiya.Background.Api.xml", "啊美雅微信后台API");
            #region 配置全局路由
            //在各个控制器添加前缀
            services.AddMvc(opt =>
            {
                opt.UseCentralRoutePrefix(new RouteAttribute("/amiyabg"));
            });
            #endregion
            services.AddFxAmiyaCommonServices();
            services.AddHostedService<FxAmiyaBackgroundInitializer>();
            services.AddTimedJob();
            services.AddScoped<IMongoRepository<CallRecordVo>, FxCallRecordMongoRepository>();
            services.AddFxAmiyaSyncTmallOrderService();
            services.AddFxAmiyaSyncWeiFenXiaoOrderService();
            services.AddFxAmiyaSyncTikTokOrderService();
            services.AddFxAmiyaSyncWeChatVideoOrderService();
            services.AddFeishuSyncMultidimensionalTableService();
            FxAmiyaModulesRegister.RegisteModules(services);

            services.AddFxJwtTokenAuthenticationServices();
            services.AddScoped<IFxIdentityRepository, FxIdentityRepository>();
            services.AddFxAuthroizationServices();
            services.AddSingleton<IOffcialWebUserSessionStorage,OfficialWebUserSessionStorage>();
            services.AddSingleton<ValidateLoginAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(b => b.WithOrigins("*").WithHeaders("*").WithMethods("*"));
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            var enableApiDoc = Configuration.GetValue<bool?>("FxOpen:Params:EnableApiDoc");
            if (enableApiDoc == true)
            {
                app.UseFxSwagger();
            }
            app.UseTimedJob();
            app.UseFxExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

          

        }
    }
}
