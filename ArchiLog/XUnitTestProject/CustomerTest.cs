using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Controllers;
using Xunit;
using XUnitTestProject.Mock;
using XUnitTestProject.Mock.Models;

namespace XUnitTestProject
{
   public class CustomerTest
    {
        private MockDbContext _db;
        private CustomerController _controller;

        
        public CustomerTest()
        {
            _db = MockDbContext.GetDbContext();
            _controller = new CustomerController(_db);
        }

        [Fact]
        public async Task TestGetAll()
        {
            var actionResult = await _controller.GetAllAsync();
            var result = actionResult.Result as ObjectResult;
            var values = ((IEnumerable<object>)(result).Value);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

            _db.Customers.Count().Should().Be(values.Count());

        }


        [Fact]
        public async Task TestGetBYId()
        {

            var actionResult = await _controller.GetById(2, "");
            var result = actionResult.Result as ObjectResult;

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        }

        [Fact]
        public async Task TestPut()
        {
            CustomerMock customer = new CustomerMock
            {
                Email = "AliAhmadr@yahoo.fr",
                Phone = "65421895154",
                Lastname = "Fouret",
                Firstname = "Jeanne",
                Genre = "Autres",
                Address = null,
                ZipCode = "6854",
                City = "Limoges",
                ID = 2
            };

            var actionResult = await _controller.UpdateItem(2, customer);
            var result = actionResult.Result as ObjectResult;

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        }

        [Fact]
        public async Task TestCreate()
        {
            CustomerMock customer = new CustomerMock
            {
                Email = "AliAhmadr@yahoo.fr",
                Phone = "65421895154",
                Lastname = "Maria",
                Firstname = "Julia",
                Genre = "Autres",
                Address = null,
                ZipCode = "6854",
                City = "Limoges"

            };

            var actionResult = await _controller.CreateItem(customer);
            var result = actionResult.Result as ObjectResult;

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        }

        [Fact]
        public async Task TestDelete()
        {
            // Penser à Changer la valeur retounée de la DeleteItem
            var actionResult = await _controller.DeleteItem(1);
            var result = actionResult.Result as ObjectResult;

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        }

        [Fact]
        public async Task TestSearch()
        {
            var actionResult = await _controller.Search("*Charles*", "Homme", "");
            var result = actionResult.Result as ObjectResult;

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        }

        [Fact]
        public async Task TestNotFoundSearch()
        {
            var actionResult = await _controller.Search("Charles", "Homme", "");
            var result = actionResult.Result as ObjectResult;

            var okResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;

        }


        [Fact]
        public async Task TestSort()
        {
            var actionResult = await _controller.Sort("", "lastname", "");
            var result = actionResult.Result as ObjectResult;

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        }
    }
}
