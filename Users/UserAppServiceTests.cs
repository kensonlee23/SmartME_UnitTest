using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.Users;
using com.trustmechain.SmartME.Core.Authorization;
using com.trustmechain.SmartME.Core.Authorization.Roles;
using com.trustmechain.SmartME.Core.Authorization.Users;
using com.trustmechain.SmartME.Application.Users.Dto;
using Abp.Domain.Uow;
using Abp.Application.Services.Dto;

namespace com.trustmechain.SmartME.Tests
{
    public class UserAppServiceTests : SmartMETestBase
    {
        private IRepository<User, long> subRepositoryUserLong;
        private UserManager subUserManager;
        private RoleManager subRoleManager;
        private IRepository<Role> subRepositoryRole;
        private IPasswordHasher<User> subPasswordHasher;
        private IAbpSession subAbpSession;
        private LogInManager subLogInManager;

        public UserAppServiceTests()
        {
            this.subRepositoryUserLong = Resolve<IRepository<User, long>>();
            this.subUserManager = Resolve<UserManager>();
            this.subRoleManager = Resolve<RoleManager>();
            this.subRepositoryRole = Resolve<IRepository<Role>>();
            this.subPasswordHasher = Resolve<IPasswordHasher<User>>();
            this.subAbpSession = Resolve<IAbpSession>();
            this.subLogInManager = Resolve<LogInManager>();
        }

        private UserAppService CreateService()
        {
            return new UserAppService(
                this.subRepositoryUserLong,
                this.subUserManager,
                this.subRoleManager,
                this.subRepositoryRole,
                this.subPasswordHasher,
                this.subAbpSession,
                this.subLogInManager);
        }

