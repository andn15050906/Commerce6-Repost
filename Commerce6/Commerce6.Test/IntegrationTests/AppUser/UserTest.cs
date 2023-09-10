using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Commerce6.Test.IntegrationTests;
using Commerce6.Test.Helpers;
using Commerce6.Data.Domain.Merchant;
using Commerce6.Test.DataGenerators;
using Commerce6.Web.Models.Contact.AddressDTOs;

namespace Commerce6.Test.ControllerTest.AppUser
{
    public class UserTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public UserTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();

            //login before any test
            _client.GetAuthHeaders("Ngothinguyet771@gmail.com", "Ngothinguyet771");
        }

        //[Fact]
        public async Task Update()
        {
            //Arrange
            //Case 1
            MultipartFormDataContent content = new()
            {
                { new StringContent("Ngô Thị Nguyệt updated"), "FullName" }
            };

            //Case 2
            //could not test this because of the relative path
            /*MultipartFormDataContent content = new()
            {
                { new StringContent("Ngô Thị Nguyệt"), "FullName" },
                { new ByteArrayContent(FileHelper.GetFile("4-black-wallpaper.jpg", Dir.Avatar)), "Avatar", "4-black-wallpaper.jpg" }
            };*/

            //Act
            HttpResponseMessage response = await _client.RequestFormData(HttpMethod.Put, "api/users", content);

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            if (response.IsSuccessStatusCode)
                _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        //Batch test
        //[Fact]
        public async Task UpdateAddresses()
        {
            KeyValuePair<string, string>[] users = AccountHelper.GetTargetedUsers();
            Dictionary<string, string?> keyValuePairs;

            foreach (KeyValuePair<string, string> user in users)
            {
                //Arrange
                AddressRequestDTO dto = AddressGenerator.GenerateRandomAddressDTO();
                keyValuePairs = new()
                {
                    { "Address.Province", dto.Province },
                    { "Address.District", dto.District },
                    { "Address.Street", dto.Street },
                    { "Address.StreetNumber", dto.StreetNumber }
                };
                MultipartFormDataContent content = new();
                foreach (KeyValuePair<string, string?> kvp in keyValuePairs)
                    if (kvp.Value != null)
                        content.Add(new StringContent(kvp.Value), kvp.Key);

                //Act
                _client.GetAuthHeaders(user.Key, user.Value);
                HttpResponseMessage response = await _client.RequestFormData(HttpMethod.Put, "api/users", content);

                //Assert
                _output.WriteLine(response.StatusCode.ToString());
                _output.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }


        /*[Fact]
        public async Task ChangePassword()
        {
            //Arrange
            ChangePasswordDTO dto = new() { CurrentPassword = "771Ngothinguyet", NewPassword = "Ngothinguyet771" };

            //Act
            HttpResponseMessage response =await _client.RequestJson(
                HttpMethod.Put, "api/users/ChangePassword", JsonSerializer.Serialize(dto));

            //Assert
            _output.WriteLine(response.StatusCode.ToString());
            Assert.True(response.IsSuccessStatusCode);
        }*/
    }
}
