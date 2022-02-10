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
	[TestFixture]
	public class UnitOfMeasureRepoTest : CommonTests
	{
		IUnitOfMeasureRepository repository;
		IUOW uow;
		UnitOfMeasure Input;

		public UnitOfMeasureRepoTest() : base()
		{
		}
		

		[SetUp]
		public async Task Setup()
		{
			await Clean();
			repository = new UnitOfMeasureRepository(DataContext);
			uow = new UOW(DataContext);
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

			Input = new UnitOfMeasure
			{
				//Id = 1,
				Code = "CHIEC",
				Name = "Chiếc",
				StatusId = 1,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Used = false
			};
		}

		//Create
		//[Test]
		public async Task UnitOfMeasure_Create_ReturnTrue()
		{
			DataContext.SaveChanges();
			// Create Instance
			await uow.UnitOfMeasureRepository.Create(Input);

			// Assert
			var Output = DataContext.UnitOfMeasure.Where(x => x.Id == 1).FirstOrDefault();
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
			await uow.UnitOfMeasureRepository.Create(Input);

			// Update
			var UpdateData = DataContext.UnitOfMeasure.SingleOrDefault(x => x.Code == Input.Code);
			Input = new UnitOfMeasure
			{
				Id = UpdateData.Id,
				Code = "THUNG",
				Name = "Thùng",
				StatusId = 1,
				CreatedAt = UpdateData.CreatedAt,
				UpdatedAt = DateTime.Now,
				Used = true,
			};
			await uow.UnitOfMeasureRepository.Update(Input);

			//Assert
			var Output = DataContext.UnitOfMeasure.Find(Input.Id);
			Assert.AreEqual(Input.Code, Output.Code);
			Assert.AreEqual(Input.Name, Output.Name);
			Assert.AreEqual(Input.StatusId, Output.StatusId);
			Assert.AreEqual(Input.Used, Output.Used);
			Assert.AreEqual(Input.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"), Output.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss"));
		}
		//Delete
		[Test]
		public async Task UnitOfMeasure_Delete_ReturnTrue()
		{
			// Create Instance
			await repository.Create(Input);

			// Delete
			var DeleteData = DataContext.UnitOfMeasure.SingleOrDefault(x => x.Id == 1);
			await repository.Delete(ConvertDAOToEntity(DeleteData));
			// Assert
			var Output = await repository.Get(1);//DataContext.OrderService.SingleOrDefault(x => x.Id == 1);
			Assert.IsNotNull(Output.DeletedAt);
		}
		public UnitOfMeasure ConvertDAOToEntity(UnitOfMeasureDAO UnitOfMeasureDAO)
		{
			return new UnitOfMeasure
			{
				Id = UnitOfMeasureDAO.Id,
				Code = UnitOfMeasureDAO.Code,
				Name = UnitOfMeasureDAO.Name,
				StatusId = UnitOfMeasureDAO.StatusId,
				CreatedAt = UnitOfMeasureDAO.CreatedAt,
				UpdatedAt = UnitOfMeasureDAO.UpdatedAt,
				DeletedAt = UnitOfMeasureDAO.DeletedAt,
				Used = UnitOfMeasureDAO.Used,
			};
		}
		//List Order By Name + Skip and Take
		//[Test]
		public async Task UnitOfMeasure_GetListByName_ReturnTrue()
		{
			// Create Instance
			await uow.UnitOfMeasureRepository.Create(Input);
			await uow.UnitOfMeasureRepository.Create(Input);
			await uow.UnitOfMeasureRepository.Create(Input);

			string Name = "Chiếc";

			// Get List
			UnitOfMeasureFilter UnitOfMeasureFilter = new UnitOfMeasureFilter
			{
				Skip = 0,
				Take = 10,
				Name = new StringFilter { Equal = Name },
				Selects = UnitOfMeasureSelect.Name
			};

			//Assert
			int count = await repository.Count(UnitOfMeasureFilter);
			Assert.AreEqual(3, count);
		}
		//List Order By Type + Skip and Take

		//Bulk Insert 
	}
}
