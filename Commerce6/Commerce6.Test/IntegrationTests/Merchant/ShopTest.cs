using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Commerce6.Test.IntegrationTests.Merchant
{
    public class ShopTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public ShopTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();

            //login & loginAsMerchant before any test
            _client.GetAuthHeaders("Ngothinguyet771@gmail.com", "Ngothinguyet771");
            _client.GetShopAuthHeaders();
        }

        //[Fact]
        public async Task CreateCategory()
        {
            //Arrange
            string name;

            name = "Giảm giá 50%";

            //Act
            HttpResponseMessage res = await _client.PostAsync($"/api/shops/Category?name={name}", null);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
        }

        //[Fact]
        public async Task UpdateCategory()
        {
            //Arrange
            int id;
            string name;

            id = 1;
            name = "Giảm giá 50%";

            //Act
            HttpResponseMessage res = await _client.PutAsync($"/api/shops/Category?id={id}&name={name}", null);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
        }

        //[Fact]
        public async Task DeleteCategory()
        {
            //Arrange
            int id;
            id = 3;

            //Act
            HttpResponseMessage res = await _client.DeleteAsync($"/api/shops/Category?id={id}");

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
        }
    }
}
