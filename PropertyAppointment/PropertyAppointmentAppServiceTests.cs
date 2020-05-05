using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.Notification;
using com.trustmechain.SmartME.Application.PropertyAppointment;
using com.trustmechain.SmartME.Core.Models.Agent;
using com.trustmechain.SmartME.Core.PropertyAppointment;
using com.trustmechain.SmartME.Application.PropertyAppointment.Dto;
using com.trustmechain.SmartME.Application.PropertyAppointmentSchedule.Dto;
using Abp.Runtime.Session;
using Abp.Domain.Uow;
using System.Collections.Generic;

namespace com.trustmechain.SmartME.Tests.PropertyAppointment
{
    public class PropertyAppointmentAppServiceTests : SmartMETestBase
    {
        private IRepository<Core.Models.PropertyAppointment> subRepositoryPropertyAppointment;
        private NotificationAppService subNotificationAppService;
        private IHttpContextAccessor subHttpContextAccessor;
        private IRepository<AgentProfile, long> subRepositoryAgentProfileLong;
        private IRepository<AgentCompanyBranch> subRepositoryAgentCompanyBranch;
        private IRepository<AgentCompany> subRepositoryAgentCompany;
        private IRepository<Core.Models.PropertyAppointmentSchedule> subRepositoryPropertyAppointmentSchedule;
        private ILogger<PropertyAppointmentAppService> subLogger;
        private IConfiguration subConfiguration;
        private IRepository<Core.Models.View.AgentFlatSimpleConsolidateView, long> subRepositoryAgentFlatSimpleConsolidateViewLong;
        private IPropertyAppointmentManager subPropertyAppointmentManager;

        public PropertyAppointmentAppServiceTests()
        {
            this.subRepositoryPropertyAppointment = Resolve<IRepository<Core.Models.PropertyAppointment>>();
            this.subNotificationAppService = Resolve<NotificationAppService>();
            this.subHttpContextAccessor = Resolve<IHttpContextAccessor>();
            this.subRepositoryAgentProfileLong = Resolve<IRepository<AgentProfile, long>>();
            this.subRepositoryAgentCompanyBranch = Resolve<IRepository<AgentCompanyBranch>>();
            this.subRepositoryAgentCompany = Resolve<IRepository<AgentCompany>>();
            this.subRepositoryPropertyAppointmentSchedule = Resolve<IRepository<Core.Models.PropertyAppointmentSchedule>>();
            this.subLogger = Resolve<ILogger<PropertyAppointmentAppService>>();
            this.subConfiguration = Resolve<IConfiguration>();
            this.subRepositoryAgentFlatSimpleConsolidateViewLong = Resolve<IRepository<Core.Models.View.AgentFlatSimpleConsolidateView, long>>();
            this.subPropertyAppointmentManager = Resolve<IPropertyAppointmentManager>();
        }

        private PropertyAppointmentAppService CreateService()
        {
            return new PropertyAppointmentAppService(
                this.subRepositoryPropertyAppointment,
                this.subNotificationAppService,
                this.subHttpContextAccessor,
                this.subRepositoryAgentProfileLong,
                this.subRepositoryAgentCompanyBranch,
                this.subRepositoryAgentCompany,
                this.subRepositoryPropertyAppointmentSchedule,
                this.subLogger,
                this.subConfiguration,
                this.subRepositoryAgentFlatSimpleConsolidateViewLong,
                this.subPropertyAppointmentManager);
        }

        [Fact]
        public async void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            //LoginAsTenant("Default","85261586394");
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            CreatePropertyAppointmentInput input1 = new CreatePropertyAppointmentInput
            {
                AgentProfileId = 6,
                Name = null,
                BuyerMobile = null,
                Category = new Core.Models.PropertyAppointment.PropertyAppointmentCategory { },
                Price = 0,
                PropertyAppointmentSchedule = null,
                PropertyId = 1,
                Remark = null,
                Surname = null,
                TelAreaCode = null,
                Title = null,

            };

            CreatePropertyAppointmentInput input2 = new CreatePropertyAppointmentInput
            {
                AgentProfileId = 6,
                Name = "name",
                BuyerMobile = null,
                Category = new Core.Models.PropertyAppointment.PropertyAppointmentCategory { },
                Price = 0,
                PropertyAppointmentSchedule = null,
                PropertyId = 1,
                Remark = null,
                Surname = null,
                TelAreaCode = null,
                Title = null,

            };

            CreatePropertyAppointmentInput input3 = new CreatePropertyAppointmentInput
            {
                AgentProfileId = 6,
                Name = "name",
                BuyerMobile = "98765432",
                Category = new Core.Models.PropertyAppointment.PropertyAppointmentCategory { },
                Price = 0,
                PropertyAppointmentSchedule = null,
                PropertyId = 1,
                Remark = null,
                Surname = null,
                TelAreaCode = null,
                Title = null,

            };

