using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Polly;
using IdentityServer4.AccessTokenValidation;
using Microsoft.OpenApi.Models;
using Eggs.Core.Common;
using Eggs.Core.Gateway.Extensions;
using Microsoft.IdentityModel.Logging;

namespace Eggs.Core.Gateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsSetup();//�������
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Gateway API",
                        Version = "v1",
                        Description = "# gateway api..."
                    });
            });


            services.AddControllers();
            //string AuthenticationProviderKey = "blog.core.api";
            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(AuthenticationProviderKey, options =>
            //    {
            //        options.Authority = $"{Appsettings.app("IdentityServer", "Url")}";//��Ȩ���ĵ�ַ
            //        options.ApiName = "blog.core.api";
            //        options.RequireHttpsMetadata = false;
            //        options.SupportedTokens = SupportedTokens.Jwt;
            //        options.ApiSecret = "api_secret";
            //    });

            //���ocelot����
            services.AddOcelot()
                .AddConsul()
                //��ӻ���
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                //���Polly
                .AddPolly();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(Appsettings.app(new string[] { "Startup", "Cors", "PolicyName" }));
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Blog.Core API V1");
            });

            //����Ocelot�м��
            app.UseOcelot().Wait();

            IdentityModelEventSource.ShowPII = true;//�Ƿ�����Identity����������Ϣ
        }
    }
}
