using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using com.trustmechain.SmartME.Application.Property;
using com.trustmechain.SmartME.Application.PropertyImage;
using com.trustmechain.SmartME.Application.SignalRHub;
using com.trustmechain.SmartME.Core.Agent;
using com.trustmechain.SmartME.Core.Authorization.Users;
using com.trustmechain.SmartME.Core.Azure;
using com.trustmechain.SmartME.Core.Models;
using com.trustmechain.SmartME.Core.Models.Agent;
using com.trustmechain.SmartME.Core.Notification;
using com.trustmechain.SmartME.Core.Property;
using com.trustmechain.SmartME.Core.PropertyImage;
using com.trustmechain.SmartME.Core.Report;
using com.trustmechain.SmartME.Core.Survey;
using Abp.Runtime.Session;
using com.trustmechain.SmartME.Application.Property.Dto;
using com.trustmechain.SmartME.Application.Notification.Dto;
using Abp.Timing;
using Task = System.Threading.Tasks.Task;
using Abp.Application.Services.Dto;

namespace com.trustmechain.SmartME.Tests.Property
{
    public class PropertyAppServiceTests : SmartMETestBase
    {
        private IRepository<Core.Models.Property> subRepositoryProperty;
        private IRepository<PropertyLeaseInfo> subRepositoryPropertyLeaseInfo;
        private IRepository<PropertySaleInfo> subRepositoryPropertySaleInfo;
        private IRepository<PropertySaleSelectedAgent> subRepositoryPropertySaleSelectedAgent;
        private IRepository<PropertyLeaseSelectedAgent> subRepositoryPropertyLeaseSelectedAgent;
        private IRepository<Core.Models.PropertyImage> subRepositoryPropertyImage;
        private IRepository<PropertyMeta> subRepositoryPropertyMeta;
        private IRepository<User, long> subRepositoryUserLong;
        private IRepository<Core.Models.PropertyAppointment> subRepositoryPropertyAppointment;
        private INotificationManager subNotificationManager;
        private IHostingEnvironment subHostingEnvironment;
        private IHttpContextAccessor subHttpContextAccessor;
        private ILogger<PropertyAppService> subLogger;
        private IUnitOfWorkManager subUnitOfWorkManager;
        private IRepository<Core.Models.PropertyFollowUp> subRepositoryPropertyFollowUp;
        private IConfiguration subConfiguration;
        private IAzureBlobManager subAzureBlobManager;
        private IPropertyImageAppService subPropertyImageAppService;
        private IRepository<Core.Models.View.AgentFlatSimpleConsolidateView, long> subRepositoryAgentFlatSimpleConsolidateViewLong;
        private IPropertyManager subPropertyManager;
        private IAgentManager subAgentManager;
        private IPropertyImageManager subPropertyImageManager;
        private IRepository<AgentProfile, long> subRepositoryAgentProfileLong;
        private IRepository<Core.Models.View.PropertyConsolidateListView> subRepositoryPropertyConsolidateListView;
        private IRepository<Core.Models.PropertySelectedAgent> subRepositoryPropertySelectedAgent;
        private IRepository<Core.Models.Survey> subRepositorySurvey;
        private IRepository<Core.Models.PropertySellerAvailableSlot> subRepositoryPropertySellerAvailableSlot;
        private IHubContext<PropertyHub> subHubContext;
        private UserManager subUserManager;
        private IReportManager subReportManager;
        private ISurveyManager subSurveyManager;
        private ISettingManager subSettingManager;

