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
		#region GenericEnum Sync
            public static GenericEnum CustomerSync = new GenericEnum { Id = 1, Code = "Customer.Sync", Name = "Đồng bộ Customer" };
            public static GenericEnum ServiceSync = new GenericEnum { Id = 2, Code = "Service.Sync", Name = "Đồng bộ Service" };
            public static GenericEnum OrderServiceSync = new GenericEnum { Id = 3, Code = "OrderService.Sync", Name = "Đồng bộ OrderService" };
            public static GenericEnum OrderServiceContentSync = new GenericEnum { Id = 4, Code = "OrderServiceContent.Sync", Name = "Đồng bộ OrderServiceContent" };
            public static GenericEnum StatusSync = new GenericEnum { Id = 5, Code = "Status.Sync", Name = "Đồng bộ Status" };
            public static GenericEnum UnitOfMeasureSync = new GenericEnum { Id = 6, Code = "UnitOfMeasure.Sync", Name = "Đồng bộ UnitOfMeasure" };
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
