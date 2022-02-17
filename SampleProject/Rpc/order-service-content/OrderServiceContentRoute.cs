using System.ComponentModel;

namespace SampleProject.Rpc.order_service_content
{
	[DisplayName("Chi tiết đơn hàng")]
	public class OrderServiceContentRoute: Root
	{
		public const string Parent = Module + "/order-service-content";
		public const string Master = Module + "/order-service-content/order-service-content-master";
		public const string Detail = Module + "/order-service-content/order-service-content-detail/*";
		private const string Default = Rpc + Module + "/order-service-content";
		public const string Count = Default + "/count";
		public const string List = Default + "/list";
		public const string Get = Default + "/get";
		public const string Create = Default + "/create";
		public const string Update = Default + "/update";
	}
}
