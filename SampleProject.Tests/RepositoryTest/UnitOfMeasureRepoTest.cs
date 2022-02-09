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

namespace SampleProject.Tests
{
	//[TestFixture]
	public class UnitOfMeasureRepoTest : CommonTests
	{
		IUnitOfMeasureRepository repository;
		IUOW uow;
		public UnitOfMeasureRepoTest() : base()
		{

		}
		[SetUp]
		public async Task Setup()
		{
			await Clean();
			//repository = new UnitOfMeasureRepository(DataContext);
			uow = new UOW(DataContext);
			//Business Group
			DataContext.Status.Add(new StatusDAO
			{
				Id = 1,
				Code = "ACTIVE",
				Name = "Hoạt động",
			});
			DataContext.Status.Add(new StatusDAO
			{
				Id = 2,
				Code = "INACTIVE",
				Name = "Dừng hoạt động",
			});
			DataContext.SaveChanges();
		}

		//Create
		//[Test]
		public async Task UnitOfMeasure_Create_ReturnTrue()
		{
			UnitOfMeasure Input = new UnitOfMeasure
			{
				Id = 1,
				Code = "CHIEC",
				Name = "Chiếc",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			};
			await repository.Create(Input);

			var Output = DataContext.UnitOfMeasure.Where(x => x.Id == Input.Id).FirstOrDefault();
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.Used, Output.Used);
			Assert.AreEqual(Input.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}
		//Update
		//[Test]
		public async Task UnitOfMeasure_Update_ReturnTrue()
		{
			// Create Instance
			UnitOfMeasure Input = new UnitOfMeasure
			{
				Id = 1,
				Code = "CHIEC",
				Name = "Chiếc",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			};
			await repository.Create(Input);

			// Update
			Input.Code = "THUNG";
			Input.Name = "Thùng";
			Input.StatusId = 2;
			Input.UpdatedAt = DateTime.Now;
			Input.Used = true;
			
			await repository.Update(Input);

			var Output = DataContext.UnitOfMeasure.Find(Input.Id);
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.Used, Output.Used);
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}
		//Delete
		//[Test]
		public async Task UnitOfMeasure_Delete_ReturnTrue()
		{
			// Create Instance
			UnitOfMeasure Input = new UnitOfMeasure
			{
				Id = 1,
				Code = "CHIEC",
				Name = "Chiếc",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			};
			await uow.UnitOfMeasureRepository.Create(Input);

			// Delete
			//Input.DeletedAt = DateTime.Now;
			//await repository.Update(Input);

			await uow.UnitOfMeasureRepository.Delete(Input);

			Assert.IsNotNull(Input.DeletedAt);
			//var Output = DataContext.UnitOfMeasure.Find(Input.Id);
			//Assert.AreNotEqual(Input.DeletedAt, Output.DeletedAt);    // Assert.IsNotNull(Output.DeletedAt);
		}
		//List Order By Name + Skip and Take
		[Test]
		public async Task UnitOfMeasure_GetListByName_ReturnTrue()
		{
			// Create Instance
			UnitOfMeasure Input = new UnitOfMeasure
			{
				//Id = 1,
				Code = "THUNG",
				Name = "Thùng",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			};
			await uow.UnitOfMeasureRepository.Create(Input);
			await uow.UnitOfMeasureRepository.Create(Input);

			string Name = "Thùng";
			// Get List
			UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
			{
				Skip = 0,
				Take = 10,
				Name = new StringFilter { Equal = Name},
				Selects = UnitOfMeasureSelect.Name
			};
			int count = await uow.UnitOfMeasureRepository.Count(UnitOfMeasureFilter);

			Assert.AreEqual(2, count);
		}
		//List Order By Type + Skip and Take

		//Bulk Insert 
	}
}
