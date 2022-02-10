namespace SampleProject.Rpc.Customer
{
    public class CustomerRoute : Root
    {
        public const string Parent = Module + "/customer";
        public const string Master = Module + "/customer/customer/customer-master";
        public const string Detail = Module + "/customer/customer/customer-detail/*";
        private const string Default = Rpc + Module + "/customer";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string BulkDelete = Default + "/bulk-delete";
    }
}
