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
	public class ServiceRepoTest: CommonTests
	{
		IServiceRepository repository;
		IUOW uow;
		Service Input;
		public ServiceRepoTest(): base() {
		}

		[SetUp]
		public async Task Setup()
		{
			await Clean();
			repository = new ServiceRepository(DataContext);
			uow = new UOW(DataContext);

			#region Setup Status + UnitOfMeasure
			// Setup Status
			DataContext.Status.Add(new StatusDAO
			{
				Code = "ACTIVE",
				Name = "Đang hoạt động"
			});
			DataContext.Status.Add(new StatusDAO
			{
				Code = "INACTIVE",
				Name = "Không hoạt động"
			});

			// Setup UnitOfMeasure
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
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			});
			#endregion
			DataContext.SaveChanges();

			// Create an Instance Service
			Input = new Service
			{
				Code = "Jean",
				Name = "Quần Jean",
				UnitOfMeasureId =1,
				Price = 100000,
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			};
		}

		// Create 
		//[Test]
		public async Task Service_Create_ReturnTrue()
		{
			// Create Instance
			await uow.ServiceRepository.Create(Input);

			// Assert
			var Output = DataContext.Service.SingleOrDefault(x=>x.Id == 1);
			Assert.IsNotNull(Output);
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.Price, Output.Price);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.UnitOfMeasureId, Output.UnitOfMeasureId);
			Assert.AreEqual(Input.CreatedAt, Output.CreatedAt);
			Assert.AreEqual(Input.UpdatedAt, Output.UpdatedAt);
		}

		// Update 
		//[Test]
		public async Task Service_Update_ReturnTrue()
		{
			// Create Instance
			await uow.ServiceRepository.Create(Input);

			// Update
			var UpdateData = DataContext.Service.SingleOrDefault(x => x.Id == 1);
			Input = new Service
			{
				Id = UpdateData.Id,
				Code = "Canvas",
				Name = "Ao Canvas",
				UnitOfMeasureId = 1,
				Price = 500000,
				StatusId = 1,
				CreatedAt = UpdateData.CreatedAt,
				UpdatedAt = DateTime.Now,
				Used = false
			};
			await uow.ServiceRepository.Update(Input);

			// Assert
			var Output = DataContext.Service.SingleOrDefault(x => x.Id == 1);
			Assert.IsNotNull(Output);
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.Price, Output.Price);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.UnitOfMeasureId, Output.UnitOfMeasureId);
			Assert.AreEqual(Input.CreatedAt, Output.CreatedAt);
			Assert.AreEqual(Input.UpdatedAt, Output.UpdatedAt);
		}

		// Delete
		[Test]
		public async Task Service_Delete_ReturnTrue()
		{
			// Create Instance
			await uow.ServiceRepository.Create(Input);

			// Delete
			await uow.ServiceRepository.Delete(Input);

			// Assert
			Assert.IsNotNull(Input.DeletedAt);
		}
	}
}
