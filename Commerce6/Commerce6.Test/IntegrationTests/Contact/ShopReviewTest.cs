using Commerce6.Web.Models.Merchant.ShopReviewDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using System.Text.Json;

namespace Commerce6.Test.IntegrationTests.Contact
{
    public class ShopReviewTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public ShopReviewTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Create()
        {
            //Arrange
            _client.GetAuthHeaders("Ngodangsang026@hotmail.com", "Ngodangsang026");
            CreateShopReviewDTO dto = new()
            {
                Content = "Shop nhiệt tình đóng gói hàng cẩn thận nên mua nha mọi người",
                Rating = 5,
                ShopId = 4
            };

            //Act
            HttpResponseMessage response = await _client.RequestJson(
                HttpMethod.Post, "/api/shopreviews", JsonSerializer.Serialize(dto));

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        public async Task Update()
        {
            //Arrange
            _client.GetAuthHeaders("Ngodangsang026@hotmail.com", "Ngodangsang026");
            UpdateShopReviewDTO dto = new()
            {
                Id = 1,
                Content = "shop nhiệt tình đóng gói hàng cẩn thận nên mua nha mọi người",
                Rating = 4
            };

            //Act
            HttpResponseMessage response = await _client.RequestJson(
                HttpMethod.Put, "/api/shopreviews", JsonSerializer.Serialize(dto));

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        public async Task Delete()
        {
            //Arrange
            _client.GetAuthHeaders("Ngodangsang026@hotmail.com", "Ngodangsang026");

            //Act
            HttpResponseMessage response = await _client.DeleteAsync("/api/shopreviews/" + 1);

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
