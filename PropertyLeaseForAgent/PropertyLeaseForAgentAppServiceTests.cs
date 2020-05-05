using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.MetaEnum;
using com.trustmechain.SmartME.Application.PropertyForAgent;
using com.trustmechain.SmartME.Core.Authorization.Users;
using com.trustmechain.SmartME.Core.Azure;
using com.trustmechain.SmartME.Core.MetaEnum;
using com.trustmechain.SmartME.Core.Models;
using com.trustmechain.SmartME.Core.Models.Agent;
using com.trustmechain.SmartME.Core.Notification;
using com.trustmechain.SmartME.Application.PropertyLeaseForAgent.Dto;
using Abp.Domain.Uow;
using Abp.Runtime.Session;

namespace com.trustmechain.SmartME.Tests.PropertyLeaseForAgent
{
    public class PropertyLeaseForAgentAppServiceTests : SmartMETestBase
    {
        private IRepository<Core.Models.PropertyLeaseSelectedAgent> subRepositoryPropertyLeaseSelectedAgent;
        private IRepository<PropertyLeaseInfo> subRepositoryPropertyLeaseInfo;
        private INotificationManager subNotificationManager;
        private IRepository<Core.Models.PropertyImage> subRepositoryPropertyImage;
        private IRepository<PropertyMeta> subRepositoryPropertyMeta;
        private IRepository<Core.Models.Property> subRepositoryProperty;
        private IRepository<User, long> subRepositoryUserLong;
        private IRepository<Core.Models.PropertyAppointment> subRepositoryPropertyAppointment;
        private IRepository<Core.Models.PropertySellerAvailableSlot> subRepositoryPropertySellerAvailableSlot;
        private IHostingEnvironment subHostingEnvironment;
        private IHttpContextAccessor subHttpContextAccessor;
        private IConfiguration subConfiguration;
        private ILogger<PropertyLeaseForAgentAppService> subLogger;
        private MetaEnumAppService subMetaEnumAppService;
        private IAzureBlobManager subAzureBlobManager;
        private IRepository<AgentPreferredSubDistrict> subRepositoryAgentPreferredSubDistrict;
        private IMetaEnumManager subMetaEnumManager;

        public PropertyLeaseForAgentAppServiceTests()
        {
            this.subRepositoryPropertyLeaseSelectedAgent = Resolve<IRepository<Core.Models.PropertyLeaseSelectedAgent>>();
            this.subRepositoryPropertyLeaseInfo = Resolve<IRepository<PropertyLeaseInfo>>();
            this.subNotificationManager = Resolve<INotificationManager>();
            this.subRepositoryPropertyImage = Resolve<IRepository<Core.Models.PropertyImage>>();
            this.subRepositoryPropertyMeta = Resolve<IRepository<PropertyMeta>>();
            this.subRepositoryProperty = Resolve<IRepository<Core.Models.Property>>();
            this.subRepositoryUserLong = Resolve<IRepository<User, long>>();
            this.subRepositoryPropertyAppointment = Resolve<IRepository<Core.Models.PropertyAppointment>>();
            this.subRepositoryPropertySellerAvailableSlot = Resolve<IRepository<Core.Models.PropertySellerAvailableSlot>>();
            this.subHostingEnvironment = Resolve<IHostingEnvironment>();
            this.subHttpContextAccessor = Resolve<IHttpContextAccessor>();
            this.subConfiguration = Resolve<IConfiguration>();
            this.subLogger = Resolve<ILogger<PropertyLeaseForAgentAppService>>();
            this.subMetaEnumAppService = Resolve<MetaEnumAppService>();
            this.subAzureBlobManager = Resolve<IAzureBlobManager>();
            this.subRepositoryAgentPreferredSubDistrict = Resolve<IRepository<AgentPreferredSubDistrict>>();
            this.subMetaEnumManager = Resolve<IMetaEnumManager>();
        }

        private PropertyLeaseForAgentAppService CreateService()
        {
            return new PropertyLeaseForAgentAppService(
                this.subRepositoryPropertyLeaseSelectedAgent,
                this.subRepositoryPropertyLeaseInfo,
                this.subNotificationManager,
                this.subRepositoryPropertyImage,
                this.subRepositoryPropertyMeta,
                this.subRepositoryProperty,
                this.subRepositoryUserLong,
                this.subRepositoryPropertyAppointment,
                this.subRepositoryPropertySellerAvailableSlot,
                this.subHostingEnvironment,
                this.subHttpContextAccessor,
                this.subConfiguration,
                this.subLogger,
                this.subMetaEnumAppService,
                this.subAzureBlobManager,
                this.subRepositoryAgentPreferredSubDistrict,
                this.subMetaEnumManager);
        }

