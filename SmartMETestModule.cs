using System;
using Castle.MicroKernel.Registration;
using NSubstitute;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Configuration.Startup;
using Abp.Net.Mail;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Abp.Zero.EntityFrameworkCore;
using com.trustmechain.SmartME.Application;
using com.trustmechain.SmartME.EntityFrameworkCore;
using com.trustmechain.SmartME.Tests.DependencyInjection;
using com.trustmechain.SmartME.Core.Authorization.Users;
using Microsoft.Extensions.Configuration;
using com.trustmechain.SmartME.Core.Configuration;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using com.trustmechain.SmartME.Application.SignalRHub;
using Microsoft.AspNetCore.SignalR;

namespace com.trustmechain.SmartME.Tests
{
    [DependsOn(
        typeof(SmartMEApplicationModule),
        typeof(SmartMEEntityFrameworkModule),
        typeof(AbpTestBaseModule)
        )]
    public class SmartMETestModule : AbpModule
    {
        public SmartMETestModule(SmartMEEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            // Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            RegisterFakeService<AbpZeroDbMigrator<SmartMEDbContext>>();

            Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);


            //Configuration.ReplaceService<Microsoft.Extensions.Configuration.IConfiguration, Microsoft.Extensions.Configuration.ConfigurationRoot>();

            Configuration.ReplaceService<Microsoft.Extensions.Configuration.IConfiguration>(() =>
            {

                var _appConfiguration = AppConfigurations.Get(
                   typeof(SmartMETestModule).GetAssembly().GetDirectoryPathOrNull(), System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  );

                IocManager.IocContainer.Register(
                    Component.For<Microsoft.Extensions.Configuration.IConfiguration>().Instance(_appConfiguration)
                );
            });

            Configuration.ReplaceService<Microsoft.AspNetCore.Identity.IPasswordHasher<User>, PasswordHasher<User>>(DependencyLifeStyle.Singleton);

            
            //Configuration.ReplaceService<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            Configuration.ReplaceService<Microsoft.AspNetCore.Http.IHttpContextAccessor>(() =>
              {
                  var _httpContextAccessor = GetHttpContext("/testing/", "localhost:8080");

                  IocManager.IocContainer.Register(
                    Component.For<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Instance(_httpContextAccessor)
                );
              });


            Configuration.ReplaceService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>(() =>
            {
                var mockEnvironment = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
                //...Setup the mock as needed
                mockEnvironment
                    .Setup(m => m.EnvironmentName)
                    .Returns("Hosting:UnitTestEnvironment");

                IocManager.IocContainer.Register(
                  Component.For<Microsoft.AspNetCore.Hosting.IHostingEnvironment>().Instance(mockEnvironment.Object)
              );
            });



            Configuration.ReplaceService<IHubContext<PropertyHub>>(() =>
            {
                Mock<IHubClients> mockClients = new Mock<IHubClients>();
                Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
                mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

                var hubContext = new Mock<IHubContext<PropertyHub>>();
                hubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);

                IocManager.IocContainer.Register(
                Component.For<IHubContext<PropertyHub>>().Instance(hubContext.Object)
            );
            });

            //IocManager.IocContainer.Register(Component.For<IOptions<AppSettings>>()
            //    .UsingFactoryMethod(x => GetAppSettingOptions()));

        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
        }

        private void RegisterFakeService<TService>() where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );
        }

        //private static IOptions<AppSettings> GetAppSettingOptions()
        //{
        //    var services = new ServiceCollection();
        //    services.AddOptions();
        //    var configurationRoot = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
        //    services.Configure<AppSettings>(configurationRoot.GetSection(nameof(AppSettings)).Bind);
        //    var serviceProvider = services.BuildServiceProvider();
        //    var appSettingOptions = serviceProvider.GetRequiredService<IOptions<AppSettings>>();
        //    return appSettingOptions;
        //}


        public static IHttpContextAccessor GetHttpContext(string incomingRequestUrl, string host)
        {
            var context = new DefaultHttpContext();
            context.Request.Path = incomingRequestUrl;
            context.Request.Host = new HostString(host);

            //Do your thing here...

            var obj = new HttpContextAccessor();
            obj.HttpContext = context;
            return obj;
        }
    }
}
