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
		Service Input;
		public ServiceRepoTest(): base() {
		}

		[SetUp]
		public async Task Setup()
		{
			Initialize();
			await Clean();
			repository = new ServiceRepository(DataContext);

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
		[Test]
		public async Task Service_Create_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);

			// Assert
			var Output = DataContext.Service.SingleOrDefault(x=>x.Id == Input.Id);
			Assert.IsNotNull(Output);
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.Price, Output.Price);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.UnitOfMeasureId, Output.UnitOfMeasureId);
			Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}

		// Update 
		[Test]
		public async Task Service_Update_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);

			// Update
			var UpdateData = DataContext.Service.SingleOrDefault(x => x.Id == Input.Id);
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
			await repository.Update(Input);

			// Assert
			var Output = DataContext.Service.SingleOrDefault(x => x.Id == Input.Id);
			Assert.IsNotNull(Output);
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.Price, Output.Price);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.UnitOfMeasureId, Output.UnitOfMeasureId);
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}

		// Delete
		[Test]
		public async Task Service_Delete_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);

			// Delete
			await repository.Delete(Input);
			Initialize();
			// Assert
			var Output = DataContext.Service.Find(Input.Id);
			Assert.IsNotNull(Output.DeletedAt);
		}
	}
}
