using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.Authorization;
using com.trustmechain.SmartME.Application.Authorization.Accounts;
using com.trustmechain.SmartME.Application.Roles;
using com.trustmechain.SmartME.Application.SmartContract;
using com.trustmechain.SmartME.Application.SmartUserDocUpload;
using com.trustmechain.SmartME.Core.Agent;
using com.trustmechain.SmartME.Core.Authorization;
using com.trustmechain.SmartME.Core.Authorization.Users;
using com.trustmechain.SmartME.Core.ClientFavourite;
using com.trustmechain.SmartME.Core.Models.Agent;
using com.trustmechain.SmartME.Core.SMS;
using com.trustmechain.SmartME.Application.Authorization.Accounts.Dto;
using Abp.Domain.Uow;

namespace com.trustmechain.SmartME.Tests.Authorization.Accounts
{
    public class AccountAppServiceTests : SmartMETestBase
    {
        private IRepository<User, long> subRepositoryUserLong;
        private UserRegistrationManager subUserRegistrationManager;
        private IClientFavouriteManager subClientFavouriteManager;
        private ISMSManager subSMSManager;
        private ISmartContractAppService subSmartContractAppService;
        private IAbpSession subAbpSession;
        private ITenantCache subTenantCache;
        private IConfiguration subConfiguration;
        private AbpLoginResultTypeHelper subAbpLoginResultTypeHelper;
        private IRoleAppService subRoleAppService;
        private ILogger<AccountAppService> subLogger;
        private IRepository<AgentProfile, long> subRepositoryAgentProfileLong;
        private IRepository<AgentCompany> subRepositoryAgentCompany;
        private IRepository<AgentCompanyBranch> subRepositoryAgentCompanyBranch;
        private IRepository<UserRole, long> subRepositoryUserRoleLong;
        private IRepository<AgentPreferredCategory> subRepositoryAgentPreferredCategory;
        private IRepository<AgentPreferredSubDistrict> subRepositoryAgentPreferredSubDistrict;
        private UserManager subUserManager;
        private LogInManager subLogInManager;
        private ISmartUserDocUploadAppService subSmartUserDocUploadAppService;
        private IRepository<Core.Models.SmartAgreementLease> subRepositorySmartAgreementLease;
        private IRepository<Core.Models.SmartAgreementSell> subRepositorySmartAgreementSell;
        private IRepository<Core.Models.SmartContract> subRepositorySmartContract;
        private IRepository<Core.Models.SmartUserToken> subRepositorySmartUserToken;
        private AgentManager subAgentManager;

        public AccountAppServiceTests()
        {
            this.subRepositoryUserLong = Resolve<IRepository<User, long>>();
            this.subUserRegistrationManager = Resolve<UserRegistrationManager>();
            this.subClientFavouriteManager = Resolve<IClientFavouriteManager>();
            this.subSMSManager = Resolve<ISMSManager>();
            this.subSmartContractAppService = Resolve<ISmartContractAppService>();
            this.subAbpSession = Resolve<IAbpSession>();
            this.subTenantCache = Resolve<ITenantCache>();
            this.subConfiguration = Resolve<IConfiguration>();
            this.subAbpLoginResultTypeHelper = Resolve<AbpLoginResultTypeHelper>();
            this.subRoleAppService = Resolve<IRoleAppService>();
            this.subLogger = Resolve<ILogger<AccountAppService>>();
            this.subRepositoryAgentProfileLong = Resolve<IRepository<AgentProfile, long>>();
            this.subRepositoryAgentCompany = Resolve<IRepository<AgentCompany>>();
            this.subRepositoryAgentCompanyBranch = Resolve<IRepository<AgentCompanyBranch>>();
            this.subRepositoryUserRoleLong = Resolve<IRepository<UserRole, long>>();
            this.subRepositoryAgentPreferredCategory = Resolve<IRepository<AgentPreferredCategory>>();
            this.subRepositoryAgentPreferredSubDistrict = Resolve<IRepository<AgentPreferredSubDistrict>>();
            this.subUserManager = Resolve<UserManager>();
            this.subLogInManager = Resolve<LogInManager>();
            this.subSmartUserDocUploadAppService = Resolve<ISmartUserDocUploadAppService>();
            this.subRepositorySmartAgreementLease = Resolve<IRepository<Core.Models.SmartAgreementLease>>();
            this.subRepositorySmartAgreementSell = Resolve<IRepository<Core.Models.SmartAgreementSell>>();
            this.subRepositorySmartContract = Resolve<IRepository<Core.Models.SmartContract>>();
            this.subRepositorySmartUserToken = Resolve<IRepository<Core.Models.SmartUserToken>>();
            this.subAgentManager = Resolve<AgentManager>();
        }

