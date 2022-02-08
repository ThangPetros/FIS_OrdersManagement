using TrueSight.Common;
using SampleProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleProject.Enums
{
	public class RoutingKeyEnum
	{
		#region Nhap
		public static GenericEnum AppUserSync = new GenericEnum { Id = 1, Code = "AppUser.Sync", Name = "Đồng bộ AppUser" };
            public static GenericEnum CustomerSync = new GenericEnum { Id = 2, Code = "Customer.Sync", Name = "Đồng bộ Customer" };
            public static GenericEnum CategorySync = new GenericEnum { Id = 3, Code = "Category.Sync", Name = "Đồng bộ Category" };
            public static GenericEnum DistrictSync = new GenericEnum { Id = 4, Code = "District.Sync", Name = "Đồng bộ District" };
            public static GenericEnum OrganizationSync = new GenericEnum { Id = 5, Code = "Organization.Sync", Name = "Đồng bộ Organization" };
            public static GenericEnum PositionSync = new GenericEnum { Id = 6, Code = "Position.Sync", Name = "Đồng bộ Position" };
            public static GenericEnum ProductSync = new GenericEnum { Id = 7, Code = "Product.Sync", Name = "Đồng bộ Product" };
            public static GenericEnum ProductTypeSync = new GenericEnum { Id = 8, Code = "ProductType.Sync", Name = "Đồng bộ ProductType" };
            public static GenericEnum ProductGroupingSync = new GenericEnum { Id = 9, Code = "ProductGrouping.Sync", Name = "Đồng bộ  ProductGrouping" };
            public static GenericEnum ProvinceSync = new GenericEnum { Id = 10, Code = "Province.Sync", Name = "Đồng bộ Province" };
            public static GenericEnum StoreSync = new GenericEnum { Id = 12, Code = "Store.Sync", Name = "Đồng bộ Store" };
            public static GenericEnum StoreGroupingSync = new GenericEnum { Id = 13, Code = "StoreGrouping.Sync", Name = "Đồng bộ StoreGrouping" };
            public static GenericEnum StoreTypeSync = new GenericEnum { Id = 14, Code = "StoreType.Sync", Name = "Đồng bộ StoreType" };
            public static GenericEnum SupplierSync = new GenericEnum { Id = 15, Code = "Supplier.Sync", Name = "Đồng bộ Supplier" };
            public static GenericEnum TaxTypeSync = new GenericEnum { Id = 16, Code = "TaxType.Sync", Name = "Đồng bộ TaxType" };
            public static GenericEnum UnitOfMeasureSync = new GenericEnum { Id = 17, Code = "UnitOfMeasure.Sync", Name = "Đồng bộ UnitOfMeasure" };
            public static GenericEnum UnitOfMeasureGroupingSync = new GenericEnum { Id = 18, Code = "UnitOfMeasureGrouping.Sync", Name = "Đồng bộ UnitOfMeasureGrouping" };
            public static GenericEnum WardSync = new GenericEnum { Id = 19, Code = "Ward.Sync", Name = "Đồng bộ Ward" };
            public static GenericEnum ImageSync = new GenericEnum { Id = 20, Code = "Image.Sync", Name = "Đồng bộ Image" };
            public static GenericEnum SiteSync = new GenericEnum { Id = 21, Code = "Site.Sync", Name = "Đồng bộ Site" };
            public static GenericEnum PasswordSync = new GenericEnum { Id = 22, Code = "AppUserPassword.Sync", Name = "Đồng bộ Password" };
            public static GenericEnum CodeGeneratorRuleSync = new GenericEnum { Id = 23, Code = "CodeGeneratorRule.Sync", Name = "Đồng bộ CodeGeneratorRule" };
            public static GenericEnum NationSync = new GenericEnum { Id = 24, Code = "Nation.Sync", Name = "Đồng bộ Nation" };
            public static GenericEnum CurrencySync = new GenericEnum { Id = 25, Code = "Currency.Sync", Name = "Đồng bộ Currency" };
            public static GenericEnum ExchangeRateSync = new GenericEnum { Id = 26, Code = "ExchangeRate.Sync", Name = "Đồng bộ ExchangeRate" };
            public static GenericEnum SupplierUserSync = new GenericEnum { Id = 1, Code = "SupplierUser.Sync", Name = "Đồng bộ SupplierUser" };
            public static GenericEnum ItemSync = new GenericEnum { Id = 30, Code = "Item.Sync", Name = "Đồng bộ Item" };


            public static GenericEnum ColorSync = new GenericEnum { Id = 101, Code = "Color.Sync", Name = "Đồng bộ Color" };
            public static GenericEnum SexSync = new GenericEnum { Id = 102, Code = "Sex.Sync", Name = "Đồng bộ Sex" };
            public static GenericEnum StatusSync = new GenericEnum { Id = 103, Code = "Status.Sync", Name = "Đồng bộ Status" };
            public static GenericEnum StoreStatusSync = new GenericEnum { Id = 104, Code = "StoreStatus.Sync", Name = "Đồng bộ StoreStatus" };
            public static GenericEnum UsedVariationSync = new GenericEnum { Id = 105, Code = "UsedVariation.Sync", Name = "Đồng bộ UsedVariation" };
            public static GenericEnum EntityComponentSync = new GenericEnum { Id = 106, Code = "EntityComponent.Sync", Name = "Đồng bộ EntityComponent" };
            public static GenericEnum EntityTypeSync = new GenericEnum { Id = 107, Code = "EntityType.Sync", Name = "Đồng bộ EntityType" };

            public static GenericEnum AdministrativeUnitImport = new GenericEnum { Id = 200, Code = "AdministrativeUnit.Import", Name = "Import AdministrativeUnit" };
            public static GenericEnum MailSend = new GenericEnum { Id = 296, Code = "Mail.Send", Name = "Gửi Mail" };
            public static GenericEnum UserNotificationSend = new GenericEnum { Id = 297, Code = "UserNotification.Send", Name = "Gửi thông báo" };
		#endregion
		public static GenericEnum AuditLogSend = new GenericEnum { Id = 298, Code = "AuditLog.Send", Name = "Audit Log" };
            public static GenericEnum SystemLogSend = new GenericEnum { Id = 299, Code = "SystemLog.Send", Name = "System Log" };

            #region internal
            /*public static GenericEnum AppUserUsed = new GenericEnum { Id = 1001, Code = "AppUser.Used", Name = "AppUser Used" };
            public static GenericEnum BrandUsed = new GenericEnum { Id = 1002, Code = "Brand.Used", Name = "Brand Used" };
            public static GenericEnum CategoryUsed = new GenericEnum { Id = 1003, Code = "Category.Used", Name = "Category Used" };
            public static GenericEnum DistrictUsed = new GenericEnum { Id = 1004, Code = "District.Used", Name = "District Used" };
            public static GenericEnum GeneralVariationUsed = new GenericEnum { Id = 1005, Code = "GeneralVariation.Used", Name = "GeneralVariation Used" };
            public static GenericEnum ItemUsed = new GenericEnum { Id = 1006, Code = "Item.Used", Name = "Item Used" };
            public static GenericEnum OrganizationUsed = new GenericEnum { Id = 1007, Code = "Organization.Used", Name = "Organization Used" };
            public static GenericEnum ProductUsed = new GenericEnum { Id = 1009, Code = "Product.Used", Name = "Product Used" };
            public static GenericEnum ProductTypeUsed = new GenericEnum { Id = 1010, Code = "ProductType.Used", Name = "ProductType Used" };
            public static GenericEnum ProvinceUsed = new GenericEnum { Id = 1011, Code = "Province.Used", Name = "Province Used" };
            public static GenericEnum RoleUsed = new GenericEnum { Id = 1012, Code = "MDM.Role.Used", Name = "Role Used" };
            public static GenericEnum SupplierUsed = new GenericEnum { Id = 1016, Code = "Supplier.Used", Name = "Supplier Used" };
            public static GenericEnum TaxTypeUsed = new GenericEnum { Id = 1017, Code = "TaxType.Used", Name = "TaxType Used" };
            public static GenericEnum UnitOfMeasureUsed = new GenericEnum { Id = 1018, Code = "UnitOfMeasure.Used", Name = "UnitOfMeasure Used" };
            public static GenericEnum UnitOfMeasureGroupingUsed = new GenericEnum { Id = 1019, Code = "UnitOfMeasureGrouping.Used", Name = "UnitOfMeasureGrouping Used" };
            public static GenericEnum WardUsed = new GenericEnum { Id = 1020, Code = "Ward.Used", Name = "Ward Used" };
            public static GenericEnum WorkflowDefinitionUsed = new GenericEnum { Id = 1021, Code = "MDM.WorkflowDefinition.Used", Name = "WorkflowDefinition Used" };
            public static GenericEnum NationUsed = new GenericEnum { Id = 1023, Code = "Nation.Used", Name = "Nation Used" };
            public static GenericEnum CodeGeneratorRuleUsed = new GenericEnum { Id = 138, Code = "CodeGeneratorRule.Used", Name = "CodeGeneratorRule Used" };
            public static GenericEnum CurrencyUsed = new GenericEnum { Id = 139, Code = "Currency.Used", Name = "Currency Used" };*/
            #endregion
      }
}
