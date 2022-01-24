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

namespace SampleProject.Tests
{
    [TestFixture]
    public class UnitOfMeasureRepoTest : CommonTests
    {
        IUnitOfMeasureRepository repository;
        public UnitOfMeasureRepoTest() : base()
        {

        }

        [SetUp]
        public async Task Setup()
        {
            await Clean();
            repository = new UnitOfMeasureRepository(DataContext);
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
        [Test]
        public async Task UnitOfMeasure_Create_ReturnTrue()
        {
            UnitOfMeasure Input = new UnitOfMeasure
            {
                StatusId = 1,
                Code = "CHIEC",
                Name = "Chiếc",
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

        //Delete

        //List Order By Name + Skip and Take

        //List Order By Type + Skip and Take
      
        //Bulk Insert 
    }
}