        private AccountAppService CreateService()
        {
            return new AccountAppService(
                this.subRepositoryUserLong,
                this.subUserRegistrationManager,
                this.subClientFavouriteManager,
                this.subSMSManager,
                this.subSmartContractAppService,
                this.subAbpSession,
                this.subTenantCache,
                this.subConfiguration,
                this.subAbpLoginResultTypeHelper,
                this.subRoleAppService,
                this.subLogger,
                this.subRepositoryAgentProfileLong,
                this.subRepositoryAgentCompany,
                this.subRepositoryAgentCompanyBranch,
                this.subRepositoryUserRoleLong,
                this.subRepositoryAgentPreferredCategory,
                this.subRepositoryAgentPreferredSubDistrict,
                this.subUserManager,
                this.subLogInManager,
                this.subSmartUserDocUploadAppService,
                this.subRepositorySmartAgreementLease,
                this.subRepositorySmartAgreementSell,
                this.subRepositorySmartContract,
                this.subRepositorySmartUserToken,
                this.subAgentManager);
        }

        [Fact]
        public void CompleteRegister_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();

            CompleteRegisterInput input1 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = "852", PhoneNumber = "61107946", RegistrationId = "123", OTP = "1111" };
            CompleteRegisterInput input2 = new CompleteRegisterInput() { Name = null, Password = null, Surname = null, TelAreaCode = null, PhoneNumber = null, RegistrationId = null, OTP = null };
            CompleteRegisterInput input3 = new CompleteRegisterInput() { Name = "testing", Password = null, Surname = null, TelAreaCode = null, PhoneNumber = null, RegistrationId = null, OTP = null };
            CompleteRegisterInput input4 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = null, TelAreaCode = null, PhoneNumber = null, RegistrationId = null, OTP = null };
            CompleteRegisterInput input5 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = null, PhoneNumber = null, RegistrationId = null, OTP = null };
            CompleteRegisterInput input6 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = "852", PhoneNumber = null, RegistrationId = null, OTP = null };
            CompleteRegisterInput input7 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = "852", PhoneNumber = "61107946", RegistrationId = null, OTP = null };
            CompleteRegisterInput input8 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = "852", PhoneNumber = "61107946", RegistrationId = "123", OTP = null };
            CompleteRegisterInput input9 = new CompleteRegisterInput() { Name = "", Password = "", Surname = "", TelAreaCode = "", PhoneNumber = "", RegistrationId = "", OTP = "" };
            CompleteRegisterInput input10 = new CompleteRegisterInput() { Name = "testing", Password = "", Surname = "", TelAreaCode = "", PhoneNumber = "", RegistrationId = "", OTP = "" };
            CompleteRegisterInput input11 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "", TelAreaCode = "", PhoneNumber = "", RegistrationId = "", OTP = "" };
            CompleteRegisterInput input12 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = "", PhoneNumber = "", RegistrationId = "", OTP = "" };
            CompleteRegisterInput input13 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = "852", PhoneNumber = "", RegistrationId = "", OTP = "" };
            CompleteRegisterInput input14 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = "852", PhoneNumber = "61107946", RegistrationId = "", OTP = "" };
            CompleteRegisterInput input15 = new CompleteRegisterInput() { Name = "testing", Password = "12345678", Surname = "agent1", TelAreaCode = "852", PhoneNumber = "61107946", RegistrationId = "123", OTP = "" };

            // Act
            var result1 = unitUnderTest.CompleteRegister(
                input1);
            var result2 = unitUnderTest.CompleteRegister(
                input2);
            var result3 = unitUnderTest.CompleteRegister(
                input3);
            var result4 = unitUnderTest.CompleteRegister(
                input4);
            var result5 = unitUnderTest.CompleteRegister(
                input5);
            var result6 = unitUnderTest.CompleteRegister(
                input6);
            var result7 = unitUnderTest.CompleteRegister(
                input7);
            var result8 = unitUnderTest.CompleteRegister(
                input8);
            var result9 = unitUnderTest.CompleteRegister(
                input9);
            var result10 = unitUnderTest.CompleteRegister(
                input10);
            var result11 = unitUnderTest.CompleteRegister(
                input11);
            var result12 = unitUnderTest.CompleteRegister(
                input12);
            var result13 = unitUnderTest.CompleteRegister(
                input13);
            var result14 = unitUnderTest.CompleteRegister(
                input14);
            var result15 = unitUnderTest.CompleteRegister(
                input15);


            // Assert
            //Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.UserDto != null);
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.Faulted);
            Assert.True(result5.Status == TaskStatus.Faulted);
            Assert.True(result6.Status == TaskStatus.RanToCompletion && result6.Result.Message == "OTP validation fail");
            Assert.True(result7.Status == TaskStatus.RanToCompletion && result7.Result.Message == "OTP validation fail");
            Assert.True(result8.Status == TaskStatus.RanToCompletion && result8.Result.Message == "OTP validation fail");
            Assert.True(result9.Status == TaskStatus.RanToCompletion && result9.Result.Message == "OTP validation fail");
            Assert.True(result10.Status == TaskStatus.RanToCompletion && result10.Result.Message == "OTP validation fail");
            Assert.True(result11.Status == TaskStatus.RanToCompletion && result11.Result.Message == "OTP validation fail");
            Assert.True(result12.Status == TaskStatus.RanToCompletion && result12.Result.Message == "OTP validation fail");
            Assert.True(result13.Status == TaskStatus.RanToCompletion && result13.Result.Message == "OTP validation fail");
            Assert.True(result14.Status == TaskStatus.RanToCompletion && result14.Result.Message == "OTP validation fail");
            Assert.True(result15.Status == TaskStatus.RanToCompletion && result15.Result.Message == "OTP validation fail");


        }

        [Fact]
        public void PreRegister_StateUnderTest_ExpectedBehavior()
        {
            LoginAsTenant("Default", "85261586394");
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            //LoginAsHost("85261107946");
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();
            PreRegisterInput input1 = new PreRegisterInput() { TelAreaCode = "852", PhoneNumber = "61586394" };
            PreRegisterInput input2 = new PreRegisterInput() { TelAreaCode = "852", PhoneNumber = "99999999" };
            PreRegisterInput input3 = new PreRegisterInput() { TelAreaCode = null, PhoneNumber = null };
            PreRegisterInput input4 = new PreRegisterInput() { TelAreaCode = "852", PhoneNumber = null };
            PreRegisterInput input5 = new PreRegisterInput() { TelAreaCode = null, PhoneNumber = "6158394" };
            PreRegisterInput input6 = new PreRegisterInput() { TelAreaCode = "", PhoneNumber = "" };
            PreRegisterInput input7 = new PreRegisterInput() { TelAreaCode = "852", PhoneNumber = "" };
            PreRegisterInput input8 = new PreRegisterInput() { TelAreaCode = "", PhoneNumber = "61586394" };

            // Act
            var result1 = unitUnderTest.PreRegister(
                input1);
            var result2 = unitUnderTest.PreRegister(
                input2);
            var result3 = unitUnderTest.PreRegister(
                input3);
            var result4 = unitUnderTest.PreRegister(
                input4);
            var result5 = unitUnderTest.PreRegister(
                input5);
            var result6 = unitUnderTest.PreRegister(
                input6);
            var result7 = unitUnderTest.PreRegister(
                input7);
            var result8 = unitUnderTest.PreRegister(
                input8);


            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.IsMobileExist == true);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result.IsMobileExist == false);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.Faulted);
            Assert.True(result5.Status == TaskStatus.Faulted);
            Assert.True(result6.Status == TaskStatus.RanToCompletion && result6.Result.IsMobileExist == false);
            Assert.True(result7.Status == TaskStatus.RanToCompletion && result7.Result.IsMobileExist == false);
            Assert.True(result8.Status == TaskStatus.RanToCompletion && result8.Result.IsMobileExist == false);

        }

        [Fact]
        public void GenerateLoginOTP_StateUnderTest_ExpectedBehavior()
        {
            LoginAsTenant("Default", "85261586394");
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();

            GenerateLoginOTPInput input1 = new GenerateLoginOTPInput() { PhoneNumber = "61586394", TelAreaCode = "852" };
            GenerateLoginOTPInput input2 = new GenerateLoginOTPInput() { PhoneNumber = "99999999", TelAreaCode = "852" };
            GenerateLoginOTPInput input3 = new GenerateLoginOTPInput() { PhoneNumber = "", TelAreaCode = "" };
            GenerateLoginOTPInput input4 = new GenerateLoginOTPInput() { PhoneNumber = null, TelAreaCode = null };
            GenerateLoginOTPInput input5 = new GenerateLoginOTPInput() { PhoneNumber = null, TelAreaCode = "" };
            GenerateLoginOTPInput input6 = new GenerateLoginOTPInput() { PhoneNumber = "", TelAreaCode = null };
            GenerateLoginOTPInput input7 = new GenerateLoginOTPInput() { PhoneNumber = "61586394", TelAreaCode = null };
            GenerateLoginOTPInput input8 = new GenerateLoginOTPInput() { PhoneNumber = null, TelAreaCode = "852" };
            GenerateLoginOTPInput input9 = new GenerateLoginOTPInput() { PhoneNumber = "61586394", TelAreaCode = "" };
            GenerateLoginOTPInput input10 = new GenerateLoginOTPInput() { PhoneNumber = "", TelAreaCode = "852" };

            // Act
            var result1 = unitUnderTest.GenerateLoginOTP(
                input1);
            var result2 = unitUnderTest.GenerateLoginOTP(
                input2);
            var result3 = unitUnderTest.GenerateLoginOTP(
                input3);
            var result4 = unitUnderTest.GenerateLoginOTP(
                input4);
            var result5 = unitUnderTest.GenerateLoginOTP(
                input5);
            var result6 = unitUnderTest.GenerateLoginOTP(
                input6);
            var result7 = unitUnderTest.GenerateLoginOTP(
                input7);
            var result8 = unitUnderTest.GenerateLoginOTP(
                input8);
            var result9 = unitUnderTest.GenerateLoginOTP(
                input9);
            var result10 = unitUnderTest.GenerateLoginOTP(
                input10);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.IsMobileExist == true);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result.IsMobileExist == false);
            Assert.True(result3.Status == TaskStatus.RanToCompletion && result3.Result.IsMobileExist == false);
            Assert.True(result4.Status == TaskStatus.RanToCompletion && result4.Result.IsMobileExist == false);
            Assert.True(result5.Status == TaskStatus.RanToCompletion && result5.Result.IsMobileExist == false);
            Assert.True(result6.Status == TaskStatus.RanToCompletion && result6.Result.IsMobileExist == false);
            Assert.True(result7.Status == TaskStatus.RanToCompletion && result7.Result.IsMobileExist == false);
            Assert.True(result8.Status == TaskStatus.RanToCompletion && result8.Result.IsMobileExist == false);
            Assert.True(result9.Status == TaskStatus.RanToCompletion && result9.Result.IsMobileExist == false);
            Assert.True(result10.Status == TaskStatus.RanToCompletion && result10.Result.IsMobileExist == false);
        }

        [Fact]
        public void UserLogin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            LoginAsHost("85261107946");
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();

            UserLoginInput input1 = new UserLoginInput() { OTP = "1111", TelAreaCode = "852", PhoneNumber = "61107946", Password = "12345678" };
            UserLoginInput input2 = new UserLoginInput() { OTP = null, TelAreaCode = null, PhoneNumber = null, Password = null };
            UserLoginInput input3 = new UserLoginInput() { OTP = "1111", TelAreaCode = null, PhoneNumber = null, Password = null };
            UserLoginInput input4 = new UserLoginInput() { OTP = "1111", TelAreaCode = "852", PhoneNumber = null, Password = null };
            UserLoginInput input5 = new UserLoginInput() { OTP = "1111", TelAreaCode = "852", PhoneNumber = "61107946", Password = null };
            UserLoginInput input6 = new UserLoginInput() { OTP = "", TelAreaCode = "", PhoneNumber = "", Password = "" };
            UserLoginInput input7 = new UserLoginInput() { OTP = "1111", TelAreaCode = "", PhoneNumber = "", Password = "" };
            UserLoginInput input8 = new UserLoginInput() { OTP = "1111", TelAreaCode = "852", PhoneNumber = "", Password = "" };
            UserLoginInput input9 = new UserLoginInput() { OTP = "1111", TelAreaCode = "852", PhoneNumber = "61107946", Password = "" };
            UserLoginInput input10 = new UserLoginInput() { OTP = null, TelAreaCode = "852", PhoneNumber = "61107946", Password = "12345678" };

            // Act
            var result1 = unitUnderTest.UserLogin(
                input1);
            var result2 = unitUnderTest.UserLogin(
                input2);
            var result3 = unitUnderTest.UserLogin(
                input3);
            var result4 = unitUnderTest.UserLogin(
                input4);
            var result5 = unitUnderTest.UserLogin(
                input5);
            var result6 = unitUnderTest.UserLogin(
                input6);
            var result7 = unitUnderTest.UserLogin(
                input7);
            var result8 = unitUnderTest.UserLogin(
                input8);
            var result9 = unitUnderTest.UserLogin(
                input9);
            var result10 = unitUnderTest.UserLogin(
                input10);

            // Assert
            //Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.UserDto != null);
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.RanToCompletion && result4.Result.Message == "OTP validation fail");
            Assert.True(result5.Status == TaskStatus.RanToCompletion && result5.Result.Message == "OTP validation fail");
            Assert.True(result6.Status == TaskStatus.Faulted);
            Assert.True(result7.Status == TaskStatus.RanToCompletion && result7.Result.Message == "OTP validation fail");
            Assert.True(result8.Status == TaskStatus.RanToCompletion && result8.Result.Message == "OTP validation fail");
            Assert.True(result9.Status == TaskStatus.RanToCompletion && result9.Result.Message == "OTP validation fail");
            Assert.True(result10.Status == TaskStatus.RanToCompletion && result10.Result.UserDto != null);
        }

        [Fact]
        public async Task UserLoginAPortal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UserLoginAPortalInput input = null;

            // Act
            var result = await service.UserLoginAPortal(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void RegisterForAPortal_StateUnderTest_ExpectedBehavior()
        {
            //LoginAsTenant("Default", "85261586394");
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();

            RegisterForAPortalInput input1 = new RegisterForAPortalInput()
            {
                agent_license = null,
                area_code = null,
                br_no = null,
                business_card = null,
                business_no = null,
                chinese_name = null,
                company_address = null,
                company_fax = null,
                company_fax_area_code = null,
                company_name = null,
                company_tel = null,
                company_tel_area_code = null,
                email = null,
                fax = null,
                fax_area_code = null,
                first_name = null,
                last_name = null,
                license_expiry_date = DateTime.Now,
                license_no = null,
                Password = null,
                RegistrationId = null,
                role = null,
                tel = null,
                username = null,
                user_id = 2

            };

            RegisterForAPortalInput input2 = new RegisterForAPortalInput()
            {
                agent_license = null,
                area_code = "852",
                br_no = null,
                business_card = null,
                business_no = null,
                chinese_name = null,
                company_address = null,
                company_fax = null,
                company_fax_area_code = null,
                company_name = null,
                company_tel = null,
                company_tel_area_code = null,
                email = null,
                fax = null,
                fax_area_code = null,
                first_name = null,
                last_name = null,
                license_expiry_date = DateTime.Now,
                license_no = null,
                Password = null,
                RegistrationId = null,
                role = null,
                tel = null,
                username = null,
                user_id = 2

            };

            RegisterForAPortalInput input3 = new RegisterForAPortalInput()
            {
                agent_license = null,
                area_code = "852",
                br_no = null,
                business_card = null,
                business_no = null,
                chinese_name = null,
                company_address = null,
                company_fax = null,
                company_fax_area_code = null,
                company_name = null,
                company_tel = null,
                company_tel_area_code = null,
                email = "testing@email.com",
                fax = null,
                fax_area_code = null,
                first_name = null,
                last_name = null,
                license_expiry_date = DateTime.Now,
                license_no = null,
                Password = null,
                RegistrationId = null,
                role = null,
                tel = null,
                username = null,
                user_id = 2

            };

            RegisterForAPortalInput input4 = new RegisterForAPortalInput()
            {
                agent_license = null,
                area_code = "852",
                br_no = null,
                business_card = null,
                business_no = null,
                chinese_name = null,
                company_address = null,
                company_fax = null,
                company_fax_area_code = null,
                company_name = null,
                company_tel = null,
                company_tel_area_code = null,
                email = "testing@email.com",
                fax = null,
                fax_area_code = null,
                first_name = null,
                last_name = null,
                license_expiry_date = DateTime.Now,
                license_no = null,
                Password = "12345678",
                RegistrationId = null,
                role = null,
                tel = "98765432",
                username = null,
                user_id = 2

            };

            RegisterForAPortalInput input5 = new RegisterForAPortalInput()
            {
                agent_license = null,
                area_code = "852",
                br_no = null,
                business_card = null,
                business_no = null,
                chinese_name = null,
                company_address = null,
                company_fax = null,
                company_fax_area_code = null,
                company_name = null,
                company_tel = null,
                company_tel_area_code = null,
                email = "testing@email.com",
                fax = null,
                fax_area_code = null,
                first_name = null,
                last_name = null,
                license_expiry_date = DateTime.Now,
                license_no = null,
                Password = "12345678",
                RegistrationId = null,
                role = null,
                tel = "98765432",
                username = null,
                user_id = 5

            };

            RegisterForAPortalInput input6 = new RegisterForAPortalInput()
            {
                agent_license = "agent_license",
                area_code = "852",
                br_no = "1234",
                business_card = "businesscard",
                business_no = "b123456",
                chinese_name = "chinesename",
                company_address = "company address",
                company_fax = "company fax",
                company_fax_area_code = "456",
                company_name = "company 1",
                company_tel = "65432100",
                company_tel_area_code = "852",
                email = "testing@email.com",
                fax = "123456789",
                fax_area_code = "852",
                first_name = "Tai",
                last_name = "Chan",
                license_expiry_date = DateTime.Now,
                license_no = "l1234",
                Password = "12345678",
                RegistrationId = "r1234",
                role = "role1",
                tel = "98765432",
                username = "85298765432",
                user_id = 5

            };


            // Act
            var result1 = unitUnderTest.RegisterForAPortal(
                input1);
            var result2 = unitUnderTest.RegisterForAPortal(
                input2);
            var result3 = unitUnderTest.RegisterForAPortal(
                input3);
            var result4 = unitUnderTest.RegisterForAPortal(
                input4);
            //var result5 = unitUnderTest.RegisterForAPortal(
            //    input5);
            var result6 = unitUnderTest.RegisterForAPortal(
                input6);

            // Assert
            Assert.True(result1.Status == TaskStatus.Faulted);
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.RanToCompletion);
            //Assert.True(result5.Status == TaskStatus.RanToCompletion);
            Assert.True(result6.Status == TaskStatus.RanToCompletion);

        }

        [Fact]
        public async Task CreateAgentData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            RegisterForAgentInput input = null;

            // Act
            var result = await service.CreateAgentData(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Logout_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            LogoutInput input = null;

            // Act
            await service.Logout(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdatePassword_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdatePasswordInput input = null;

            // Act
            var result = await service.UpdatePassword(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdatePasswordWithToken_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdatePasswordTokenInput input = null;

            // Act
            var result = await service.UpdatePasswordWithToken(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task VerifyOTPForResetPW_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            VerifyOTPInput input = null;

            // Act
            var result = await service.VerifyOTPForResetPW(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task SendOTPForResetPW_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GenerateLoginOTPInput input = null;

            // Act
            var result = await service.SendOTPForResetPW(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task VerifyOTPForRegAPortal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            VerifyOTPInput input = null;

            // Act
            var result = await service.VerifyOTPForRegAPortal(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task SendOTPForRegAPortal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GenerateLoginOTPInput input = null;

            // Act
            var result = await service.SendOTPForRegAPortal(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task ValidateRefreshToken_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UserTokenInput input = null;

            // Act
            var result = await service.ValidateRefreshToken(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task FirebaseTokenUpdate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdateFirebaseTokenInput input = null;

            // Act
            var result = await service.FirebaseTokenUpdate(
                input);

            // Assert
            Assert.True(false);
        }
    }
}
