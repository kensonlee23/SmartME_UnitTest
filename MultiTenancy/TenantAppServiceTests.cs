using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.MultiTenancy;
using com.trustmechain.SmartME.Core.Authorization.Roles;
using com.trustmechain.SmartME.Core.Authorization.Users;
using com.trustmechain.SmartME.Core.Editions;
using com.trustmechain.SmartME.Core.MultiTenancy;
using com.trustmechain.SmartME.Application.MultiTenancy.Dto;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Application.Services.Dto;

namespace com.trustmechain.SmartME.Tests.MultiTenancy
{
    public class TenantAppServiceTests : SmartMETestBase
    {
        private IRepository<Tenant, int> subRepository;
        private TenantManager subTenantManager;
        private EditionManager subEditionManager;
        private UserManager subUserManager;
        private RoleManager subRoleManager;
        private IAbpZeroDbMigrator subAbpZeroDbMigrator;

        public TenantAppServiceTests()
        {
            this.subRepository = Resolve<IRepository<Tenant, int>>();
            this.subTenantManager = Resolve<TenantManager>();
            this.subEditionManager = Resolve<EditionManager>();
            this.subUserManager = Resolve<UserManager>();
            this.subRoleManager = Resolve<RoleManager>();
            this.subAbpZeroDbMigrator = Resolve<IAbpZeroDbMigrator>();
        }

        private TenantAppService CreateService()
        {
            return new TenantAppService(
                this.subRepository,
                this.subTenantManager,
                this.subEditionManager,
                this.subUserManager,
                this.subRoleManager,
                this.subAbpZeroDbMigrator);
        }

        [Fact]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            CreateTenantDto input1 = new CreateTenantDto
            {
                AdminEmailAddress = null,
                ConnectionString = null,
                Name = null,
                TenancyName = null,
                IsActive = true
            };

            CreateTenantDto input2 = new CreateTenantDto
            {
                AdminEmailAddress = "AdminEmailAddress",
                ConnectionString = null,
                Name = null,
                TenancyName = null,
                IsActive = true
            };

            CreateTenantDto input3 = new CreateTenantDto
            {
                AdminEmailAddress = "AdminEmailAddress",
                ConnectionString = "connectionString",
                Name = null,
                TenancyName = null,
                IsActive = true
            };

            CreateTenantDto input4 = new CreateTenantDto
            {
                AdminEmailAddress = "AdminEmailAddress",
                ConnectionString = "connectionString",
                Name = "name2",
                TenancyName = null,
                IsActive = true
            };

            CreateTenantDto input5 = new CreateTenantDto
            {
                AdminEmailAddress = "AdminEmailAddress",
                ConnectionString = "connectionString",
                Name = "name2",
                TenancyName = "tenancyname",
                IsActive = true
            };

            // Act
            var result1 = service.Create(
                input1);
            var result2 = service.Create(
                input2);
            var result3 = service.Create(
                input3);
            var result4 = service.Create(
                input4);
            var result5 = service.Create(
                input5);

            // Assert
            Assert.True(result1.Status == TaskStatus.Faulted);
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.Faulted);
            Assert.True(result5.Status == TaskStatus.RanToCompletion && result5.Result != null);  //exception: "Relational-specific methods can only be used when the context is using a relational database provider."
        }

        [Fact]
        public void Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            EntityDto input1 = new EntityDto { Id = 1 };
            EntityDto input2 = new EntityDto { Id = 999 };


            // Act
            var result1 = service.Delete(
                input1);
            var result2 = service.Delete(
                input2);


            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion);
            Assert.True(result2.Status == TaskStatus.Faulted);
        }
    }
}
