using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Text;
using Commerce6.Web.Models.AppUser.UserDTOs;

namespace Commerce6.Test.ControllerTest.AppUser
{
    public class AuthTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;

        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _client = _factory.CreateClient();
        }

        //[Fact]
        public async Task Login()
        {
            //Arrange
            LoginDTO dto = new() { PhoneOrEmail = "Ngothinguyet771@gmail.com", Password = "Ngothinguyet771" };
            StringContent content = new(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            //Act
            HttpResponseMessage response = await _client.PostAsync("/api/auth/Login", content);

            //Assert
            Assert.True(response.IsSuccessStatusCode);

            foreach(KeyValuePair<string, IEnumerable<string>> kv in
                response.Headers.Where(h => h.Key == "Set-Cookie"))
            {
                foreach(string val in kv.Value)
                    _output.WriteLine(val);
            }
        }
    }
}
