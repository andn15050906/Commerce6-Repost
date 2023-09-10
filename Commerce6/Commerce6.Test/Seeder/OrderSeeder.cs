using Commerce6.Data.Domain.Sale;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Infrastructure.Models.Sale;
using Commerce6.Test.Helpers;
using Commerce6.Test.IntegrationTests;
using Commerce6.Web.Models.Contact.AddressDTOs;
using Commerce6.Web.Models.Sale.OrderDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using Xunit.Abstractions;

namespace Commerce6.Test.Seeder
{
    public class OrderSeeder : IClassFixture<WebApplicationFactory<Program>>
    {
        public class Data
        {
            public List<string> ProductIds { get; set; }
            public int ShopId { get; set; }
            public string CustomerName { get; set; }
        }

        private string[] PaymentMethods = new string[] {
            "PayPal", "MasterCard",
            "VNPay", "Momo", "ViettelPay",
            "AppCurrency",
            "CashOnDelivery"
        };

        private string[] Transporters = new string[]
        {
            "EMS", "ViettelPost", "Giaohangnhanh", "Giaohangtietkiem", "GRAB"
        };

        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public OrderSeeder(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Seed()
        {
            //Arrange
            (int, List<Order_ProductRequestDTO>) shop_oprs = await ChooseRandomProducts();

            KeyValuePair<string, string> customer = AccountHelper.GetRandomUser();
                //Might fail - should not be the shop's owner
            _client.GetAuthHeaders(customer.Key, customer.Value);

            string payment = GetRandomPaymentMethod();
            string transporter = GetRandomTransporter();
            //...
            AddressRequestDTO? address = null;
            bool defaultAddress = true;

            CreateOrderDTO dto = new()
            {
                PaymentMethod = payment,
                Transporter = transporter,
                ToAddress = address,
                DefaultAddress = defaultAddress,
                ShopId = shop_oprs.Item1,
                Products = shop_oprs.Item2.ToArray()
            };

            //Act
            HttpResponseMessage res = await _client.RequestJson(HttpMethod.Post, "api/orders", JsonSerializer.Serialize(dto));

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);

            if (res.StatusCode == System.Net.HttpStatusCode.Created)
            {
                List<Data> data = new()
                {
                    new Data
                    {
                        ProductIds = shop_oprs.Item2.Select(_ => _.ProductId).ToList(),
                        ShopId = shop_oprs.Item1,
                        CustomerName = customer.Key
                    }
                };
                SeedingHelper.SaveGeneratedOrders(data);
            }
        }

        private string GetRandomPaymentMethod()
            => PaymentMethods[new Random().Next(PaymentMethods.Length)];

        private string GetRandomTransporter()
            => Transporters[new Random().Next(Transporters.Length)];

        private async Task<(int, List<Order_ProductRequestDTO>)> ChooseRandomProducts()
        {
            //~Seeding

            List<ProductDTO>? products = await ProductHelper.GetProducts(_client);

            //Get the shop with most selected products
            Dictionary<int, int> map = new();
            int maxProductShop = 0, maxProductCount = 0;
            foreach (ProductDTO product in products!)
            {
                if (map.ContainsKey(product.ShopId))
                    map[product.ShopId]++;
                else
                    map.Add(product.ShopId, 1);
            }
            foreach (KeyValuePair<int, int> kvp in map)
                if (kvp.Value >= maxProductCount)
                {
                    maxProductCount = kvp.Value;
                    maxProductShop = kvp.Key;
                }

            //take some random products (at least 1)
            List<ProductDTO> shopProducts = new();
            List<ProductDTO> selected = new();
            foreach (ProductDTO product in products)
                if (product.ShopId == maxProductShop)
                    shopProducts.Add(product);
            Random rd = new();
            foreach (ProductDTO product in shopProducts)
            {
                if (rd.Next(2) == 1)
                    selected.Add(product);
                //no more than 5 products in the order
                if (selected.Count > 5)
                    break;
            }
            //if not added any, select the last one
            if (selected.Count == 0)
                selected.Add(products.Last());

            //randomize quantity
            List<Order_ProductRequestDTO> dtos = new();
            foreach (ProductDTO product in selected)
                dtos.Add(new Order_ProductRequestDTO
                {
                    Quantity = rd.Next(product.Stock / 10) + 1,
                    ProductId = product.Id
                });

            return (maxProductShop, dtos);
        }
    }
}
