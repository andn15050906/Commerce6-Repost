using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using Commerce6.Test.IntegrationTests;
using Commerce6.Web.Models.Merchant.CategoryDTOs;

namespace Commerce6.Test.Seeder
{
    public class CategorySeeder : IClassFixture<WebApplicationFactory<Program>>
    {
        public class Data
        {
            public string Name { get; set; }
            public int? Parent { get; set; }
            public string Description { get; set; }
        }

        private readonly HttpClient _client;
        private readonly List<Data> data = new();

        public CategorySeeder(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            //loginAsAdmin before seeding
            _client.GetAuthHeaders("0123456789", "AdminPass");
        }

        //[Fact]
        public async Task Seed()
        {
            PrepareData();

            foreach (Data category in data)
            {
                CreateCategoryDTO dto = new()
                {
                    Name = category.Name,
                    Parent = category.Parent,
                    Description = category.Description
                };

                await _client.RequestJson(HttpMethod.Post, "api/Categories", JsonSerializer.Serialize(dto));
            }
        }

        private void PrepareData()
        {
            data.Add(new Data { Name = "Thời trang nam" });
            data.Add(new Data { Name = "Thời trang nữ" });
            data.Add(new Data { Name = "Điện thoại và phụ kiện" });
            data.Add(new Data { Name = "Mẹ và bé" });
            data.Add(new Data { Name = "Thiết bị điện tử" });
            data.Add(new Data { Name = "Nhà cửa và đời sống" });
            data.Add(new Data { Name = "Máy tính và laptop" });
            data.Add(new Data { Name = "Sắc đẹp" });
            data.Add(new Data { Name = "Máy ảnh và máy quay phim" });
            data.Add(new Data { Name = "Sức khỏe" });
            data.Add(new Data { Name = "Đồng hồ" });
            data.Add(new Data { Name = "Giày dép nữ" });
            data.Add(new Data { Name = "Túi ví nữ" });
            data.Add(new Data { Name = "Giày dép nam" });
            data.Add(new Data { Name = "Thiết bị gia dụng" });
            data.Add(new Data { Name = "Phụ kiện và trang sức nữ" });
            data.Add(new Data { Name = "Thể thao và du lịch" });
            data.Add(new Data { Name = "Bách hóa" });
            data.Add(new Data { Name = "Xe" });
            data.Add(new Data { Name = "Sách" });
        }
    }
}
