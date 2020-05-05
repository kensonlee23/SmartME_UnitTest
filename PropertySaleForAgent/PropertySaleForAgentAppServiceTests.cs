using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.MetaEnum;
using com.trustmechain.SmartME.Application.PropertySaleForAgent;
using com.trustmechain.SmartME.Core.Authorization.Users;
using com.trustmechain.SmartME.Core.Azure;
using com.trustmechain.SmartME.Core.MetaEnum;
using com.trustmechain.SmartME.Core.Models;
using com.trustmechain.SmartME.Core.Models.Agent;
using com.trustmechain.SmartME.Core.Notification;
using com.trustmechain.SmartME.Application.PropertySaleForAgent.Dto;
using Abp.Runtime.Session;
using Abp.Domain.Uow;

namespace com.trustmechain.SmartME.Tests.PropertySaleForAgent
{
    public class PropertySaleForAgentAppServiceTests : SmartMETestBase
    {
        private IRepository<Core.Models.PropertySaleSelectedAgent> subRepositoryPropertySaleSelectedAgent;
        private IRepository<PropertySaleInfo> subRepositoryPropertySaleInfo;
        private INotificationManager subNotificationManager;
        private IRepository<Core.Models.PropertyImage> subRepositoryPropertyImage;
        private IRepository<PropertyMeta> subRepositoryPropertyMeta;
        private IRepository<Core.Models.Property> subRepositoryProperty;
        private IRepository<User, long> subRepositoryUserLong;
        private IRepository<Core.Models.PropertyAppointment> subRepositoryPropertyAppointment;
        private IRepository<Core.Models.PropertySellerAvailableSlot> subRepositoryPropertySellerAvailableSlot;
        private IHttpContextAccessor subHttpContextAccessor;
        private IConfiguration subConfiguration;
        private ILogger<PropertySaleForAgentAppService> subLogger;
        private MetaEnumAppService subMetaEnumAppService;
        private IAzureBlobManager subAzureBlobManager;
        private IRepository<AgentPreferredSubDistrict> subRepositoryAgentPreferredSubDistrict;
        private IMetaEnumManager subMetaEnumManager;

        public PropertySaleForAgentAppServiceTests()
        {
            this.subRepositoryPropertySaleSelectedAgent = Resolve<IRepository<Core.Models.PropertySaleSelectedAgent>>();
            this.subRepositoryPropertySaleInfo = Resolve<IRepository<PropertySaleInfo>>();
            this.subNotificationManager = Resolve<INotificationManager>();
            this.subRepositoryPropertyImage = Resolve<IRepository<Core.Models.PropertyImage>>();
            this.subRepositoryPropertyMeta = Resolve<IRepository<PropertyMeta>>();
            this.subRepositoryProperty = Resolve<IRepository<Core.Models.Property>>();
            this.subRepositoryUserLong = Resolve<IRepository<User, long>>();
            this.subRepositoryPropertyAppointment = Resolve<IRepository<Core.Models.PropertyAppointment>>();
            this.subRepositoryPropertySellerAvailableSlot = Resolve<IRepository<Core.Models.PropertySellerAvailableSlot>>();
            this.subHttpContextAccessor = Resolve<IHttpContextAccessor>();
            this.subConfiguration = Resolve<IConfiguration>();
            this.subLogger = Resolve<ILogger<PropertySaleForAgentAppService>>();
            this.subMetaEnumAppService = Resolve<MetaEnumAppService>();
            this.subAzureBlobManager = Resolve<IAzureBlobManager>();
            this.subRepositoryAgentPreferredSubDistrict = Resolve<IRepository<AgentPreferredSubDistrict>>();
            this.subMetaEnumManager = Resolve<IMetaEnumManager>();
        }

