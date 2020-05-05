using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.Agents;
using com.trustmechain.SmartME.Application.Authorization.Accounts;
using com.trustmechain.SmartME.Application.SmartUserDocUpload;
using com.trustmechain.SmartME.Core.Agent;
using com.trustmechain.SmartME.Core.Authorization.Users;
using com.trustmechain.SmartME.Core.Models.Agent;
using Abp.Domain.Uow;
using com.trustmechain.SmartME.Application.Agents.Dto;
using System.Collections.Generic;
using com.trustmechain.SmartME.Application.AdBanner.Dto;

namespace com.trustmechain.SmartME.Tests.Agents
{
    public class AgentAppServiceTests : SmartMETestBase
    {
        private IRepository<AgentProfile, long> subRepositoryAgentProfileLong;
        private IRepository<AgentCompany> subRepositoryAgentCompany;
        private IHttpContextAccessor subHttpContextAccessor;
        private IConfiguration subConfiguration;
        private IRepository<AgentCompanyBranch> subRepositoryAgentCompanyBranch;
        private IRepository<Core.Models.Agent.AgentPreferredSubDistrict> subRepositoryAgentPreferredSubDistrict;
        private IRepository<Core.Models.Agent.AgentPreferredCategory> subRepositoryAgentPreferredCategory;
        private ILogger<AgentAppService> subLogger;
        private UserManager subUserManager;
        private IAbpSession subAbpSession;
        private ISmartUserDocUploadAppService subSmartUserDocUploadAppService;
        private IAccountAppService subAccountAppService;
        private IRepository<Core.Models.View.AgentFlatSimpleConsolidateView, long> subRepositoryAgentFlatSimpleConsolidateViewLong;
        private AgentManager subAgentManager;

        public AgentAppServiceTests()
        {
            this.subRepositoryAgentProfileLong = Resolve<IRepository<AgentProfile, long>>();
            this.subRepositoryAgentCompany = Resolve<IRepository<AgentCompany>>();
            this.subHttpContextAccessor = Resolve<IHttpContextAccessor>();
            this.subConfiguration = Resolve<IConfiguration>();
            this.subRepositoryAgentCompanyBranch = Resolve<IRepository<AgentCompanyBranch>>();
            this.subRepositoryAgentPreferredSubDistrict = Resolve<IRepository<Core.Models.Agent.AgentPreferredSubDistrict>>();
            this.subRepositoryAgentPreferredCategory = Resolve<IRepository<Core.Models.Agent.AgentPreferredCategory>>();
            this.subLogger = Resolve<ILogger<AgentAppService>>();
            this.subUserManager = Resolve<UserManager>();
            this.subAbpSession = Resolve<IAbpSession>();
            this.subSmartUserDocUploadAppService = Resolve<ISmartUserDocUploadAppService>();
            this.subAccountAppService = Resolve<IAccountAppService>();
            this.subRepositoryAgentFlatSimpleConsolidateViewLong = Resolve<IRepository<Core.Models.View.AgentFlatSimpleConsolidateView, long>>();
            this.subAgentManager = Resolve<AgentManager>();
        }

        private AgentAppService CreateService()
        {
            return new AgentAppService(
                this.subRepositoryAgentProfileLong,
                this.subRepositoryAgentCompany,
                this.subHttpContextAccessor,
                this.subConfiguration,
                this.subRepositoryAgentCompanyBranch,
                this.subRepositoryAgentPreferredSubDistrict,
                this.subRepositoryAgentPreferredCategory,
                this.subLogger,
                this.subUserManager,
                this.subAbpSession,
                this.subSmartUserDocUploadAppService,
                this.subAccountAppService,
                this.subRepositoryAgentFlatSimpleConsolidateViewLong,
                this.subAgentManager);
        }

        //[Fact]
        //public async Task GetAllDistrictAgent_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var service = this.CreateService();
        //    GetAllDistrictAgentInput input = null;

        //    // Act
        //    var result = await service.GetAllDistrictAgent(
        //        input);

        //    // Assert
        //    Assert.True(false);
        //}

        //[Fact]
        //public async Task GetAllForAgentPreference_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var service = this.CreateService();
        //    GetAgentListInput input = null;

        //    // Act
        //    var result = await service.GetAllForAgentPreference(
        //        input);

        //    // Assert
        //    Assert.True(false);
        //}

