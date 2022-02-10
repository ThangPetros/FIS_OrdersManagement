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

	public class OrderServiceContentRepoTest : CommonTests
	{
		IOrderServiceContentRepository repository;
		IUOW uow;
		OrderServiceContent Input;

		public OrderServiceContentRepoTest() : base()
		{

		}


		[SetUp]
		public async Task Setup()
		{
			await Clean();
            repository = new OrderServiceContentRepository(DataContext);
			uow = new UOW(DataContext);

			#region Setup UnitOfMeasure + Service + OrderService
			// Setup UnitOfMeasure
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
			DataContext.UnitOfMeasure.Add(new UnitOfMeasureDAO
			{
				Code = "CHIEC",
				Name = "Chiếc",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			});
			DataContext.UnitOfMeasure.Add(new UnitOfMeasureDAO
			{
				Code = "BO",
				Name = "Bộ",
				StatusId = 2,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			});
			DataContext.SaveChanges();
			// Setup Service
			DataContext.Service.Add(new ServiceDAO
			{
				Code = "Jean",
				Name = "Quần Jean",
				UnitOfMeasureId = 1,
				Price = 100000,
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			});
			//Setup Customer
			DataContext.Customer.Add(new CustomerDAO
			{
				//Id = 1,
				Code = "2018603202",
				Name = "Tạ Văn Toàn",
				Address = "Yên Mô, Ninh Bình",
				Phone = "0866910323",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			});
			DataContext.SaveChanges();
			// Setup OrderService
			DataContext.OrderService.Add(new OrderServiceDAO
			{
				Code = "Trang phuc",
				OrderDate = DateTime.Now,
				CustomerId = 1,
				Total = 1000,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			});

			#endregion
			DataContext.SaveChanges();

			// Create an Instance Service
			Input = new OrderServiceContent
			{
				ServiceId = 1,
				OrderServiceId = 1,
				PrimaryUnitOfMeasureId=1,
				UnitOfMeasureId = 1,
				Quantity = 100,
				RequestQuantity = 100,
				Price = 200,
				Amount = 200,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};
		}

		// Create
		//[Test]
		public async Task OrderServiceContent_Create_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);

			// Assert
			var Output = DataContext.OrderServiceContent.Where(x => x.Id == 1).FirstOrDefault();
			Assert.AreEqual(Input.ServiceId, Output.ServiceId);
			Assert.AreEqual(Input.OrderServiceId, Output.OrderServiceId);
			Assert.AreEqual(Input.PrimaryUnitOfMeasureId, Output.PrimaryUnitOfMeasureId);
			Assert.AreEqual(Input.UnitOfMeasureId, Output.UnitOfMeasureId);
			Assert.AreEqual(Input.Quantity, Output.Quantity);
			Assert.AreEqual(Input.RequestQuantity, Output.RequestQuantity);
			Assert.AreEqual(Input.Price, Output.Price);
			Assert.AreEqual(Input.Amount, Output.Amount);
			Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}

        // Update
        [Test]
        public async Task OrderServiceContent_Update_ReturnTrue()
        {
            // Create Instance
            await repository.Create(Input);

            // Update
            var UpdateData = DataContext.OrderServiceContent.SingleOrDefault(x => x.Id == 1);
            Input = new OrderServiceContent
			{
				Id = UpdateData.Id,
				ServiceId = UpdateData.ServiceId,
				OrderServiceId = 1,
				PrimaryUnitOfMeasureId=1,
				UnitOfMeasureId = 1,
				Quantity = 150,
				RequestQuantity = 100,
				Price = 200,
				Amount = 200,
				UpdatedAt = UpdateData.CreatedAt
			};
            await repository.Update(Input);
			// Assert
			var Output = DataContext.OrderServiceContent.Where(x => x.Id == 1).FirstOrDefault();
            Assert.AreEqual(Input.ServiceId, Output.ServiceId);
            Assert.AreEqual(Input.OrderServiceId, Output.OrderServiceId);
			Assert.AreEqual(Input.PrimaryUnitOfMeasureId, Output.PrimaryUnitOfMeasureId);
			Assert.AreEqual(Input.UnitOfMeasureId, Output.UnitOfMeasureId);
			Assert.AreEqual(Input.Quantity, Output.Quantity);
			Assert.AreEqual(Input.RequestQuantity, Output.RequestQuantity);
			Assert.AreEqual(Input.Price, Output.Price);
			Assert.AreEqual(Input.Amount, Output.Amount);
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}
    }
}
