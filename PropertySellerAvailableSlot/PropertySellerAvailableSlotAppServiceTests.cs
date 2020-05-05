using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.PropertySellerAvailableSlot;
using com.trustmechain.SmartME.Core.PropertySellerAvailableSlot;
using Abp.Runtime.Session;
using System.Collections.Generic;
using com.trustmechain.SmartME.Application.PropertySellerAvailableSlot.Dto;
using Abp.Application.Services.Dto;

namespace com.trustmechain.SmartME.Tests.PropertySellerAvailableSlot
{
    public class PropertySellerAvailableSlotAppServiceTests : SmartMETestBase
    {
        private ILogger<PropertySellerAvailableSlotAppService> subLogger;
        private IUnitOfWorkManager subUnitOfWorkManager;
        private IRepository<Core.Models.PropertySellerAvailableSlot, int> subRepository;
        private IPropertySellerAvailableSlotManager subPropertySellerAvailableSlotManager;

        public PropertySellerAvailableSlotAppServiceTests()
        {
            this.subLogger = Substitute.For<ILogger<PropertySellerAvailableSlotAppService>>();
            this.subUnitOfWorkManager = Substitute.For<IUnitOfWorkManager>();
            this.subRepository = Substitute.For<IRepository<Core.Models.PropertySellerAvailableSlot, int>>();
            this.subPropertySellerAvailableSlotManager = Substitute.For<IPropertySellerAvailableSlotManager>();
        }

        private PropertySellerAvailableSlotAppService CreateService()
        {
            return new PropertySellerAvailableSlotAppService(
                this.subLogger,
                this.subUnitOfWorkManager,
                this.subRepository,
                this.subPropertySellerAvailableSlotManager);
        }

        [Fact]
        public void CreatePropertySellerAvailableSlotList_V2_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            service.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();

            service.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            service.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            service.UnitOfWorkManager.Begin();




            CreatePropertySellerAvailableSlotInput_V2 input1 = new CreatePropertySellerAvailableSlotInput_V2
            {
                PropertyId = 1,
                propertySellerAvailableSlots =  new List<PropertySellerAvailableSlotDto>
                {
                    new PropertySellerAvailableSlotDto
                    {
                        PropertyId = 1,
                        StartDate = DateTime.Now,
                        StartTime = DateTime.Now,
                        EndDate = DateTime.Now,
                        EndTime = DateTime.Now,

                    }
                 }
            };

            CreatePropertySellerAvailableSlotInput_V2 input2 = new CreatePropertySellerAvailableSlotInput_V2
            {
                PropertyId = 9999,
                propertySellerAvailableSlots = new List<PropertySellerAvailableSlotDto>
                {
                    new PropertySellerAvailableSlotDto
                    {
                        PropertyId = 9999,
                        StartDate = DateTime.Now,
                        StartTime = DateTime.Now,
                        EndDate = DateTime.Now,
                        EndTime = DateTime.Now,

                    }
                 }
            };

            


            // Act
            var result1 = service.CreatePropertySellerAvailableSlotList_V2(
                input1);
            var result2 = service.CreatePropertySellerAvailableSlotList_V2(
                input2);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Items.Count > 0);  //unmapped object was found
            Assert.True(result2.Status == TaskStatus.RanToCompletion && result1.Result.Items.Count == 0);

        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertySellerAvailableSlot input = null;

            // Act
            var result = await service.GetAll(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            PropertySellerAvailableSlotDto input = null;

            // Act
            var result = await service.Create(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            PropertySellerAvailableSlotDto input = null;

            // Act
            var result = await service.Update(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            EntityDto input = null;

            // Act
            await service.Delete(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            EntityDto input = null;

            // Act
            var result = await service.Get(
                input);

            // Assert
            Assert.True(false);
        }
    }
}
