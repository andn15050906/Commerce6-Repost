using Commerce6.Test.Helpers;
using Commerce6.Test.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Commerce6.Test.Seeder
{
    public class FollowSeeder : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public FollowSeeder(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Seed()
        {
            //Arrange
            OrderSeeder.Data data = SeedingHelper.ReadGeneratedOrders().First();
            string username = data.CustomerName;

            //Act
            _client.GetAuthHeaders(username, AccountHelper.GetUserPassword(username));
            HttpResponseMessage res = await _client.PostAsync("api/follows?shopId=" + data.ShopId, null);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);
        }
    }
}