            CreatePropertyAppointmentInput input4 = new CreatePropertyAppointmentInput
            {
                AgentProfileId = 6,
                Name = "name",
                BuyerMobile = "98765432",
                Category = new Core.Models.PropertyAppointment.PropertyAppointmentCategory { },
                Price = 0,
                PropertyAppointmentSchedule = new List<CreatePropertyAppointmentScheduleInput>
                {
                   new CreatePropertyAppointmentScheduleInput
                   {
                       AppointmentDate = DateTime.Now,
                       EndTime = DateTime.Now,
                       StartTime = DateTime.Now,
                       Status = Core.Models.PropertyAppointmentSchedule.PropertyAppointmentScheduleStatus.Pending
                   }
                },
                PropertyId = 1,
                Remark = null,
                Surname = null,
                TelAreaCode = null,
                Title = null,

            };

            CreatePropertyAppointmentInput input5 = new CreatePropertyAppointmentInput
            {
                AgentProfileId = 6,
                Name = "name",
                BuyerMobile = "98765432",
                Category = new Core.Models.PropertyAppointment.PropertyAppointmentCategory { },
                Price = 0,
                PropertyAppointmentSchedule = new List<CreatePropertyAppointmentScheduleInput>
                {
                   new CreatePropertyAppointmentScheduleInput
                   {
                       AppointmentDate = DateTime.Now,
                       EndTime = DateTime.Now,
                       StartTime = DateTime.Now,
                       Status = Core.Models.PropertyAppointmentSchedule.PropertyAppointmentScheduleStatus.Pending
                   }
                },
                PropertyId = 1,
                Remark = "remark",
                Surname = null,
                TelAreaCode = null,
                Title = null,

            };

            CreatePropertyAppointmentInput input6 = new CreatePropertyAppointmentInput
            {
                AgentProfileId = 6,
                Name = "name",
                BuyerMobile = "98765432",
                Category = new Core.Models.PropertyAppointment.PropertyAppointmentCategory { },
                Price = 0,
                PropertyAppointmentSchedule = new List<CreatePropertyAppointmentScheduleInput>
                {
                   new CreatePropertyAppointmentScheduleInput
                   {
                       AppointmentDate = DateTime.Now,
                       EndTime = DateTime.Now,
                       StartTime = DateTime.Now,
                       Status = Core.Models.PropertyAppointmentSchedule.PropertyAppointmentScheduleStatus.Pending
                   }
                },
                PropertyId = 1,
                Remark = "remark",
                Surname = "Surnname",
                TelAreaCode = null,
                Title = null,

            };

            CreatePropertyAppointmentInput input7 = new CreatePropertyAppointmentInput
            {
                AgentProfileId = 6,
                Name = "name",
                BuyerMobile = "98765432",
                Category = new Core.Models.PropertyAppointment.PropertyAppointmentCategory { },
                Price = 0,
                PropertyAppointmentSchedule = new List<CreatePropertyAppointmentScheduleInput>
                {
                   new CreatePropertyAppointmentScheduleInput
                   {
                       AppointmentDate = DateTime.Now,
                       EndTime = DateTime.Now,
                       StartTime = DateTime.Now,
                       Status = Core.Models.PropertyAppointmentSchedule.PropertyAppointmentScheduleStatus.Pending
                   }
                },
                PropertyId = 1,
                Remark = "remark",
                Surname = "Surnname",
                TelAreaCode = "852",
                Title = null,

            };

            CreatePropertyAppointmentInput input8 = new CreatePropertyAppointmentInput
            {
                AgentProfileId = 6,
                Name = "name",
                BuyerMobile = "98765432",
                Category = new Core.Models.PropertyAppointment.PropertyAppointmentCategory(),
                Price = 5000,
                PropertyAppointmentSchedule = new List<CreatePropertyAppointmentScheduleInput>
                {
                   new CreatePropertyAppointmentScheduleInput
                   {
                       AppointmentDate = DateTime.Now,
                       EndTime = DateTime.Now,
                       StartTime = DateTime.Now,
                       Status = Core.Models.PropertyAppointmentSchedule.PropertyAppointmentScheduleStatus.Pending
                   }
                },
                PropertyId = 1,
                Remark = "remark",
                Surname = "surname",
                TelAreaCode = "852",
                Title = "title",

            };

            // Act
            var result1 = service.Create(
                input1);
            var result2 = service.Create(
                input2);
            var result3 = service.Create(
                input3);
            var result4 = await service.Create(
                input4);
            var result5 = await service.Create(
                input5);
            var result6 = await service.Create(
                input6);
            var result7 = await service.Create(
                input7);
            var result8 = await service.Create(
                input8);


            // Assert
            Assert.True(result1.Status == TaskStatus.Faulted);
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4 != null); //WaitingForActivation
            Assert.True(result5 != null);
            Assert.True(result6 != null);
            Assert.True(result7 != null);
            Assert.True(result8 != null);
        }

        [Fact]
        public async Task GetAllPropertyAppointmentListForSeller_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyAppointmentInput input = null;

            // Act
            var result = await service.GetAllPropertyAppointmentListForSeller(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetAllPropertyAppointmentListForAgent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyAppointmentInput input = null;

            // Act
            var result = await service.GetAllPropertyAppointmentListForAgent(
                input);

            // Assert
            Assert.True(false);
        }
    }
}
