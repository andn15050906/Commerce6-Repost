using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Xunit.Abstractions;
using Commerce6.Test.DataGenerators;
using Commerce6.Test.Helpers;
using Commerce6.Test.IntegrationTests;
using Commerce6.Web.Models.AppUser.UserDTOs;
using Commerce6.Web.Models.Contact.AddressDTOs;

namespace Commerce6.Test.Seeder
{
    public class UserSeeder : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public UserSeeder(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Seed()
        {
            //Arrange
            List<RegisterDTO> data = PrepareData();

            //Act
            List<KeyValuePair<string, string>> kvps = new();
            foreach (RegisterDTO dto in data)
            {
                HttpResponseMessage response =
                    await _client.RequestJson(HttpMethod.Post, "api/users", JsonSerializer.Serialize(dto));
                _output.WriteLine(response.StatusCode.ToString());
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    kvps.Add(new(dto.Email, dto.Password));
            }
            SeedingHelper.SaveGeneratedUsers(kvps);

            //Assert
            //No assertion (might be invalid since phoneGenerator does not guarantee distinct values)
        }

        //[Fact]
        public async Task UpdateAddress()
        {
            KeyValuePair<string, string>[] users = AccountHelper.GetTargetedUsers();
            Dictionary<string, string?> keyValuePairs;

            foreach (KeyValuePair<string, string> user in users)
            {
                //Arrange
                UpdateUserDTO dto = new UpdateUserDTO
                {
                    Address = AddressGenerator.GenerateRandomAddressDTO()
                };
                keyValuePairs = new()
                {
                    { "Address.Province", dto.Address.Province },
                    { "Address.District", dto.Address.District },
                    { "Address.Street", dto.Address.Street },
                    { "Address.StreetNumber", dto.Address.StreetNumber }
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

        private List<RegisterDTO> PrepareData()
        {
            UserGenerator userGenerator = new();
            Random rd = new();

            List<RegisterDTO> dtos = new();
            for (int i = 0; i < 10; i++)
                dtos.Add(GenerateRegisterDTO(rd, userGenerator));
            return dtos;
        }

        private RegisterDTO GenerateRegisterDTO(Random rd, UserGenerator userGen)
        {
            int gender = rd.Next(2);
            string name = gender == 0 ? userGen.GenerateMaleName() : userGen.GenerateFemaleName();
            string phone = userGen.GeneratePhone();
            string password = userGen.GeneratePassword(name, phone);
            return new RegisterDTO
            {
                FullName = name,
                Phone = phone,
                Password = password,
                Email = userGen.GenerateEmail(password),
                DateOfBirth = userGen.GenerateDateOfBirth()
            };
        }
    }
}