        public PropertyAppServiceTests()
        {
            this.subRepositoryProperty = Resolve<IRepository<Core.Models.Property>>();
            this.subRepositoryPropertyLeaseInfo = Resolve<IRepository<PropertyLeaseInfo>>();
            this.subRepositoryPropertySaleInfo = Resolve<IRepository<PropertySaleInfo>>();
            this.subRepositoryPropertySaleSelectedAgent = Resolve<IRepository<PropertySaleSelectedAgent>>();
            this.subRepositoryPropertyLeaseSelectedAgent = Resolve<IRepository<PropertyLeaseSelectedAgent>>();
            this.subRepositoryPropertyImage = Resolve<IRepository<Core.Models.PropertyImage>>();
            this.subRepositoryPropertyMeta = Resolve<IRepository<PropertyMeta>>();
            this.subRepositoryUserLong = Resolve<IRepository<User, long>>();
            this.subRepositoryPropertyAppointment = Resolve<IRepository<Core.Models.PropertyAppointment>>();
            this.subNotificationManager = Resolve<INotificationManager>();
            this.subHostingEnvironment = Resolve<IHostingEnvironment>();
            this.subHttpContextAccessor = Resolve<IHttpContextAccessor>();
            this.subLogger = Resolve<ILogger<PropertyAppService>>();
            this.subUnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            this.subRepositoryPropertyFollowUp = Resolve<IRepository<Core.Models.PropertyFollowUp>>();
            this.subConfiguration = Resolve<IConfiguration>();
            this.subAzureBlobManager = Resolve<IAzureBlobManager>();
            this.subPropertyImageAppService = Resolve<IPropertyImageAppService>();
            this.subRepositoryAgentFlatSimpleConsolidateViewLong = Resolve<IRepository<Core.Models.View.AgentFlatSimpleConsolidateView, long>>();
            this.subPropertyManager = Resolve<IPropertyManager>();
            this.subAgentManager = Resolve<IAgentManager>();
            this.subPropertyImageManager = Resolve<IPropertyImageManager>();
            this.subRepositoryAgentProfileLong = Resolve<IRepository<AgentProfile, long>>();
            this.subRepositoryPropertyConsolidateListView = Resolve<IRepository<Core.Models.View.PropertyConsolidateListView>>();
            this.subRepositoryPropertySelectedAgent = Resolve<IRepository<Core.Models.PropertySelectedAgent>>();
            this.subRepositorySurvey = Resolve<IRepository<Core.Models.Survey>>();
            this.subRepositoryPropertySellerAvailableSlot = Resolve<IRepository<Core.Models.PropertySellerAvailableSlot>>();
            this.subHubContext = Resolve<IHubContext<PropertyHub>>();
            this.subUserManager = Resolve<UserManager>();
            this.subReportManager = Resolve<IReportManager>();
            this.subSurveyManager = Resolve<ISurveyManager>();
            this.subSettingManager = Resolve<ISettingManager>();
        }

        private PropertyAppService CreateService()
        {
            return new PropertyAppService(
                this.subRepositoryProperty,
                this.subRepositoryPropertyLeaseInfo,
                this.subRepositoryPropertySaleInfo,
                this.subRepositoryPropertySaleSelectedAgent,
                this.subRepositoryPropertyLeaseSelectedAgent,
                this.subRepositoryPropertyImage,
                this.subRepositoryPropertyMeta,
                this.subRepositoryUserLong,
                this.subRepositoryPropertyAppointment,
                this.subNotificationManager,
                this.subHostingEnvironment,
                this.subHttpContextAccessor,
                this.subLogger,
                this.subUnitOfWorkManager,
                this.subRepositoryPropertyFollowUp,
                this.subConfiguration,
                this.subAzureBlobManager,
                this.subPropertyImageAppService,
                this.subRepositoryAgentFlatSimpleConsolidateViewLong,
                this.subPropertyManager,
                this.subAgentManager,
                this.subPropertyImageManager,
                this.subRepositoryAgentProfileLong,
                this.subRepositoryPropertyConsolidateListView,
                this.subRepositoryPropertySelectedAgent,
                this.subRepositorySurvey,
                this.subRepositoryPropertySellerAvailableSlot,
                this.subHubContext,
                this.subUserManager,
                this.subAzureBlobManager,
                this.subReportManager,
                this.subSurveyManager,
                this.subSettingManager);
        }

        //[Fact]
        //public async Task Delete_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var service = this.CreateService();
        //    EntityDto input = null;

        //    // Act
        //    await service.Delete(
        //        input);

        //    // Assert
        //    Assert.True(false);
        //}