        private PropertySaleForAgentAppService CreateService()
        {
            return new PropertySaleForAgentAppService(
                this.subRepositoryPropertySaleSelectedAgent,
                this.subRepositoryPropertySaleInfo,
                this.subNotificationManager,
                this.subRepositoryPropertyImage,
                this.subRepositoryPropertyMeta,
                this.subRepositoryProperty,
                this.subRepositoryUserLong,
                this.subRepositoryPropertyAppointment,
                this.subRepositoryPropertySellerAvailableSlot,
                this.subHttpContextAccessor,
                this.subConfiguration,
                this.subLogger,
                this.subMetaEnumAppService,
                this.subAzureBlobManager,
                this.subRepositoryAgentPreferredSubDistrict,
                this.subMetaEnumManager);
        }

        [Fact]
        public void GetPropertySaleListForAgent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            GetPropertySaleListForAgentInput input1 = new GetPropertySaleListForAgentInput { Status = null };
            GetPropertySaleListForAgentInput input2 = new GetPropertySaleListForAgentInput { Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Confirm };
            GetPropertySaleListForAgentInput input3 = new GetPropertySaleListForAgentInput { Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Pending };
            GetPropertySaleListForAgentInput input4 = new GetPropertySaleListForAgentInput { Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject };


            // Act
            var result1 = service.GetPropertySaleListForAgent(
                input1);
            var result2 = service.GetPropertySaleListForAgent(
                input2);
            var result3 = service.GetPropertySaleListForAgent(
                input3);
            var result4 = service.GetPropertySaleListForAgent(
                input4);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Items.Count > 0);
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result.Items.Count == 0);
            Assert.True(result3.Status == TaskStatus.RanToCompletion && result3.Result.Items.Count > 0);
            Assert.True(result4.Status == TaskStatus.RanToCompletion && result4.Result.Items.Count == 0);
        }

        [Fact]
        public void GetPropertySaleDetailForAgent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            GetPropertySaleDetailForAgentInput input1 = new GetPropertySaleDetailForAgentInput { PropertyID = 1, PropertySaleSelectedAgentId = 1 };
            GetPropertySaleDetailForAgentInput input2 = new GetPropertySaleDetailForAgentInput { PropertyID = 1, PropertySaleSelectedAgentId = 9999 };
            GetPropertySaleDetailForAgentInput input3 = new GetPropertySaleDetailForAgentInput { PropertyID = 9999, PropertySaleSelectedAgentId = 1 };

            // Act
            var result1 = service.GetPropertySaleDetailForAgent(
                input1);
            var result2 = service.GetPropertySaleDetailForAgent(
                input2);
            var result3 = service.GetPropertySaleDetailForAgent(
                input3);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null);   //error mapping PropertyImageDto
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result == null);
            Assert.True(result3.Status == TaskStatus.RanToCompletion);
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

            UpdatePropertySaleSelectedAgentInput input1 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = null,
                Id = 1,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Confirm
            };

            UpdatePropertySaleSelectedAgentInput input2 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = 1,
                Id = 1,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Pending
            };

            UpdatePropertySaleSelectedAgentInput input3 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = 1,
                Id = 1,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
            };

            UpdatePropertySaleSelectedAgentInput input4 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = 1,
                Id = 9999,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Pending
            };

            UpdatePropertySaleSelectedAgentInput input5 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = 1,
                Id = 1,
                PropertyId = 9999,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Pending
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


            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result != null); //waitingforactivation
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result2.Result != null);
            Assert.True(result3.Status == TaskStatus.RanToCompletion && result3.Result != null);
            Assert.True(result4.Status == TaskStatus.Faulted);
            Assert.True(result5.Status == TaskStatus.RanToCompletion && result5.Result != null);


        }

        [Fact]
        public void UpdateContractRefID_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            UpdatePropertySaleSelectedAgentInput input1 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = null,
                Id = 1,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Confirm
            };

            UpdatePropertySaleSelectedAgentInput input2 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = 1,
                Id = 1,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Pending
            };

            UpdatePropertySaleSelectedAgentInput input3 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = null,
                Id = 9999,
                PropertyId = 1,
                Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
            };

            UpdatePropertySaleSelectedAgentInput input4 = new UpdatePropertySaleSelectedAgentInput
            {
                ContractRefId = null,
                Id = 1,
                PropertyId = 9999,
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
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.RanToCompletion && result4.Result != null);
        }
    }
}