        [Fact]
        public void CreateAgentPreference_StateUnderTest_ExpectedBehavior()
        {


            // Arrange
            var service = this.CreateService();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();



            CreateAgentPreferenceInput input1 = new CreateAgentPreferenceInput
            {
                AgentPreferredCategories = new List<AgentPreferredCategoryDto>
                {
                    new AgentPreferredCategoryDto
                    {
                        PreferredCategory = null,
                        //AgentProfileId = null
                    }

                },
                AgentPreferredSubDistricts = new List<AgentPreferredSubDistrictDto>
                {
                    new AgentPreferredSubDistrictDto
                    {
                        //AgentProfileId = null,
                        PreferredSubDistrict = null
                    }
                }
            };

            CreateAgentPreferenceInput input2 = new CreateAgentPreferenceInput
            {
                AgentPreferredCategories = new List<AgentPreferredCategoryDto>
                {
                    new AgentPreferredCategoryDto
                    {
                        PreferredCategory = "",
                        //AgentProfileId = null
                    }

                },
                AgentPreferredSubDistricts = new List<AgentPreferredSubDistrictDto>
                {
                    new AgentPreferredSubDistrictDto
                    {
                        PreferredSubDistrict = "",
                        //AgentProfileId = null
                    }
                }
            };

            CreateAgentPreferenceInput input3 = new CreateAgentPreferenceInput
            {
                AgentPreferredCategories = new List<AgentPreferredCategoryDto>
                {
                    new AgentPreferredCategoryDto
                    {
                        PreferredCategory = "",
                        //AgentProfileId = 1
                    }

                },
                AgentPreferredSubDistricts = new List<AgentPreferredSubDistrictDto>
                {
                    new AgentPreferredSubDistrictDto
                    {
                        PreferredSubDistrict = "",
                        //AgentProfileId = 1
                    }
                }
            };

            CreateAgentPreferenceInput input4 = new CreateAgentPreferenceInput
            {
                AgentPreferredCategories = new List<AgentPreferredCategoryDto>
                {
                    new AgentPreferredCategoryDto
                    {
                        PreferredCategory = "PreferredCategory",
                        //AgentProfileId = 1
                    }

                },
                AgentPreferredSubDistricts = new List<AgentPreferredSubDistrictDto>
                {
                    new AgentPreferredSubDistrictDto
                    {
                        PreferredSubDistrict = null,
                        //AgentProfileId = 1
                    }
                }
            };

            CreateAgentPreferenceInput input5 = new CreateAgentPreferenceInput
            {
                AgentPreferredCategories = new List<AgentPreferredCategoryDto>
                {
                    new AgentPreferredCategoryDto
                    {
                        PreferredCategory = null,
                        //AgentProfileId = 1
                    }

                },
                AgentPreferredSubDistricts = new List<AgentPreferredSubDistrictDto>
                {
                    new AgentPreferredSubDistrictDto
                    {
                        PreferredSubDistrict = "PreferredSubDistrict",
                        //AgentProfileId = 1
                    }
                }
            };

            CreateAgentPreferenceInput input6 = new CreateAgentPreferenceInput
            {
                AgentPreferredCategories = new List<AgentPreferredCategoryDto>
                {
                    new AgentPreferredCategoryDto
                    {
                        PreferredCategory = "PreferredCategory",
                        //AgentProfileId = 1
                    }

                },
                AgentPreferredSubDistricts = new List<AgentPreferredSubDistrictDto>
                {
                    new AgentPreferredSubDistrictDto
                    {
                        PreferredSubDistrict = "PreferredSubDistrict",
                        //AgentProfileId = 1
                    }
                }
            };

            // Act
            var result1 = service.CreateAgentPreference(
                input1);
            var result2 = service.CreateAgentPreference(
                input2);
            var result3 = service.CreateAgentPreference(
                input3);
            var result4 = service.CreateAgentPreference(
                input4);
            var result5 = service.CreateAgentPreference(
                input5);
            var result6 = service.CreateAgentPreference(
                input6);



            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.AgentPreferredCategories[0].PreferredCategory == null && result1.Result.AgentPreferredSubDistricts[0].PreferredSubDistrict == null);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result.AgentPreferredCategories[0].PreferredCategory == "" && result2.Result.AgentPreferredSubDistricts[0].PreferredSubDistrict == "");
            Assert.True(result3.Status == TaskStatus.RanToCompletion && result3.Result.AgentPreferredCategories[0].AgentProfileId == 6 && result3.Result.AgentPreferredSubDistricts[0].AgentProfileId == 6);
            Assert.True(result4.Status == TaskStatus.RanToCompletion && !string.IsNullOrEmpty(result4.Result.AgentPreferredCategories[0].PreferredCategory) && result4.Result.AgentPreferredSubDistricts[0].PreferredSubDistrict == null);
            Assert.True(result5.Status == TaskStatus.RanToCompletion && result5.Result.AgentPreferredCategories[0].PreferredCategory == null && !string.IsNullOrEmpty(result5.Result.AgentPreferredSubDistricts[0].PreferredSubDistrict));
            Assert.True(result6.Status == TaskStatus.RanToCompletion && !string.IsNullOrEmpty(result6.Result.AgentPreferredCategories[0].PreferredCategory) && !string.IsNullOrEmpty(result6.Result.AgentPreferredSubDistricts[0].PreferredSubDistrict));


        }

        [Fact]
        public void GetAgentPreferenceList_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();


            // Act
            var result1 = service.GetAgentPreferenceList();


            // Assert
            Assert.True(
                result1.Status == TaskStatus.RanToCompletion
                && result1.Result.AgentPreferredCategories.Count > 0
                && result1.Result.AgentPreferredSubDistricts.Count > 0
                );

        }

        [Fact]
        public async Task GetAgentData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = await service.GetAgentData();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdateAgentData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdateAgentDataInput input = null;

            // Act
            var result = await service.UpdateAgentData(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task VerifyAgent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            VerifyAgentInput input = null;

            // Act
            var result = await service.VerifyAgent(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async void Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdateAgentInput input = null;

            // Act
            var result = await service.Update(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetTotalAgentCount_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = await service.GetTotalAgentCount();

            // Assert
            Assert.True(false);
        }
    }
}
