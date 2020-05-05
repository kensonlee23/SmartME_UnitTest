using Abp.Domain.Repositories;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.Roles;
using com.trustmechain.SmartME.Core.Authorization.Roles;
using com.trustmechain.SmartME.Core.Authorization.Users;
using com.trustmechain.SmartME.Application.Roles.Dto;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using System.Collections.Generic;
using Abp.Authorization;

namespace com.trustmechain.SmartME.Tests.Roles
{
    public class RoleAppServiceTests : SmartMETestBase
    {
        private IRepository<Role> subRepository;
        private RoleManager subRoleManager;
        private UserManager subUserManager;

        public RoleAppServiceTests()
        {
            this.subRepository = Resolve<IRepository<Role>>();
            this.subRoleManager = Resolve<RoleManager>();
            this.subUserManager = Resolve<UserManager>();
        }

        private RoleAppService CreateService()
        {
            return new RoleAppService(
                this.subRepository,
                this.subRoleManager,
                this.subUserManager);
        }

        [Fact]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.PermissionManager = Resolve<IPermissionManager>();

            CreateRoleDto input1 = new CreateRoleDto
            {
                Name = "role1",
                Description = "description",
                DisplayName = "role1d",
                NormalizedName = "role1n",
                Permissions = new System.Collections.Generic.List<string>() { "role3", "aaa" }
            };

            CreateRoleDto input2 = new CreateRoleDto
            {
                Name = "",
                Description = "",
                DisplayName = "",
                NormalizedName = "",
                Permissions = null
            };

            CreateRoleDto input3 = new CreateRoleDto
            {
                Name = "role3",
                Description = "",
                DisplayName = "",
                NormalizedName = "",
                Permissions = null
            };

            CreateRoleDto input4 = new CreateRoleDto
            {
                Name = "role4",
                Description = "",
                DisplayName = "displayname4",
                NormalizedName = "",
                Permissions = new System.Collections.Generic.List<string>() { "role3", "aaa" }
            };

            CreateRoleDto input5 = new CreateRoleDto
            {
                Name = "role5",
                Description = "description",
                DisplayName = "displayname5",
                NormalizedName = "",
                Permissions = new System.Collections.Generic.List<string>() { "role3", "aaa" }
            };

            CreateRoleDto input6 = new CreateRoleDto
            {
                Name = "role6",
                Description = "description",
                DisplayName = "displayname6",
                NormalizedName = "normalizedname",
                Permissions = new System.Collections.Generic.List<string>() { "role3", "aaa" }
            };

            CreateRoleDto input7 = new CreateRoleDto
            {
                Name = "role7",
                Description = "description",
                DisplayName = "displayname6",
                NormalizedName = "normalizedname",
                Permissions = new System.Collections.Generic.List<string>() { "role3", "aaa" }
            };

            CreateRoleDto input8 = new CreateRoleDto
            {
                Name = "role6",
                Description = "description",
                DisplayName = "displayname7",
                NormalizedName = "normalizedname",
                Permissions = new System.Collections.Generic.List<string>() { "role3", "aaa" }
            };

            CreateRoleDto input9 = new CreateRoleDto
            {
                Name = "role9",
                Description = "",
                DisplayName = "",
                NormalizedName = "",
                Permissions = new System.Collections.Generic.List<string>() { "role3", "aaa" }
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
            var result6 = service.Create(
                input6);
            var result7 = service.Create(
                input7);
            var result8 = service.Create(
                input8);
            var result9 = service.Create(
                input9);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null);
            Assert.True(result2.Status == TaskStatus.Faulted && result2.Exception.InnerException.Message == "Identity.InvalidEmail");
            Assert.True(result3.Status == TaskStatus.Faulted && result3.Exception.InnerException.Message == "Object reference not set to an instance of an object."); // when permission object is null
            Assert.True(result4.Status == TaskStatus.RanToCompletion && result4.Result != null);
            Assert.True(result5.Status == TaskStatus.RanToCompletion && result5.Result != null);
            Assert.True(result6.Status == TaskStatus.RanToCompletion && result6.Result != null);
            Assert.True(result7.Status == TaskStatus.Faulted && result7.Exception.InnerException.Message == "Role display name displayname6 is already taken."); //display name unique
            Assert.True(result8.Status == TaskStatus.Faulted && result8.Exception.InnerException.Message == "Role name role6 is already taken.");
            Assert.True(result9.Status == TaskStatus.Faulted && result9.Exception.InnerException.Message == "Role display name  is already taken.");
        }

