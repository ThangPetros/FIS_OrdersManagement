//using TrueSight.Common;
//using SampleProject.Common;
//using SampleProject.Entities;
//using SampleProject.Services.MCustomer;
//using SampleProject.Services.MStatus;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SampleProject.Rpc.Customer
//{
//    public class CustomerController : RpcController
//    {
//        private IStatusService StatusService;
//        private ICustomerService CustomerService;
//        private ICurrentContext CurrentContext;
//        public CustomerController(
//            IStatusService StatusService,
//            ICustomerService CustomerService,
//            ICurrentContext CurrentContext)
//        {
//            this.StatusService = StatusService;
//            this.CustomerService = CustomerService;
//            this.CurrentContext = CurrentContext;
//        }
//        [Route(CustomerRoute.Count), HttpPost]
//        public async Task<ActionResult<int>> Count([FromBody] Customer_CustomerFilterDTO Customer_CustomerFilterDTO)
//        {
//            if(!ModelState.IsValid)
//                throw new BindException(ModelState);
//        }
//    }
//}
