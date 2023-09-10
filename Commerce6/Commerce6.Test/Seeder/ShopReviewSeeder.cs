using Commerce6.Test.Helpers;
using Commerce6.Test.IntegrationTests;
using Commerce6.Web.Models.Merchant.ShopReviewDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using System.Text.Json;

namespace Commerce6.Test.Seeder
{
    public class ShopReviewSeeder : IClassFixture<WebApplicationFactory<Program>>
    {
        private (string, int)[] ContentLst = new (string, int)[]
        {
            ("Shop nhiệt tình đóng gói hàng cẩn thận nên mua nha mọi người", 5)
        };

        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public ShopReviewSeeder(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Seed()
        {
            //Arrange
            Random rd = new();
            OrderSeeder.Data data = SeedingHelper.ReadGeneratedOrders().First();
            string username = data.CustomerName;
            (string, int) content_rating = ContentLst[rd.Next(ContentLst.Length)];
            CreateShopReviewDTO dto = new()
            {
                Content = content_rating.Item1,
                Rating = content_rating.Item2,
                ShopId = data.ShopId
            };

            //Act
            _client.GetAuthHeaders(username, AccountHelper.GetUserPassword(username));
            HttpResponseMessage response = await _client.RequestJson(
                HttpMethod.Post, "/api/shopreviews", JsonSerializer.Serialize(dto));

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}