        [Fact]
        public void GetRolesAsync_StateUnderTest_ExpectedBehavior()
        {
            LoginAsHost("85261107946");
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();
            GetRolesInput input1 = new GetRolesInput { Permission = "" };
            GetRolesInput input2 = new GetRolesInput { Permission = "role3" };
            GetRolesInput input3 = new GetRolesInput { Permission = "nodata" };
            GetRolesInput input4 = new GetRolesInput { Permission = null };

            // Act
            var result1 = service.GetRolesAsync(
                input1);

            var result2 = service.GetRolesAsync(
                input2);

            var result3 = service.GetRolesAsync(
                input3);

            var result4 = service.GetRolesAsync(
                input4);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Items.Count > 0);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result.Items.Count > 0);
            Assert.True(result3.Status == TaskStatus.RanToCompletion && result3.Result.Items.Count == 0);
            Assert.True(result4.Status == TaskStatus.RanToCompletion && result4.Result.Items.Count > 0);
        }

        [Fact]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            LoginAsHost("85261107946");
            // Arranges
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.PermissionManager = Resolve<IPermissionManager>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            RoleDto input1 = new RoleDto
            {
                Id = 99,
                Description = "desc",
                DisplayName = "dname",
                Name = "name",
                NormalizedName = "nname",
                Permissions = new List<string> { "role3", "nodata" }
            };
            RoleDto input2 = new RoleDto
            {
                Id = 999,
                Description = "",
                DisplayName = "",
                Name = "",
                NormalizedName = "",
                Permissions = null
            };
            RoleDto input3 = new RoleDto
            {
                Id = 99,
                Description = "",
                DisplayName = "",
                Name = "",
                NormalizedName = "",
                Permissions = null
            };

            RoleDto input4 = new RoleDto
            {
                Id = 99,
                Description = "",
                DisplayName = "",
                Name = "role3",
                NormalizedName = "",
                Permissions = null
            };

            RoleDto input5 = new RoleDto
            {
                Id = 99,
                Description = "",
                DisplayName = "",
                Name = "role3",
                NormalizedName = "",
                Permissions = new List<string> { "role3", "nodata" }
            };

            RoleDto input6 = new RoleDto
            {
                Id = 99,
                Description = "desc",
                DisplayName = "",
                Name = "roleps99",
                NormalizedName = "",
                Permissions = new List<string> { "role3", "nodata" }
            };

            RoleDto input7 = new RoleDto
            {
                Id = 99,
                Description = "desc",
                DisplayName = "",
                Name = "role3",
                NormalizedName = "",
                Permissions = new List<string> { "role3", "nodata" }
            };

            RoleDto input8 = new RoleDto
            {
                Id = 99,
                Description = "desc",
                DisplayName = "role3",
                Name = "roleps99",
                NormalizedName = "",
                Permissions = new List<string> { "role3", "nodata" }
            };

            RoleDto input9 = new RoleDto
            {
                Id = 99,
                Description = "desc",
                DisplayName = "role3",
                Name = "role3",
                NormalizedName = "nname",
                Permissions = new List<string> { "role3", "nodata" }
            };

            // Act
            var result1 = service.Update(
                input1);
            var result2 = service.Update(
                input2);
            var result3 = service.Update(
                input3);
            var result4 = service.Update(
                input4);
            var result5 = service.Update(
                input5);
            var result6 = service.Update(
                input6);
            var result7 = service.Update(
                input7);
            var result8 = service.Update(
                input8);
            var result9 = service.Update(
                input9);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null);
            Assert.True(result2.Status == TaskStatus.Faulted && result2.Exception.InnerException.Message == "There is no role with id: 999");
            Assert.True(result3.Status == TaskStatus.Faulted && result3.Exception.InnerException.Message == "Identity.InvalidEmail");
            Assert.True(result4.Status == TaskStatus.Faulted && result4.Exception.InnerException.Message == "Object reference not set to an instance of an object."); //Permissions List required
            Assert.True(result5.Status == TaskStatus.RanToCompletion && result5.Result != null);
            Assert.True(result6.Status == TaskStatus.RanToCompletion && result6.Result != null);
            Assert.True(result7.Status == TaskStatus.RanToCompletion && result7.Result != null);
            Assert.True(result8.Status == TaskStatus.RanToCompletion && result8.Result != null);
            Assert.True(result9.Status == TaskStatus.RanToCompletion && result9.Result != null);
        }

        [Fact]
        public void Delete_StateUnderTest_ExpectedBehavior()
        {
            LoginAsHost("85261107946");
            // Arrange
            var service = this.CreateService();
            EntityDto input1 = new EntityDto { Id = 3 };
            EntityDto input2 = new EntityDto { Id = 1 };
            //EntityDto input3 = null;

            // Act
            var result1 = service.Delete(
                input1);
            var result2 = service.Delete(
                input2);
            //var result3 = service.Delete(
            //    input3);


            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion);
            Assert.True(result2.Status == TaskStatus.Faulted); // cannot find records
            //Assert.True(result3.Status == TaskStatus.Faulted);
        }

        [Fact]
        public void GetAllPermissions_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.PermissionManager = Resolve<IPermissionManager>();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();

            // Act
            var result1 = service.GetAllPermissions();

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Items.Count > 0);
        }

        [Fact]
        public void GetRoleForEdit_StateUnderTest_ExpectedBehavior()
        {
            LoginAsHost("85261107946");
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.PermissionManager = Resolve<IPermissionManager>();

            EntityDto input1 = new EntityDto { Id = 1 };
            EntityDto input2 = new EntityDto { Id = 99 };
            EntityDto input3 = new EntityDto { Id = 99999 };

            // Act
            var result1 = service.GetRoleForEdit(
                input1);
            var result2 = service.GetRoleForEdit(
                input2);
            var result3 = service.GetRoleForEdit(
                input3);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result != null);
            Assert.True(result3.Status == TaskStatus.Faulted && result3.Exception.InnerException.Message == "There is no role with id: 99999");
        }
    }
}
