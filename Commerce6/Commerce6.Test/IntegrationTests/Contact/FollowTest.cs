using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Commerce6.Test.IntegrationTests;
using Commerce6.Test.Helpers;

namespace Commerce6.Test.ControllerTest.Contact
{
    public class FollowTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public FollowTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Create()
        {
            //Arrange
            KeyValuePair<string, string> name_pass = AccountHelper.GetRandomUser();
            _client.GetAuthHeaders(name_pass.Key, name_pass!.Value);

            //Act
            HttpResponseMessage res = await _client.PostAsync("api/follows?shopId=" + 4, null);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        public async Task Delete()
        {
            //Arrange
            KeyValuePair<string, string> name_pass = new ("Lethuthuong724@gmail.com", "Lethuthuong724");
            _client.GetAuthHeaders(name_pass.Key, name_pass.Value);

            //Act
            HttpResponseMessage res = await _client.DeleteAsync("api/follows/" + 4);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);
        }
    }
}