        [Fact]
        public void Create_StateUnderTest_ExpectedBehavior()
        {
            
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsDefaultTenantAdmin();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            // Arrange
            CreateUserDto input1 = new CreateUserDto { UserName = "85261234567", EmailAddress = "xanxuswong@trustmechain.com", Name = "testing", Password = "12345678", Surname = "1", IsActive = true, RoleNames = new string[] { "admin" } };
            CreateUserDto input2 = new CreateUserDto { UserName = "", EmailAddress = "", Name = "", Password = "", Surname = "", IsActive = true, RoleNames = new string[] { "" } };
            CreateUserDto input3 = new CreateUserDto { UserName = null, EmailAddress = null, Name = null, Password = null, Surname = null, IsActive = true, RoleNames = null };
            CreateUserDto input4 = new CreateUserDto { UserName = "85261234567", EmailAddress = null, Name = null, Password = null, Surname = null, IsActive = true, RoleNames = null };
            CreateUserDto input5 = new CreateUserDto { UserName = "85261234567", EmailAddress = "xanxuswong@trustmechain.com", Name = null, Password = null, Surname = null, IsActive = true, RoleNames = null };
            CreateUserDto input6 = new CreateUserDto { UserName = "85261234567", EmailAddress = "xanxuswong@trustmechain.com", Name = "testing", Password = null, Surname = null, IsActive = true, RoleNames = null };
            CreateUserDto input7 = new CreateUserDto { UserName = "85261234567", EmailAddress = "xanxuswong@trustmechain.com", Name = "testing", Password = "12345678", Surname = null, IsActive = true, RoleNames = null };

            // Act
            var result1 = service.Create(input1);
            var result2 = service.Create(input2);
            var result3 = service.Create(input3);
            var result4 = service.Create(input4);
            var result5 = service.Create(input5);
            var result6 = service.Create(input6);
            var result7 = service.Create(input7);


            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null);
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.Faulted);
            Assert.True(result5.Status == TaskStatus.Faulted);
            Assert.True(result6.Status == TaskStatus.Faulted);
            Assert.True(result7.Status == TaskStatus.Faulted);


        }

        [Fact]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsTenant("Default", "85261586394");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            // Arrange
            UpdateUserDto input1 = new UpdateUserDto
            {
                Id = 3,
                UserName = "85261586394",
                FirstName_ChT = "abc",
                FirstName_Eng = "abcd",
                FullName = "abcdef",
                HKID = "A1234567",
                Surname = "testing",
                Name = "testing",
                LangPreference = "zht",
                LastName_ChT = "abcd",
                LastName_Eng = "abcde",
                
            };


            // Act
            var result1 = service.Update(
                input1);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null);  //exception SetNormalizedNname null object
        }

        [Fact]
        public void Delete_StateUnderTest_ExpectedBehavior()
        {
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsTenant("Default", "85261586394");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();


            // Arrange
            EntityDto<long> input1 = new EntityDto<long> { Id = long.Parse("3") };
            EntityDto<long> input2 = new EntityDto<long> { Id = long.Parse("5") }; // record not exist
            //EntityDto<long> input3 = null;

            // Act
            var result1 = service.Delete(
                input1);
            var result2 = service.Delete(
                input2);
            //var result3 = _userAppService.Delete(
            //    input3);

            // Assert
            UsingDbContext(context =>
                Assert.True(result1.Status == TaskStatus.RanToCompletion && context.Users.Find(long.Parse("3")) == null)
            );
            Assert.True(result2.Status == TaskStatus.Faulted && result2.Exception.InnerException.Message == "There is no user with id: 5");

        }

        [Fact]
        public void GetRoles_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsTenant("Default", "85261586394");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();


            // Act
            var result = service.GetRoles();

            // Assert
            Assert.True(result.Status == TaskStatus.RanToCompletion && result.Result.Items.Count > 0);
        }

        [Fact]
        public void ChangeLanguage_StateUnderTest_ExpectedBehavior()
        {

            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsDefaultTenantAdmin();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            // Arrange
            ChangeUserLanguageDto input1 = new ChangeUserLanguageDto { LanguageName = "zht" };
            ChangeUserLanguageDto input2 = new ChangeUserLanguageDto { LanguageName = "" }; //has mandatory checking
            ChangeUserLanguageDto input3 = new ChangeUserLanguageDto { LanguageName = null };

            // Act
            var result1 = service.ChangeLanguage(
                input1);
            var result2 = service.ChangeLanguage(
                input2);
            var result3 = service.ChangeLanguage(
                input3);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion);
            Assert.True(result2.Status == TaskStatus.RanToCompletion);
            Assert.True(result3.Status == TaskStatus.RanToCompletion);
        }

        [Fact]
        public void ChangePassword_StateUnderTest_ExpectedBehavior()
        {
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            // Arrange
            ChangePasswordDto input1 = new ChangePasswordDto { CurrentPassword = "12345678", NewPassword = "Abcd1234" };
            ChangePasswordDto input2 = new ChangePasswordDto { CurrentPassword = "", NewPassword = "" };
            ChangePasswordDto input3 = new ChangePasswordDto { CurrentPassword = "", NewPassword = "Abcd1234" };
            ChangePasswordDto input4 = new ChangePasswordDto { CurrentPassword = null, NewPassword = null };
            ChangePasswordDto input5 = new ChangePasswordDto { CurrentPassword = "12345678", NewPassword = null };
            ChangePasswordDto input6 = new ChangePasswordDto { CurrentPassword = null, NewPassword = "Abcd1234" };

            // Act
            var result1 = service.ChangePassword(input1);
            var result2 = service.ChangePassword(input2);
            var result3 = service.ChangePassword(input3);
            var result4 = service.ChangePassword(input4);
            var result5 = service.ChangePassword(input5);
            var result6 = service.ChangePassword(input6);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result == true);
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.Faulted);
            Assert.True(result5.Status == TaskStatus.Faulted);
            Assert.True(result6.Status == TaskStatus.Faulted);
        }

        [Fact]
        public void ResetPassword_StateUnderTest_ExpectedBehavior()
        {
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();


            // Arrange
            ResetPasswordDto input1 = new ResetPasswordDto { UserId = 1, NewPassword = "Abcd1234", AdminPassword = "12345678" };
            ResetPasswordDto input2 = new ResetPasswordDto { UserId = 1, NewPassword = null, AdminPassword = null };
            ResetPasswordDto input3 = new ResetPasswordDto { UserId = 1, NewPassword = "", AdminPassword = "" };
            ResetPasswordDto input4 = new ResetPasswordDto { UserId = 1, NewPassword = "Abcd1234", AdminPassword = null };
            ResetPasswordDto input5 = new ResetPasswordDto { UserId = 1, NewPassword = null, AdminPassword = "12345678" };
            ResetPasswordDto input6 = new ResetPasswordDto { UserId = 1, NewPassword = "Abcd1234", AdminPassword = "" };
            ResetPasswordDto input7 = new ResetPasswordDto { UserId = 1, NewPassword = "", AdminPassword = "12345678" };
            ResetPasswordDto input8 = new ResetPasswordDto { UserId = 999, NewPassword = "Abcd1234", AdminPassword = "12345678" };


            //var pw = Resolve<UserManager>().PasswordHasher.HashPassword(new User(),"123qwe");

            //var pw2 = new Core.Authorization.Users.PasswordHasher<User>(new Microsoft.Extensions.Options.OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(new User(), "123qwe");

            //// Act
            var result1 = service.ResetPassword(
            input1);
            var result2 = service.ResetPassword(
            input2);
            var result3 = service.ResetPassword(
            input3);
            var result4 = service.ResetPassword(
            input4);
            var result5 = service.ResetPassword(
            input5);
            var result6 = service.ResetPassword(
            input6);
            var result7 = service.ResetPassword(
            input7);
            var result8 = service.ResetPassword(
            input8);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result == true);
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.Faulted);
            Assert.True(result5.Status == TaskStatus.Faulted);
            Assert.True(result6.Status == TaskStatus.Faulted);
            Assert.True(result7.Status == TaskStatus.Faulted);
            Assert.True(result8.Status == TaskStatus.Faulted);


        }
    }
}
