using com.trustmechain.SmartME.Core.Models;
using com.trustmechain.SmartME.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.trustmechain.SmartME.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly SmartMEDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(SmartMEDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {

            //_context.Tasks.AddRange(
            //    new Task("Follow the white rabbit", "Follow the white rabbit in order to know the reality."),
            //    new Task("Clean your room") { State = TaskState.Completed }
            //    );

            //_context.Tenants.Add(
            //    new Core.MultiTenancy.Tenant()
            //    {
            //        TenancyName = "DEFAULT",
            //        Name = "Default"
            //    }
            //    );

            _context.Users.Add(
                new Core.Authorization.Users.User()
                {
                    UserName = "85261586394",
                    TenantId = 1,
                    Name = "testing",
                    Surname = "testing",
                    Password = "AQAAAAEAACcQAAAAEDz/wdLYqVDm63/Z92G7jtT5gevC0jEQGwcg5b34LTyHR2h+9Va0UMC5PWSzZzt3ag==",
                    TelAreaCode = "852",
                    Telephone = "61586394",
                    Roles = new List<Abp.Authorization.Users.UserRole>() { new Abp.Authorization.Users.UserRole() { RoleId = 1, TenantId = null } },
                    IsActive = true,
                    NormalizedUserName = "85261586394"

                }
                );

            _context.Users.Add(
                new Core.Authorization.Users.User()
                {
                    UserName = "85298765432",
                    TenantId = null,
                    Name = "agent2",
                    Password = "AQAAAAEAACcQAAAAEDz/wdLYqVDm63/Z92G7jtT5gevC0jEQGwcg5b34LTyHR2h+9Va0UMC5PWSzZzt3ag==",
                    IsActive = true,
                    Roles = new List<Abp.Authorization.Users.UserRole>() { new Abp.Authorization.Users.UserRole() { RoleId = 1, TenantId = null } },
                    Permissions = null //{ new Abp.Authorization.Users.UserPermissionSetting() { Name = "abc", IsGranted = true}}
                }
                );


            //_context.PropertyMeta.Add(
            //    new PropertyMeta()
            //    {
            //        Id = 1,
            //        DistrictCode = "WC",
            //        BuildingName_Eng = "WC",

            //    }
            //    );

            _context.AgentProfiles.Add(
                new Core.Models.Agent.AgentProfile()
                {
                    AgentCompanyBranch = new Core.Models.Agent.AgentCompanyBranch()
                    {
                        DistrictCode = "CW",
                        Company = new Core.Models.Agent.AgentCompany()
                        {
                            CompanyName_Eng = "testing CW",
                        },
                        AddressRefId = null,
                    },
                    AgentId = 3
                }
                );

            _context.SMS_OTPs.Add(
                new Core.Models.SMS.SMS_OTP()
                {
                    //Id = 1,
                    UserId = 3,
                    OTP = "1111",
                    AreaCode = "852",
                    Mobile = "61107946",
                    ExpiryTime = DateTime.Now.AddMonths(6),

                }
                );

            _context.SMS_OTPs.Add(
                new Core.Models.SMS.SMS_OTP()
                {
                    //Id = 2,
                    UserId = 6,
                    OTP = "2222",
                    AreaCode = "852",
                    Mobile = "61586394",
                    ExpiryTime = DateTime.Now.AddMonths(6),

                }
                );

            //_context.UserPermissions.Add(
            //   new Abp.Authorization.Users.UserPermissionSetting
            //   {
            //       UserId = 1,
            //       TenantId = 1,
            //       IsGranted = true,
            //       Name = "Users",

            //   }
            //    );

            //_context.RolePermissions.Add(
            //    new Abp.Authorization.Roles.RolePermissionSetting
            //    {
            //        Name = "Users",
            //        TenantId = 1,
            //        IsGranted = true,
            //        RoleId = 1,

            //    }
            //    );

            //_context.Properties.Add(
            //    new Core.Models.Property
            //    {
            //        Id = 1,
            //        AddressEng = "testing",
            //        ActualSize = 0,
            //        AddressChT = "",
            //        Age = 0,
            //        Description = "testing",
            //    }
            //    );

            _context.PropertySaleSelectedAgent.Add(
                new PropertySaleSelectedAgent
                {
                    AgentProfile = new Core.Models.Agent.AgentProfile()
                    {
                        IsVerified = true,
                        //Id = 1,
                        AgentCompanyBranch = new Core.Models.Agent.AgentCompanyBranch()
                        {
                            //Id = 1,
                            Company = new Core.Models.Agent.AgentCompany()
                            {
                                CompanyName_Eng = "testing CW",
                                //Id = 1
                            },
                            AddressRefId = 1,
                            DistrictCode = "CW"
                        },
                        //AgentId = 1,
                        User = new Core.Authorization.Users.User()
                        {
                            UserName = "85261107946",
                            TenantId = null,
                            Name = "agent1",
                            Password = "AQAAAAEAACcQAAAAEDz/wdLYqVDm63/Z92G7jtT5gevC0jEQGwcg5b34LTyHR2h+9Va0UMC5PWSzZzt3ag==",
                            IsActive = true,
                            Roles = new List<Abp.Authorization.Users.UserRole>() { new Abp.Authorization.Users.UserRole() { RoleId = 1, TenantId = null } },
                            Permissions = null //{ new Abp.Authorization.Users.UserPermissionSetting() { Name = "abc", IsGranted = true}}
                        },

                    },
                    PropertySaleInfo = new PropertySaleInfo
                    {
                        CommissionAmount = 1000,
                        CommissionRate = 15,
                        CommissionType = "bbb",
                        IsPublic = true,
                        IsVacuum = true,
                        Price = 50000000,
                        Property = new Core.Models.Property
                        {
                            //Id = 1,
                            AddressEng = "testing",
                            ActualSize = 0,
                            AddressChT = "",
                            Age = 0,
                            Description = "testing",
                            MetaRef = new PropertyMeta
                            {
                                DistrictCode = "CW",
                                BlockDescriptor_Eng = "testing"
                            },
                            OwnerId = 6,
                            IsVerified = true

                        },
                        Remarks = "remarks",
                        VacuumDate = DateTime.Now,
                        PublishStartTime = DateTime.Now,
                        PublishEndTime = DateTime.Now,

                    }
                }
                );

            _context.PropertyImages.Add(
                new Core.Models.PropertyImage
                {
                    PropertyId = 1,
                    FileName = "testing file",
                    Order = 1
                }
                );

            _context.Roles.Add(
                new Core.Authorization.Roles.Role
                {
                    //Id = 3,
                    Description = "test",
                    Name = "role1",
                    DisplayName = "role1d"
                }
                );

            _context.RolePermissions.Add(
                new Abp.Authorization.Roles.RolePermissionSetting
                {
                    RoleId = 3,
                    Name = "role3",
                }
                );

            _context.UserRoles.Add(
                new Abp.Authorization.Users.UserRole
                {
                    RoleId = 3,
                    UserId = 2,

                });

            _context.Roles.Add(
               new Core.Authorization.Roles.Role
               {
                   Id = 99,
                   Description = "test99",
                   Name = "role99",
                   DisplayName = "role99d"
               }
               );

            _context.RolePermissions.Add(
                new Abp.Authorization.Roles.RolePermissionSetting
                {
                    RoleId = 99,
                    Name = "roleps99",
                }
                );

            _context.UserRoles.Add(
                new Abp.Authorization.Users.UserRole
                {
                    RoleId = 99,
                    UserId = 2,

                });

            _context.PropertyLeaseSelectedAgent.Add(
                new PropertyLeaseSelectedAgent
                {
                    AgentProfileId = 6,
                    ContractRefId = null,
                    PropertyLeaseInfo = new PropertyLeaseInfo
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
                        LeaseMinMonth = 3,
                        Property = new Core.Models.Property
                        {

                        }
                    },
                    Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Confirm
                }
                );

            //_context.PropertyLeaseSelectedAgent.Add(
            //    new PropertyLeaseSelectedAgent
            //    {
            //        AgentProfileId = 6,
            //        ContractRefId = null,
            //        PropertyLeaseInfo = new PropertyLeaseInfo
            //        {
            //            CommissionAmount = 46666,
            //            CommissionRate = 15,
            //            CommissionType = "aaa",
            //            IsPublic = true,
            //            IsVacuum = true,
            //            MonthlyRent = 100000,
            //            PropertyId = 1,
            //            PublishEndTime = DateTime.Now,
            //            PublishStartTime = DateTime.Now,
            //            Remarks = "remark",
            //            VacuumDate = DateTime.Now,
            //            DepositMonth = 10000,
            //            DepositRate = 10,
            //            DepositType = "M",
            //            IsIncludeFurniture = true,
            //            IsPublicAfterComplete = true,
            //            LeaseMinMonth = 3,
            //            Property = new Core.Models.Property
            //            {

            //            }
            //        },
            //        Status = PropertyLeaseSelectedAgent.PropertyAppointmentStatus.Reject
            //    }
            //    );

            _context.AgentPreferredSubDistrict.Add(
                new Core.Models.Agent.AgentPreferredSubDistrict
                {
                    AgentProfileId = 6,
                    PreferredSubDistrict = "CW",
                }
                );


            _context.AgentPreferredCategory.Add(
                new Core.Models.Agent.AgentPreferredCategory
                {
                    AgentProfileId = 6,
                    PreferredCategory = "PreferredCategory1"
                }
                );

            _context.AgentPreferredSubDistrict.Add(
                new Core.Models.Agent.AgentPreferredSubDistrict
                {
                    AgentProfileId = 6,
                    PreferredSubDistrict = "PreferredSubDistrict1"
                }
                );

            _context.Notification_Sendout_Log.Add(
                new Core.Models.Notification.Notification_Sendout_Log
                {
                    UserId = 6,
                    Content = "content",
                    Feature = MetaEnum.notification_feature.BO,
                    IsDeleted = false,
                    IsSend = true,
                    PropertyId = 1,
                    Status = Core.Models.Notification.Notification_Sendout_Log.NotificationStatus.Unread,
                    RegistrationId = "123",
                    Platform = Core.Models.Notification.Notification_Sendout_Log.NotificationPlatform.APortal
                }
                );

            _context.Tenants.Add(
                new Core.MultiTenancy.Tenant
                {
                    ConnectionString = "ConnectionString",
                    IsActive = true,
                    IsDeleted = false,
                    Name = "Name1",
                    TenancyName = "TenancyName1",
                    Edition = new Abp.Application.Editions.Edition
                    {
                        Name = "Edition1",
                        DisplayName = "Edition1d",
                        IsDeleted = false,

                    },
                }
                );

            _context.PropertyAppointmentSchedule.Add(
                new Core.Models.PropertyAppointmentSchedule
                {
                    PropertyAppointment = new Core.Models.PropertyAppointment
                    {
                        AgentProfileId = 6,
                        BuyerMobile = "61586394",
                        Category = Core.Models.PropertyAppointment.PropertyAppointmentCategory.Lease,
                        Name = "PAname",
                        Price = 50000,
                        PropertyId = 1,
                        Remark = "PAremarks",
                        Surname = "PAsurname",
                        TelAreaCode = "852",
                        Title = "PAtitle",

                    }
                }
                );



            _context.BuyerRecommend.Add(
                new Core.Models.Buyer.BuyerRecommend
                {
                    AgentId = 6,
                    PropertyId = 1,
                    PropertySaleInfoId = 1,
                    PropertyLeaseInfoId = null,
                    CustomerProfileId = 1
                }
                );

            _context.CustomerProfiles.Add(
                new Core.Models.Customer.CustomerProfile
                {
                    CustomerId = 6,
                }
                );


            _context.SaveChanges();
        }
    }
}
