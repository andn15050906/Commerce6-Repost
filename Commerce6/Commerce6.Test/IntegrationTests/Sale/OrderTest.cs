using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Test.Helpers;
using Commerce6.Web.Models.Contact.AddressDTOs;
using Commerce6.Web.Models.Sale.OrderDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Net.Http.Json;
using Xunit.Abstractions;
using Commerce6.Infrastructure.Models.Sale;
using Commerce6.Data.Domain.Sale;
using Commerce6.Test.DataGenerators;

namespace Commerce6.Test.IntegrationTests.Sale
{
    public class OrderTest : IClassFixture<WebApplicationFactory<Program>>
    {
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

        public OrderTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        //Modified: No longer use FormData
        public async Task Create()
        {
            //Arrange
            (int, List<Order_ProductRequestDTO>) shop_oprs = await ChooseRandomProducts();

            KeyValuePair<string, string> customer = AccountHelper.GetRandomUser();
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
            //_output.WriteLine(JsonSerializer.Serialize(dto));



            MultipartFormDataContent content = new();
            Dictionary<string, string?> keyValuePairs = new()
            {
                { "PaymentMethod", dto.PaymentMethod },
                { "Transporter", dto.Transporter },
                { "ShopId", dto.ShopId.ToString() }
            };
            if (dto.ToAddress != null)
            {
                keyValuePairs.Add("ToAddress.Province", dto.ToAddress.Province);
                keyValuePairs.Add("ToAddress.District", dto.ToAddress.District);
                keyValuePairs.Add("ToAddress.Street", dto.ToAddress.Street);
                keyValuePairs.Add("ToAddress.StreetNumber", dto.ToAddress.StreetNumber);
            }
            else
                keyValuePairs.Add("DefaultAddress", dto.DefaultAddress.ToString());    //true
            for (int i = 0; i < dto.Products.Length; i++)
            {
                keyValuePairs.Add($"Products[{i}].Quantity", dto.Products[i].Quantity.ToString());
                keyValuePairs.Add($"Products[{i}].ProductId", dto.Products[i].ProductId);
            }
            foreach (KeyValuePair<string, string?> kvp in keyValuePairs)
                if (kvp.Value != null)
                    content.Add(new StringContent(kvp.Value), kvp.Key);

            //Act
            HttpResponseMessage res = await _client.RequestFormData(HttpMethod.Post, "api/orders", content);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        //Modified: No longer use FormData
        public async Task UpdateAsMerchant()
        {
            _client.GetAuthHeaders("Ngothinguyet771@gmail.com", "Ngothinguyet771");
            _client.GetShopAuthHeaders();

            HttpResponseMessage response = await _client.GetAsync("/api/orders/merchant");



            List<OrderDTO>? targetOrders = await response.Content.ReadFromJsonAsync<List<OrderDTO>>();
            _output.WriteLine(JsonSerializer.Serialize(targetOrders));

            if (targetOrders != null)
            {
                //Arrange
                MerchantUpdateOrderDTO dto = new()
                {
                    OrderId = targetOrders[0].Id,
                    State = GetState(OrderState.Completed),
                    /*Discount = 0.2,
                    FromAddress = AddressGenerator.GenerateRandomAddressDTO()*/
                    /*DefaultAddress = true*/
                };

                MultipartFormDataContent content = new();;
                Dictionary<string, string?> dic = new()
                {
                    { "OrderId", dto.OrderId },
                    { "State", dto.State.ToString() },
                    { "Discount", dto.Discount.ToString() }
                };
                if (dto.FromAddress != null)
                {
                    dic.Add("FromAddress.Province", dto.FromAddress.Province);
                    dic.Add("FromAddress.District", dto.FromAddress.District);
                    dic.Add("FromAddress.Street", dto.FromAddress.Street);
                    dic.Add("FromAddress.StreetNumber", dto.FromAddress.StreetNumber);
                }
                else
                    dic.Add("DefaultAddress", dto.DefaultAddress.ToString());    //true
                foreach (KeyValuePair<string, string?> kvp in dic)
                    if (kvp.Value != null)
                        content.Add(new StringContent(kvp.Value), kvp.Key);

                //Act
                HttpResponseMessage response2 = await _client.RequestFormData(HttpMethod.Put, "/api/orders/merchant", content);

                //Assert
                _output.WriteLine(response2.StatusCode.ToString());
                _output.WriteLine(response2.Content.ReadAsStringAsync().Result);
            }
        }

        //[Fact]
        //Modified: No longer use FormData
        public async Task UpdateAsCustomer()
        {
            string orderId = "776643a8-d344-4af8-81ac-8025b4b4bcf6";
            _client.GetAuthHeaders("Ngodangsang026@hotmail.com", "Ngodangsang026");

            HttpResponseMessage response = await _client.GetAsync("/api/orders/customer");
            List<OrderDTO>? targetOrders = await response.Content.ReadFromJsonAsync<List<OrderDTO>>();

            if (targetOrders != null)
            {
                //Arrange
                CustomerUpdateOrderDTO dto = new()
                {
                    OrderId = orderId,
                    State = GetState(OrderState.Delivered)
                };

                MultipartFormDataContent content = new();
                Dictionary<string, string?> keyValuePairs = new()
                {
                    { "OrderId", dto.OrderId },
                    { "PaymentMethod", dto.PaymentMethod },
                    { "Transporter", dto.Transporter },
                    { "State", dto.State.ToString() }
                };
                if (dto.ToAddress != null)
                {
                    keyValuePairs.Add("ToAddress.Province", dto.ToAddress.Province);
                    keyValuePairs.Add("ToAddress.District", dto.ToAddress.District);
                    keyValuePairs.Add("ToAddress.Street", dto.ToAddress.Street);
                    keyValuePairs.Add("ToAddress.StreetNumber", dto.ToAddress.StreetNumber);
                }
                foreach (KeyValuePair<string, string?> kvp in keyValuePairs)
                    if (kvp.Value != null)
                        content.Add(new StringContent(kvp.Value), kvp.Key);

                //Act
                HttpResponseMessage response2 = await _client.RequestFormData(HttpMethod.Put, "/api/orders/customer", content);

                //Assert
                _output.WriteLine(response2.StatusCode.ToString());
                _output.WriteLine(response2.Content.ReadAsStringAsync().Result);
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
                if (kvp.Value > maxProductCount)
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
                dtos.Add(new Order_ProductRequestDTO {
                    Quantity = rd.Next(product.Stock / 10) + 1,
                    ProductId = product.Id
                });

            return (maxProductShop, dtos);
        }

        private int GetState(OrderState state)
            => (int)state;
    }
}