        [Fact]
        public virtual void CompletePropertyRecord_StateUnderTest_ExpectedBehavior()
        {
            LoginAsDefaultTenantAdmin();
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();


            CompletePropertyRecordInput input1 = new CompletePropertyRecordInput
            {
                PropertySaleInfo = new CreatePropertySaleInfoDto()
                {
                    CommissionAmount = 46666,
                    CommissionRate = 15,
                    CommissionType = "aaa",
                    IsPublic = true,
                    IsVacuum = true,
                    Price = 100000,
                    PropertyId = 1,
                    PublishEndTime = DateTime.Now,
                    PublishStartTime = DateTime.Now,
                    Remarks = "remark",
                    VacuumDate = DateTime.Now
                },
                PropertyLeaseInfo = new CreatePropertyLeaseInfoDto()
                {
                    CommissionAmount = 46666,
                    CommissionRate = 15,
                    CommissionType = "aaa",
                    IsPublic = true,
                    IsVacuum = true,
                    MonthlyRent = 100000,
                    PropertyId = 1,
                    PublishEndTime = DateTime.Now,
                    PublishStartTime = DateTime.Now,
                    Remarks = "remark",
                    VacuumDate = DateTime.Now,
                    DepositMonth = 10000,
                    DepositRate = 10,
                    DepositType = "M",
                    IsIncludeFurniture = true,
                    IsPublicAfterComplete = true,
                    LeaseMinMonth = 3
                }
            };

            CompletePropertyRecordInput input2 = new CompletePropertyRecordInput
            {
                PropertySaleInfo = new CreatePropertySaleInfoDto()
                {
                    CommissionAmount = 46666,
                    CommissionRate = 15,
                    CommissionType = "aaa",
                    IsPublic = true,
                    IsVacuum = true,
                    Price = 100000,
                    PropertyId = 1,
                    PublishEndTime = DateTime.Now,
                    PublishStartTime = DateTime.Now,
                    Remarks = "remark",
                    VacuumDate = DateTime.Now
                },
                PropertyLeaseInfo = null
            };

            CompletePropertyRecordInput input3 = new CompletePropertyRecordInput
            {
                PropertySaleInfo = null,
                PropertyLeaseInfo = new CreatePropertyLeaseInfoDto()
                {
                    CommissionAmount = 46666,
                    CommissionRate = 15,
                    CommissionType = "aaa",
                    IsPublic = true,
                    IsVacuum = true,
                    MonthlyRent = 100000,
                    PropertyId = 1,
                    PublishEndTime = DateTime.Now,
                    PublishStartTime = DateTime.Now,
                    Remarks = "remark",
                    VacuumDate = DateTime.Now,
                    DepositMonth = 10000,
                    DepositRate = 10,
                    DepositType = "M",
                    IsIncludeFurniture = true,
                    IsPublicAfterComplete = true,
                    LeaseMinMonth = 3
                }
            };

            CompletePropertyRecordInput input4 = new CompletePropertyRecordInput { PropertySaleInfo = null, PropertyLeaseInfo = null };

            // Act
            var result1 = unitUnderTest.CompletePropertyRecord(
            input1);
            var result2 = unitUnderTest.CompletePropertyRecord(
                input2);
            var result3 = unitUnderTest.CompletePropertyRecord(
                input3);
            var result4 = unitUnderTest.CompletePropertyRecord(
                input4);

            // Assert
            Assert.True(result1.Status == TaskStatus.WaitingForActivation); //waitingforactivation status
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.Status == TaskStatus.Faulted);
            Assert.True(result4.Status == TaskStatus.RanToCompletion);

        }

        [Fact]
        public async void CreatePropertySelectedAgent_StateUnderTest_ExpectedBehavior()
        {
            var unitUnderTest = this.CreateService();
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();


            // Arrange
            CreatePropertySelectedAgentInput input1 = new CreatePropertySelectedAgentInput
            {
                PropertySaleInfoID = null,
                PropertyLeaseInfoID = null,
                PropertyID = null,
                NotificationSendoutLogInput = null
            };

            CreatePropertySelectedAgentInput input2 = new CreatePropertySelectedAgentInput
            {
                PropertySaleInfoID = 1,
                PropertyLeaseInfoID = null,
                PropertyID = 1,
                NotificationSendoutLogInput = new System.Collections.Generic.List<NotificationSendoutLogInput>() { new NotificationSendoutLogInput { AgentCompanyBranchID = long.Parse("2") } }
            };

            CreatePropertySelectedAgentInput input3 = new CreatePropertySelectedAgentInput
            {
                PropertySaleInfoID = null,
                PropertyLeaseInfoID = 1,
                PropertyID = 1,
                NotificationSendoutLogInput = new System.Collections.Generic.List<NotificationSendoutLogInput>() { new NotificationSendoutLogInput { AgentCompanyBranchID = long.Parse("2") } }
            };

            CreatePropertySelectedAgentInput input4 = new CreatePropertySelectedAgentInput
            {
                PropertySaleInfoID = 1,
                PropertyLeaseInfoID = 1,
                PropertyID = 1,
                NotificationSendoutLogInput = new System.Collections.Generic.List<NotificationSendoutLogInput>() { new NotificationSendoutLogInput { AgentCompanyBranchID = long.Parse("2") } }
            };

            // Act
            var result1 = unitUnderTest.CreatePropertySelectedAgent(input1);
            var result2 = await unitUnderTest.CreatePropertySelectedAgent(input2);
            var result3 = await unitUnderTest.CreatePropertySelectedAgent(input3);
            var result4 = await unitUnderTest.CreatePropertySelectedAgent(input4);

            // Assert
            Assert.True(result1.Status == TaskStatus.Faulted);
            Assert.True(result2.PropertyReferenceNumber != null); //pending to solve
            Assert.True(result3.PropertyReferenceNumber != null); //pending to solve
            Assert.True(result4.PropertyReferenceNumber == null);  //pending to solve

        }

