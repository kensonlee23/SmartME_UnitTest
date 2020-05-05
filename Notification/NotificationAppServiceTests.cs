using Abp.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.Notification;
using com.trustmechain.SmartME.Core.MetaEnum;
using com.trustmechain.SmartME.Core.Models.Notification;
using com.trustmechain.SmartME.Core.Notification;
using Abp.Runtime.Session;
using Abp.Domain.Uow;
using com.trustmechain.SmartME.Application.Notification.Dto;

namespace com.trustmechain.SmartME.Tests.Notification
{
    public class NotificationAppServiceTests : SmartMETestBase
    {
        private IRepository<Notification_Sendout_Log, int> subRepositoryNotification_Sendout_LogInt;
        private IRepository<Core.Models.Property> subRepositoryProperty;
        private IRepository<Core.Models.MetaEnum> subRepositoryMetaEnum;
        private ILogger<NotificationAppService> subLogger;
        private IMetaEnumManager subMetaEnumManager;
        private INotificationManager subNotificationManager;

        public NotificationAppServiceTests()
        {
            this.subRepositoryNotification_Sendout_LogInt = Substitute.For<IRepository<Notification_Sendout_Log, int>>();
            this.subRepositoryProperty = Substitute.For<IRepository<Core.Models.Property>>();
            this.subRepositoryMetaEnum = Substitute.For<IRepository<Core.Models.MetaEnum>>();
            this.subLogger = Substitute.For<ILogger<NotificationAppService>>();
            this.subMetaEnumManager = Substitute.For<IMetaEnumManager>();
            this.subNotificationManager = Substitute.For<INotificationManager>();
        }

        private NotificationAppService CreateService()
        {
            return new NotificationAppService(
                this.subRepositoryNotification_Sendout_LogInt,
                this.subRepositoryProperty,
                this.subRepositoryMetaEnum,
                this.subLogger,
                this.subMetaEnumManager,
                this.subNotificationManager);
        }

        [Fact]
        public void GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();
            GetAllPropertyAgentAssignListInput input1 = new GetAllPropertyAgentAssignListInput { MaxResultCount = 10, SkipCount = 0, Sorting = "" , Platform = Notification_Sendout_Log.NotificationPlatform.APortal};

            // Act
            var result1 = service.GetAll(
                input1);

            // Assert
            Assert.True (result1.Status == TaskStatus.RanToCompletion && result1.Result.Items.Count > 0);
        }

        [Fact]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            LoginAsHost("85261107946");
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();
            UpdateNotificationSendoutLogInput input1 = new UpdateNotificationSendoutLogInput { Id = 1, Status = Notification_Sendout_Log.NotificationStatus.Read };
            UpdateNotificationSendoutLogInput input2 = new UpdateNotificationSendoutLogInput { Id = 1, Status = Notification_Sendout_Log.NotificationStatus.Unread };
            // Act
            var result1 = service.Update(
                input1);
            var result2 = service.Update(
                input2);


            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion);
            Assert.True(result2.Status == TaskStatus.RanToCompletion);


        }

        [Fact]
        public void DeleteAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();

            var input1 = new DeleteAllInput { Platform = Notification_Sendout_Log.NotificationPlatform.APortal };
            var input2 = new DeleteAllInput { Platform = Notification_Sendout_Log.NotificationPlatform.CPortal };
            var input3 = new DeleteAllInput { Platform = null };


            // Act
            var result1 = service.DeleteAll(input1);
            var result2 = service.DeleteAll(input2);
            var result3 = service.DeleteAll(input3);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion);
            Assert.True(result2.Status == TaskStatus.RanToCompletion);
            Assert.True(result3.Status == TaskStatus.RanToCompletion);


        }

        [Fact]
        public async Task GetAllIncludeUnreadCount_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyAgentAssignListInput input = null;

            // Act
            var result = await service.GetAllIncludeUnreadCount(
                input);

            // Assert
            Assert.True(false);
        }
    }
}