        [Fact]
        public void GetPropertyLeaseListForAgent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            GetPropertyLeaseListForAgentInput input1 = new GetPropertyLeaseListForAgentInput { Status = null };
            GetPropertyLeaseListForAgentInput input2 = new GetPropertyLeaseListForAgentInput { Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Confirm };
            GetPropertyLeaseListForAgentInput input3 = new GetPropertyLeaseListForAgentInput { Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Pending };
            GetPropertyLeaseListForAgentInput input4 = new GetPropertyLeaseListForAgentInput { Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject };



            // Act
            var result1 = service.GetPropertyLeaseListForAgent(
                input1);
            var result2 = service.GetPropertyLeaseListForAgent(
                input2);
            var result3 = service.GetPropertyLeaseListForAgent(
                input3);
            var result4 = service.GetPropertyLeaseListForAgent(
                input4);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Items.Count > 0);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result.Items.Count > 0);
            Assert.True(result3.Status == TaskStatus.RanToCompletion && result3.Result.Items.Count == 0);
            Assert.True(result4.Status == TaskStatus.RanToCompletion && result4.Result.Items.Count == 0);
        }

        [Fact]
        public void GetPropertyLeaseDetailForAgent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            GetPropertyLeaseDetailForAgentInput input1 = new GetPropertyLeaseDetailForAgentInput { PropertyID = 1, PropertyLeaseSelectedAgentID = 1 };
            GetPropertyLeaseDetailForAgentInput input2 = new GetPropertyLeaseDetailForAgentInput { PropertyID = 1, PropertyLeaseSelectedAgentID = 999 };
            GetPropertyLeaseDetailForAgentInput input3 = new GetPropertyLeaseDetailForAgentInput { PropertyID = 999, PropertyLeaseSelectedAgentID = 1 };


            // Act
            var result1 = service.GetPropertyLeaseDetailForAgent(
                input1);
            var result2 = service.GetPropertyLeaseDetailForAgent(
                input2);
            var result3 = service.GetPropertyLeaseDetailForAgent(
                input3);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Property != null);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result == null);
            Assert.True(result3.Status == TaskStatus.RanToCompletion && result3.Result.Property != null);

        }

        [Fact]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            UpdatePropertyLeaseSelectedAgentInput input1 = new UpdatePropertyLeaseSelectedAgentInput
            {
                ContractRefId = null,
                Id = 1,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Confirm
            };
            UpdatePropertyLeaseSelectedAgentInput input2 = new UpdatePropertyLeaseSelectedAgentInput
            {
                ContractRefId = 1,
                Id = 1,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
            };
            UpdatePropertyLeaseSelectedAgentInput input3 = new UpdatePropertyLeaseSelectedAgentInput
            {
                ContractRefId = 1,
                Id = 9999,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
            };
            UpdatePropertyLeaseSelectedAgentInput input4 = new UpdatePropertyLeaseSelectedAgentInput
            {
                ContractRefId = 1,
                Id = 1,
                PropertyId = 9999,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
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

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null); 
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result != null); 
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.RanToCompletion && result4.Result != null);
        }

        [Fact]
        public void UpdateContractRefID_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            UpdatePropertyLeaseSelectedAgentInput input1 = new UpdatePropertyLeaseSelectedAgentInput
            {
                ContractRefId = null,
                PropertyId = 1,
                Id = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Confirm
            };
            UpdatePropertyLeaseSelectedAgentInput input2 = new UpdatePropertyLeaseSelectedAgentInput
            {
                ContractRefId = 1,
                PropertyId = 1,
                Id = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
            };

            UpdatePropertyLeaseSelectedAgentInput input3 = new UpdatePropertyLeaseSelectedAgentInput
            {
                ContractRefId = 2,
                PropertyId = 9999,
                Id = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
            };

            UpdatePropertyLeaseSelectedAgentInput input4 = new UpdatePropertyLeaseSelectedAgentInput
            {
                ContractRefId = 4,
                PropertyId = 1,
                Id = 9999,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
            };
            // Act
            var result1 = service.UpdateContractRefID(
                input1);
            var result2 = service.UpdateContractRefID(
                input2);
            var result3 = service.UpdateContractRefID(
                input3);
            var result4 = service.UpdateContractRefID(
                input4);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result != null);
            Assert.True(result3.Status == TaskStatus.RanToCompletion && result3.Result != null);
            Assert.True(result4.Status == TaskStatus.Faulted); // not exist 
        }
    }
}
