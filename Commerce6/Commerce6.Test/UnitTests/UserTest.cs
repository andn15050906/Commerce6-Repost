using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using System.Text.Json;
using Commerce6.Web.Controllers.AppUserControllers;
using Commerce6.Web.Models.AppUser.UserDTOs;
using Commerce6.Web.Services;
using Commerce6.Web.Services.AppUserServices;
using Commerce6.Test.UnitTests.Fixtures;

namespace Commerce6.Test.UnitTests
{
    public class UserTest : IClassFixture<ServicesFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly UsersController _usersController;

        public UserTest(ServicesFixture fixture, ITestOutputHelper output)
        {
            _output = output;
            UnitOfWork uow = new UnitOfWork(fixture.Context);
            UserService userService = new UserService(uow);
            _usersController = new UsersController(userService);
        }


        //[Fact]
        public void RegisterAsAdmin()
        {
            //Arrange
            RegisterDTO dto = new()
            {
                FullName = "Admin",
                Phone = "0123456789",
                Password = "AdminPass",
                Email = "antrongdn2021@gmail.com",
                DateOfBirth = new(2003, 9, 10)
            };

            //Act
            IActionResult result = _usersController.RegisterAsAdmin(dto);

            //Assert
            Assert.True(((StatusCodeResult)result).StatusCode == 201);
        }

        //[Fact]
        public void Get()
        {
            //Arrange
            string[] ids = {
                "02827073-0b7c-4ca5-991d-193969751499",
                "02827073-0b7c-4ca5-991d-193969751490",             //fail
                "3cbded5f-d7fd-4c12-9384-7b09003e17f0",
                "b28c381d-4b45-47f6-a874-99e442a76091",
            };

            //Act
            IActionResult[] results = new IActionResult[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                _usersController.Get(ids[i]);
                results[i] = _usersController.Get(ids[i]);
            }

            //Assert
            for (int i = 0; i < ids.Length; i++)
            {
                if (i == 1)
                    Assert.True(results[i].GetType() == typeof(NotFoundResult));
                else
                {
                    Assert.True(results[i].GetType() == typeof(OkObjectResult));
                    //output.WriteLine(((OkObjectResult)results[i]).Value?.ToString());
                    _output.WriteLine(ReadOkObjectResult((OkObjectResult)results[i]));
                }
            }
        }

        private string ReadOkObjectResult(OkObjectResult result) => JsonSerializer.Serialize(result);
    }
}
