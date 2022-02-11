﻿using SampleProject.Entities;
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
	public class UnitOfMeasureRepoTest : CommonTests
	{
		IStatusRepository repository;
		StatusDAO st1, st2;

		[SetUp]
		public async Task Setup()
		{
			Initialize();
			await Clean();
			repository = new StatusRepository(DataContext);

			st1 = new StatusDAO
			{
				Code = "ACTIVE",
				Name = "Hoạt động",
			};

			st2 = new StatusDAO
			{
				Code = "INACTIVE",
				Name = "Dừng hoạt động",
			};

			DataContext.Status.Add(st1);
			DataContext.Status.Add(st2);

			DataContext.SaveChanges();
		}

		[Test]
		public async Task Status_GetById_ReturnTrue()
		{
			Status status = await repository.Get(1);

			Assert.AreEqual(status.Id, st1.Id);
			Assert.AreEqual(status.Code, st1.Code);
			Assert.AreEqual(status.Name, st1.Name);
		}
	}
}
