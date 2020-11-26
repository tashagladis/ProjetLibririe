using NUnit.Framework;
using APILibrary.Core.Attributs.Controllers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using APILibrary.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace APILibrary.Test
{
    public class Tests<T, TContext> : ControllerBase where T : ModelBase where TContext : DbContext
    {
        private ControllerBaseAPI<T,TContext> controller;
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            
            var result = controller.GetAllAsync();
            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            //var contacts = okResult.Value.Should().BeAssignableTo<IEnumerable<>>().Subject;


            //contacts.Count().Should().Be(3);


            //Sur quel liste tester �tant donn� qu'on utlise des types num�riques
            // comment tester
            //Quels seront les champs de T


        }

    }
}