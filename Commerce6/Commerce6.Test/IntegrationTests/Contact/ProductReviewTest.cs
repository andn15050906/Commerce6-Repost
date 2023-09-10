using Commerce6.Web.Models.Merchant.ProductReviewDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using System.Text.Json;

namespace Commerce6.Test.IntegrationTests.Contact
{
    public class ProductReviewTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public ProductReviewTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Create()
        {
            //Arrange
            _client.GetAuthHeaders("Ngodangsang026@hotmail.com", "Ngodangsang026");
            CreateProductReviewDTO dto = new()
            {
                Content = "Nó chung là giao hàng hơi chậm áo có dính 1 vết bẩn bo mực in hay sao í",
                Rating = 4,
                ProductId = "21d6a2db-7d20-40b9-a37b-d863a478b365"
            };

            //Act
            HttpResponseMessage response = await _client.RequestJson(
                HttpMethod.Post, "/api/productreviews", JsonSerializer.Serialize(dto));

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        public async Task Update()
        {
            //Arrange
            _client.GetAuthHeaders("Ngodangsang026@hotmail.com", "Ngodangsang026");
            UpdateProductReviewDTO dto = new()
            {
                Id = 2,
                Content = "Nói chung là giao hàng hơi chậm áo có dính 1 vết bẩn bo mực in hay sao í",
                Rating = 3
            };
            
            //Act
            HttpResponseMessage response = await _client.RequestJson(
                HttpMethod.Put, "/api/productreviews", JsonSerializer.Serialize(dto));

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
            HttpResponseMessage response = await _client.DeleteAsync("/api/productreviews/" + 2);

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
