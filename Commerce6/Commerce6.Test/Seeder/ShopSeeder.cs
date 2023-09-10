using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Commerce6.Test.IntegrationTests;
using Commerce6.Web.Models.Merchant.ShopDTOs;
using Commerce6.Test.DataGenerators;
using Commerce6.Test.Helpers;

namespace Commerce6.Test.Seeder
{
    public class ShopSeeder : IClassFixture<WebApplicationFactory<Program>>
    {
        public class Data
        {
            public KeyValuePair<string, string> User { get; set; }
            public CreateShopDTO Dto { get; set; }
            public List<(byte[], string)> Images { get; set; }
        }

        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;
        private readonly List<Data> data = new();

        public ShopSeeder(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
            DirHelper.SetToWebProject();
        }

        //[Fact]
        public async Task Seed()
        {
            PrepareData();
            MultipartFormDataContent content;
            Dictionary<string, string?> keyValuePairs;

            foreach (Data shop in data)
            {
                //Arrange
                _client.GetAuthHeaders(shop.User.Key, shop.User.Value);

                content = new();
                keyValuePairs = new()
                {
                    { "Address.Province", shop.Dto.Address.Province },
                    { "Address.District", shop.Dto.Address.District },
                    { "Address.Street", shop.Dto.Address.Street },
                    { "Address.StreetNumber", shop.Dto.Address.StreetNumber },
                    { "Name", shop.Dto.Name },
                    { "Phone", shop.Dto.Phone }
                };

                foreach (KeyValuePair<string, string?> kvp in keyValuePairs)
                    if (kvp.Value != null)
                        content.Add(new StringContent(kvp.Value), kvp.Key);

                //Act
                HttpResponseMessage res = await _client.RequestFormData(HttpMethod.Post, "api/shops", content);

                //Assert
                _output.WriteLine(res.StatusCode.ToString());
                _output.WriteLine(res.Content.ReadAsStringAsync().Result);
            }
        }

        private void PrepareData()
        {
            KeyValuePair<string, string>[] users = AccountHelper.GetTargetedUsers();

            foreach (KeyValuePair<string, string> user in users)
            {
                data.Add(new Data
                {
                    User = user,
                    Dto = new CreateShopDTO
                    {
                        Address = AddressGenerator.GenerateRandomAddressDTO(),
                        Name = "Shop của " + user.Value,
                        Phone = new UserGenerator().GeneratePhone()
                    }
                    //no avatar
                });
            }
        }
    }
}
