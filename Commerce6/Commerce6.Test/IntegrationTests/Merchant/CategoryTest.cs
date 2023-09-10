using Commerce6.Web.Models.Merchant.CategoryDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using System.Text.Json;

namespace Commerce6.Test.IntegrationTests.Merchant
{
    public class CategoryTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public CategoryTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();

            //login before any test
            _client.GetAuthHeaders("0123456789", "AdminPass");
        }

        //[Fact]
        public async Task Create()
        {
            //Arrange
            CreateCategoryDTO dto = new();

            /*dto.Parent = 1;
            dto.Name = "Thời trang nam";
            dto.Description = "Kiểu cách thịnh hành";*/

            dto.Name = "Thời trang nam";
            dto.Description = "Kiểu cách thịnh hành";

            /*dto.Name = "Thời trang nam";*/

            /*dto.Parent = 1;
            dto.Name = "Áo khoác";*/

            //Act
            HttpResponseMessage response = await _client.RequestJson(
                 HttpMethod.Post, "api/Categories", JsonSerializer.Serialize(dto));

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            Assert.True(response.IsSuccessStatusCode);
        }
        
        //[Fact]
        public async Task Update()
        {
            //Arrange
            UpdateCategoryDTO dto = new();

            /*dto.Id = 4;
            dto.Name = "Thời Trang Nữ";
            dto.Description = "Description";*/

            dto.Id = 1;
            dto.Name = "Thời Trang Nam";
            dto.Description = "Kiểu cách thịnh hành";

            //Act
            HttpResponseMessage response = await _client.RequestJson(
                 HttpMethod.Put, "api/Categories", JsonSerializer.Serialize(dto));

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            Assert.True(response.IsSuccessStatusCode);
        }

        //[Fact]
        public async Task Delete()
        {
            //Arrange
            string id = "Thời Trang Nam";

            //Act
            HttpResponseMessage response = await _client.DeleteAsync($"api/Categories/{id}");

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
