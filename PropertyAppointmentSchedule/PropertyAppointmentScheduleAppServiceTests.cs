using Abp.Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.PropertyAppointmentSchedule;
using com.trustmechain.SmartME.Core.Notification;
using com.trustmechain.SmartME.Core.PropertyAppointment;
using Abp.Runtime.Session;
using Abp.Domain.Uow;
using com.trustmechain.SmartME.Application.PropertyAppointmentSchedule.Dto;

namespace com.trustmechain.SmartME.Tests.PropertyAppointmentSchedule
{
    public class PropertyAppointmentScheduleAppServiceTests : SmartMETestBase
    {
        private IRepository<Core.Models.PropertyAppointmentSchedule, int> subRepositoryPropertyAppointmentScheduleInt;
        private IRepository<Core.Models.PropertyAppointment> subRepositoryPropertyAppointment;
        private ILogger<PropertyAppointmentScheduleAppService> subLogger;
        private INotificationManager subNotificationManager;
        private IPropertyAppointmentManager subPropertyAppointmentManager;

        public PropertyAppointmentScheduleAppServiceTests()
        {
            this.subRepositoryPropertyAppointmentScheduleInt = Resolve<IRepository<Core.Models.PropertyAppointmentSchedule, int>>();
            this.subRepositoryPropertyAppointment = Resolve<IRepository<Core.Models.PropertyAppointment>>();
            this.subLogger = Resolve<ILogger<PropertyAppointmentScheduleAppService>>();
            this.subNotificationManager = Resolve<INotificationManager>();
            this.subPropertyAppointmentManager = Resolve<IPropertyAppointmentManager>();
        }

        private PropertyAppointmentScheduleAppService CreateService()
        {
            return new PropertyAppointmentScheduleAppService(
                this.subRepositoryPropertyAppointmentScheduleInt,
                this.subRepositoryPropertyAppointment,
                this.subLogger,
                this.subNotificationManager,
                this.subPropertyAppointmentManager);
        }

        [Fact]
        public void Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            service.AbpSession = Resolve<IAbpSession>();
            //LoginAsTenant("Default","85261586394");
            LoginAsHost("85261107946");
            //service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            //service.UnitOfWorkManager.Begin();

            UpdatePropertyAppointmentScheduleInput input1 = new UpdatePropertyAppointmentScheduleInput
            {
                Id = 1,
                AppointmentDate = DateTime.Now,
                EndTime = DateTime.Now,
                StartTime = DateTime.Now,
                PropertyAppointmentId = 1,
                Status = Core.Models.PropertyAppointmentSchedule.PropertyAppointmentScheduleStatus.Reject,
            };
            UpdatePropertyAppointmentScheduleInput input2 = new UpdatePropertyAppointmentScheduleInput
            {
                Id = 9999,
                AppointmentDate = DateTime.Now,
                EndTime = DateTime.Now,
                StartTime = DateTime.Now,
                PropertyAppointmentId = 999,
                Status = Core.Models.PropertyAppointmentSchedule.PropertyAppointmentScheduleStatus.Confirm,
            };
            UpdatePropertyAppointmentScheduleInput input3 = new UpdatePropertyAppointmentScheduleInput
            {
                Id = 1,
                AppointmentDate = DateTime.Now,
                EndTime = DateTime.Now,
                StartTime = DateTime.Now,
                PropertyAppointmentId = 999,
                Status = Core.Models.PropertyAppointmentSchedule.PropertyAppointmentScheduleStatus.Pending,
            };

            // Act
            var result1 = service.Update(
                input1);
            var result2 = service.Update(
                input2);
            var result3 = service.Update(
                input3);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion); // status now is waitingforactivation
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);

        }
    }
}
