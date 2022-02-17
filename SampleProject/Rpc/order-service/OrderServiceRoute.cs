using System.ComponentModel;

namespace SampleProject.Rpc.order_service
{
	[DisplayName("Đặt hàng")]
	public class OrderServiceRoute : Root
	{
		public const string Parent = Module + "/order-service";
		public const string Master = Module + "/order-service/order-service-master";
		public const string Detail = Module + "/order-service/order-service-detail/*";
		private const string Default = Rpc + Module + "/order-service";
		public const string Count = Default + "/count";
		public const string List = Default + "/list";
		public const string Get = Default + "/get";
		public const string Create = Default + "/create";
		public const string Update = Default + "/update";
		public const string Delete = Default + "/delete";
	}
}
