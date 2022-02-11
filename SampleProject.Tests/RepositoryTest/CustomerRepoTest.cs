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

	public class CustomerRepoTest: CommonTests
	{
		ICustomerRepository repository;
		Customer Input;

		public CustomerRepoTest() : base()
		{

		}


		[SetUp]
		public async Task Setup()
		{
			Initialize();
			await Clean();
			repository = new CustomerRepository(DataContext);
			//Business Group
			DataContext.Status.Add(new StatusDAO
			{
				//Id = 1,
				Code = "ACTIVE",
				Name = "Hoạt động",
			});
			DataContext.Status.Add(new StatusDAO
			{
				//Id = 2,
				Code = "INACTIVE",
				Name = "Dừng hoạt động",
			});
			DataContext.SaveChanges();

			Input = new Customer
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
			};
		}
		
		// Create
		[Test]
		public async Task Customer_Create_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);

			// Assert
			var Output = DataContext.Customer.Find(Input.Id);
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.Address, Output.Address);
			Assert.AreEqual(Input.Phone, Output.Phone);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.Used, Output.Used);
			Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}

		// Update
		[Test]
		public async Task Customer_Update_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);

			// Update
			var UpdateData = DataContext.Customer.Find(Input.Id); ;
			Input = new Customer
			{
				Id = UpdateData.Id,
				Code = "2018603517",
				Name = "Vũ Văn Kiên",
				Address = "Nga Sơn, Thanh Hóa",
				Phone = "0866910517",
				StatusId = 2,
				CreatedAt = UpdateData.CreatedAt,
				UpdatedAt = DateTime.Now,
				Used = UpdateData.Used
			};
			await repository.Update(Input);
			// Assert
			var Output = DataContext.Customer.Find(Input.Id);
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.Address, Output.Address);
			Assert.AreEqual(Input.Phone, Output.Phone);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.Used, Output.Used);
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}

		// Delete
		[Test]
		public async Task Customer_Delete_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);

			// Delete
			await repository.Delete(Input);
			Initialize();
			// Assert
			var Output = DataContext.Customer.Find(Input.Id);
			Assert.IsNotNull(Output.DeletedAt);
		}
		//List Order By Name + Skip and Take
		[Test]
		public async Task Customer_GetListByName_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);
			await repository.Create(Input);

			string Name = "Tạ Văn Toàn";
			CustomerFilter CustomerFilter = new CustomerFilter
			{
				Skip = 0,
				Take = 10,
				Name = new StringFilter { Equal = Name },
				Selects = CustomerSelect.Name
			};

			int count = await repository.Count(CustomerFilter);

			// Assert
			Assert.AreEqual(2, count);
		}

		// Bulk Insert
		[Test]
		public async Task Customer_BulkInsert_ReturnTrue()
		{
			List<Customer> Customers = new List<Customer>();
			Customers.Add(Input);
			Customers.Add(Input);
			Customers.Add(Input);
			await repository.BulkInsert(Customers);

			int count = DataContext.Customer.Select(x => x).Count();

			// Assert
			Assert.AreEqual(3, count);
		}
	}
}
