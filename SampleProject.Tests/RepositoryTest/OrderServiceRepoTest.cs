using SampleProject.Entities;
using SampleProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SampleProject.Models;
using NUnit.Framework;
using LightBDD.NUnit3;
using LightBDD.Framework;
using TrueSight.Common;

namespace SampleProject.Tests.RepositoryTest
{
	[TestFixture]

	public class OrderServiceRepoTest : CommonTests
	{
		IOrderServiceRepository repository;
		OrderService Input;

		public OrderServiceRepoTest() : base()
		{

		}


		[SetUp]
		public async Task Setup()
		{
			await Clean();
			repository = new OrderServiceRepository(DataContext);
			#region Setup Status + Customer
			// Setup Status
			DataContext.Status.Add(new StatusDAO
			{
				Code = "ACTIVE",
				Name = "Hoạt động",
			});
			DataContext.Status.Add(new StatusDAO
			{
				Code = "INACTIVE",
				Name = "Dừng hoạt động",
			});
			DataContext.SaveChanges();
			//Setup Customer
			DataContext.Customer.Add(new CustomerDAO
			{
				Code = "2018603202",
				Name = "Tạ Văn Toàn",
				Address = "Yên Mô, Ninh Bình",
				Phone = "0866910323",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			});
			DataContext.Customer.Add(new CustomerDAO
			{
				Code = "2018605135",
				Name = "Phạm Văn Thắng",
				Address = "Kim Sơn, Ninh Bình",
				Phone = "0866910323",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			});

			#endregion
			DataContext.SaveChanges();

			Input = new OrderService
			{
				Code = "Trang phuc",
				OrderDate = DateTime.Now,
				CustomerId = 1,
				Total = 1000,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			};
		}

		// Create
		//[Test]
		public async Task OrderService_Create_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);
			
			// Assert
			var Output = DataContext.OrderService.Where(x => x.Id == 1).FirstOrDefault();
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.OrderDate, Output.OrderDate);
			Assert.AreEqual(Input.CustomerId, Output.CustomerId);
			Assert.AreEqual(Input.Total, Output.Total);
			Assert.AreEqual(Input.Used, Output.Used);
			Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}

        // Update
        //[Test]
        public async Task OrderService_Update_ReturnTrue()
        {
            // Create Instance
            await repository.Create(Input);

            // Update
            var UpdateData = DataContext.OrderService.SingleOrDefault(x => x.Code == Input.Code);
            Input = new OrderService
            {
                Id = UpdateData.Id,
				Code = "Quan ao",
				OrderDate = DateTime.Now,
				CustomerId = 1,
				Total = 900,
				CreatedAt = UpdateData.CreatedAt,
				UpdatedAt = DateTime.Now,
				Used = false
			};
            await repository.Update(Input);
			// Assert
			var Output = DataContext.OrderService.Where(x => x.Id == 1).FirstOrDefault();
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.OrderDate, Output.OrderDate);
			Assert.AreEqual(Input.CustomerId, Output.CustomerId);
			Assert.AreEqual(Input.Total, Output.Total);
			Assert.AreEqual(Input.Used, Output.Used);
			Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}

        // Delete
        //[Test]
        public async Task OrderService_Delete_ReturnTrue()
        {
            // Create Instance
            await repository.Create(Input);

            // Delete
            await repository.Delete(Input);

            // Assert
            Assert.IsNotNull(Input.DeletedAt);
        }

        //List Order By Name + Skip and Take
        //[Test]
        public async Task OrderService_GetListByName_ReturnTrue()
        {
            // Create Instance
            await repository.Create(Input);
            await repository.Create(Input);

            string Code = "Trang phuc";
            OrderServiceFilter OrderServiceFilter = new OrderServiceFilter
			{
                Skip = 0,
                Take = 10,
                Code = new StringFilter { Equal = Code },
                Selects = OrderServiceSelect.Code
            };

            int count = await repository.Count(OrderServiceFilter);

            // Assert
            Assert.AreEqual(2, count);
        }

        // Bulk Insert
        //[Test]
        public async Task OrderService_BulkDelete_ReturnTrue()
        {
            List<OrderService> OrderService = new List<OrderService>();
			OrderService.Add(Input);
			OrderService.Add(Input);
			OrderService.Add(Input);
            await repository.BulkDelete(OrderService);

			Assert.IsNotNull(OrderService);
		}
    }
}
