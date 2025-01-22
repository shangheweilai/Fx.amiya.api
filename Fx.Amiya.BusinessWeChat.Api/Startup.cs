using Fx.Amiya.BusinessWechat.Api.Vo.CallRecord;
using Fx.Amiya.Common;
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
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fx.Amiya.BusinessWechat.Api
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
            services.AddFxSwagger("Fx.Amiya.BusinessWeChat.Api.xml", "啊美雅企业微信API");
            //在各个控制器添加前缀
            //services.AddMvc(opt =>
            //{
            //    opt.UseCentralRoutePrefix(new RouteAttribute("/businessWechat"));
            //});
            services.AddFxAmiyaCommonServices();
            services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:5630")));

            services.AddScoped<IMongoRepository<CallRecordVo>, FxCallRecordMongoRepository>();
            services.AddFxJwtTokenAuthenticationServices();
            services.AddScoped<IFxIdentityRepository, FxIdentityRepository>();
            services.AddFxAuthroizationServices();
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
            app.UseFxExceptionHandler();
            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<MessageHub>("/hub");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors("cors");

        }
    }
}