        //[Fact]
        //public async Task CreatePropertySelectedAgent_v2_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var service = this.CreateService();
        //    CreatePropertySelectedAgent_v2Input input = null;

        //    // Act
        //    var result = await service.CreatePropertySelectedAgent_v2(
        //        input);

        //    // Assert
        //    Assert.True(false);
        //}

        [Fact]
        public void GetPropertyLeaseList_StateUnderTest_ExpectedBehavior()
        {
            var unitUnderTest = this.CreateService();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();

            // Arrange
            GetAllPropertyListInput input1 = new GetAllPropertyListInput { MaxResultCount = 20, SkipCount = 0, Sorting = "" };


            // Act
            var result1 = unitUnderTest.GetPropertyLeaseList(
                input1);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion);// && result1.Result.Items.Count > 0);
        }

        [Fact]
        public async void Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();
            LoginAsHost("85261107946");

            CreatePropertyRecordInput input1 = new CreatePropertyRecordInput
            {
                Id = 2,
                ActualSize = 1241242,
                AddressChT = "AddressChT",
                AddressEng = "AddressEng",
                Age = 10,
                ConstructSize = 124124,
                Decoration = MetaEnum.PropertyDecoration.L,
                Description = "Description",
                Direction = MetaEnum.PropertyDirection.N,
                flat = "flat",
                floor = "floor",
                MetaRef = new PropertyMetaDto
                {
                    DistrictCode = "CW",
                    BlockDescriptorPrecedenceIndicator = "BlockDescriptorPrecedenceIndicator",
                    BlockDescriptor_ChT = "BlockDescriptor_ChT",
                    BlockDescriptor_Eng = "BlockDescriptor_Eng",
                    BlockNo = "BlockNo",
                    BuildingName_ChT = "BuildingName_ChT",
                    BuildingName_Eng = "BuildingName_Eng",
                    EstateName_ChT = "EstateName_ChT",
                    EstateName_Eng = "EstateName_Eng",
                    GPS_Easting = "GPS_Easting",
                    GPS_Latitude = "GPS_Latitude",
                    GPS_Longitude = "GPS_Longitude",
                    GPS_Northing = "GPS_Northing",
                    PhaseName_ChT = "PhaseName_ChT",
                    PhaseName_Eng = "PhaseName_Eng",
                    PhaseNo = "PhaseNo",
                    PSchoolNetwork = "PSchoolNetwork",
                    RegionCode = "RegionCode",
                    SSchoolNetwork = "SSchoolNetwork",
                    StreetBuildingNoFrom = "StreetBuildingNoFrom",
                    StreetBuildingNoTo = "StreetBuildingNoTo",
                    StreetName_ChT = "StreetName_ChT",
                    StreetName_Eng = "StreetName_Eng",
                    VillageBuildingNoFrom = "VillageBuildingNoFrom",
                    VillageBuildingNoTo = "VillageBuildingNoTo",
                    VillageName_ChT = "VillageName_ChT",
                    VillageName_Eng = "VillageName_Eng"
                }
                ,
                OwnerId = null,
                Scene = "Scene",
                SizeUnit = "SizeUnit",
                Structure_Balcony = 1,
                Structure_BathRoom = 1,
                Structure_IsOpen = true,
                Structure_LabourRoom = 1,
                Structure_LivingRoom = 1,
                Structure_Room = 1,
                Structure_Suite = 1,
                Structure_Toilet = 1,
                Tag = "Tag",
                Usage = "R",
                DistrictCode = "CW"
            };

            CreatePropertyRecordInput input2 = new CreatePropertyRecordInput
            {
                Id = 3,
                ActualSize = null,
                AddressChT = null,
                AddressEng = null,
                Age = null,
                ConstructSize = null,
                Decoration = null,
                Description = null,
                Direction = null,
                flat = null,
                floor = null,
                MetaRef = null,
                OwnerId = null,
                Scene = null,
                SizeUnit = null,
                Structure_Balcony = null,
                Structure_BathRoom = null,
                Structure_IsOpen = null,
                Structure_LabourRoom = null,
                Structure_LivingRoom = null,
                Structure_Room = null,
                Structure_Suite = null,
                Structure_Toilet = null,
                Tag = null,
                Usage = null,
                DistrictCode = null
            };

            CreatePropertyRecordInput input3 = new CreatePropertyRecordInput
            {
                Id = 4,
                ActualSize = 1241242,
                AddressChT = "AddressChT",
                AddressEng = "AddressEng",
                Age = 10,
                ConstructSize = 124124,
                Decoration =  MetaEnum.PropertyDecoration.N,
                Description = "Description",
                Direction =  MetaEnum.PropertyDirection.NW,
                flat = "flat",
                floor = "floor",
                MetaRef = new PropertyMetaDto
                {
                    DistrictCode = "CW",
                    BlockDescriptorPrecedenceIndicator = "BlockDescriptorPrecedenceIndicator",
                    BlockDescriptor_ChT = "BlockDescriptor_ChT",
                    BlockDescriptor_Eng = "BlockDescriptor_Eng",
                    BlockNo = "BlockNo",
                    BuildingName_ChT = "BuildingName_ChT",
                    BuildingName_Eng = "BuildingName_Eng",
                    EstateName_ChT = "EstateName_ChT",
                    EstateName_Eng = "EstateName_Eng",
                    GPS_Easting = "GPS_Easting",
                    GPS_Latitude = "GPS_Latitude",
                    GPS_Longitude = "GPS_Longitude",
                    GPS_Northing = "GPS_Northing",
                    PhaseName_ChT = "PhaseName_ChT",
                    PhaseName_Eng = "PhaseName_Eng",
                    PhaseNo = "PhaseNo",
                    PSchoolNetwork = "PSchoolNetwork",
                    RegionCode = "RegionCode",
                    SSchoolNetwork = "SSchoolNetwork",
                    StreetBuildingNoFrom = "StreetBuildingNoFrom",
                    StreetBuildingNoTo = "StreetBuildingNoTo",
                    StreetName_ChT = "StreetName_ChT",
                    StreetName_Eng = "StreetName_Eng",
                    VillageBuildingNoFrom = "VillageBuildingNoFrom",
                    VillageBuildingNoTo = "VillageBuildingNoTo",
                    VillageName_ChT = "VillageName_ChT",
                    VillageName_Eng = "VillageName_Eng"
                }
                ,
                OwnerId = 2,
                Scene = "Scene",
                SizeUnit = "SizeUnit",
                Structure_Balcony = 1,
                Structure_BathRoom = 1,
                Structure_IsOpen = true,
                Structure_LabourRoom = 1,
                Structure_LivingRoom = 1,
                Structure_Room = 1,
                Structure_Suite = 1,
                Structure_Toilet = 1,
                Tag = "Tag",
                Usage = "R",
                DistrictCode = "CW"
            };

            // Act

            var result1 = await unitUnderTest.Create(
            input1);
            var result2 = unitUnderTest.Create(
                input2);
            var result3 = await unitUnderTest.Create(
                input3);

            // Assert
            Assert.True(result1.PropertyReferenceNumber == ("C" + Clock.Now.Year + Clock.Now.Month + Clock.Now.Day + "2001"));
            Assert.True(result2.Status == TaskStatus.Faulted);
            Assert.True(result3.PropertyReferenceNumber == ("C" + Clock.Now.Year + Clock.Now.Month + Clock.Now.Day + "2002"));

        }

        [Fact]
        public void GetPropertySaleList_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();

            GetAllPropertyListInput input1 = new GetAllPropertyListInput { MaxResultCount = 20, SkipCount = 0, Sorting = "" };

            // Act
            var result1 = unitUnderTest.GetPropertySaleList(
                input1);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Items.Count > 0);
        }

        [Fact]
        public void GetPropertyLeaseDetail_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();
            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();


            GetPropertyLeaseDetailInput input1 = new GetPropertyLeaseDetailInput { PropertyID = 1, PropertyLeaseInfoID = 1 };
            GetPropertyLeaseDetailInput input2 = new GetPropertyLeaseDetailInput { PropertyID = 99, PropertyLeaseInfoID = 99 };
            

            // Act
            var result1 = unitUnderTest.GetPropertyLeaseDetail(
                input1);
            var result2 = unitUnderTest.GetPropertyLeaseDetail(
                input2);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Property != null);
            Assert.True(result2.Status == TaskStatus.Faulted);
        }

        [Fact]
        public void GetPropertySaleDetail_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateService();
            unitUnderTest.AbpSession = Resolve<IAbpSession>();
            LoginAsHost("85261107946");
            unitUnderTest.UnitOfWorkManager = Resolve<IUnitOfWorkManager>();
            unitUnderTest.UnitOfWorkManager.Begin();

            GetPropertySaleDetailInput input1 = new GetPropertySaleDetailInput { PropertyID = 1, PropertySaleInfoID = 1 };
            GetPropertySaleDetailInput input2 = new GetPropertySaleDetailInput { PropertyID = 99, PropertySaleInfoID = 99 };
            

            unitUnderTest.ObjectMapper = LocalIocManager.Resolve<Abp.ObjectMapping.IObjectMapper>();

            // Act
            var result1 = unitUnderTest.GetPropertySaleDetail(
                input1);
            var result2 = unitUnderTest.GetPropertySaleDetail(
                input2);

            // Assert
            Assert.True(result1.Status == TaskStatus.RanToCompletion && result1.Result.Property != null);
            Assert.True(result2.Status == TaskStatus.Faulted);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyListInput input = null;

            // Act
            var result = await service.GetAll(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetAllForAdmin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyListInput input = null;

            // Act
            var result = await service.GetAllForAdmin(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetConsolidateListForAdmin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyListInput input = null;

            // Act
            var result = await service.GetConsolidateListForAdmin(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetConsolidateListForAdmin2_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyListInput input = null;

            // Act
            var result = await service.GetConsolidateListForAdmin2(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetConsolidateListForAgentAdmin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyAgentAdminInput input = null;

            // Act
            var result = await service.GetConsolidateListForAgentAdmin(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task RequestPropertyByAgent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            EntityDto input = null;

            // Act
            var result = await service.RequestPropertyByAgent(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdatePropertyRecordInput input = null;

            // Act
            var result = await service.Update(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdatePropertyLeaseInfo_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdatePropertyLeaseInfoInput input = null;

            // Act
            var result = await service.UpdatePropertyLeaseInfo(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdatePropertySaleInfo_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdatePropertySaleInfoInput input = null;

            // Act
            var result = await service.UpdatePropertySaleInfo(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdatePropertyIsVerified_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdatePropertyIsVerifiedInput input = null;

            // Act
            var result = await service.UpdatePropertyIsVerified(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetPropertyDetailForBuyer_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string input = null;

            // Act
            var result = await service.GetPropertyDetailForBuyer(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetPropertyDetailForAgent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string input = null;

            // Act
            var result = await service.GetPropertyDetailForAgent(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task CreatePropertyInfoAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            CreatePropertyInfoAllInput input = null;

            // Act
            var result = await service.CreatePropertyInfoAll(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetPropertyDetail_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetPropertyDetailInput input = null;

            // Act
            var result = await service.GetPropertyDetail(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetPropertyListForClient_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyListInput input = null;

            // Act
            var result = await service.GetPropertyListForClient(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetPropertyDetailForClient_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetPropertyDetailForClientInput input = null;

            // Act
            var result = await service.GetPropertyDetailForClient(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetFavPropertyListForClient_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetFavPropertyListForClientInput input = null;

            // Act
            var result = await service.GetFavPropertyListForClient(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetRecommendProperty_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = await service.GetRecommendProperty();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetPropertyListForAgentAdmin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetAllPropertyAgentAdminInput input = null;

            // Act
            var result = await service.GetPropertyListForAgentAdmin(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetPropertyDetailForAgentAdmin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            GetPropertyDetailForAgentAdminInput input = null;

            // Act
            var result = await service.GetPropertyDetailForAgentAdmin(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task CreateReport_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            CreateReportInput input = null;

            // Act
            var result = await service.CreateReport(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdateReport_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdateReportInput input = null;

            // Act
            var result = await service.UpdateReport(
                input);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task UpdatePropertyStatus_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            UpdatePropertyStatusInput input = null;

            // Act
            var result = await service.UpdatePropertyStatus(
                input);

            // Assert
            Assert.True(false);
        }
    }
}
