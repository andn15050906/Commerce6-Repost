using Commerce6.Test.Helpers;
using Commerce6.Web.Models.Merchant.AttributeDTOs;
using Commerce6.Web.Models.Merchant.ProductDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Commerce6.Test.IntegrationTests.Merchant
{
    public class ProductTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public ProductTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            string dir = Directory.GetCurrentDirectory();
            int end = dir.LastIndexOf(@"Commerce6\");
            string path = dir.Substring(0, end + @"ommerce6\".Length) + @"\Commerce6.Web";
            Environment.CurrentDirectory = path;

            _output = output;
            _client = factory.CreateClient();

            //login & loginAsMerchant before any test
            _client.GetAuthHeaders("Ngothinguyet771@gmail.com", "Ngothinguyet771");
            _client.GetShopAuthHeaders();
        }

        //[Fact]
        public async Task Create()
        {
            //Arrange
            MultipartFormDataContent content = new();
            List<(byte[], string)> files = new();
            int i;

            //~https://shopee.vn/%C3%81o-s%C6%A1-mi-ng%E1%BA%AFn-tay-form-r%E1%BB%99ng-th%E1%BB%9Di-trang-hi%E1%BB%87n-%C4%91%E1%BA%A1i-unisex-ch%E1%BA%A5t-li%E1%BB%87u-v%E1%BA%A3i-l%E1%BB%A5a-m%E1%BB%81m-ch%E1%BB%91ng-nh%C4%83n-i.289383259.10816146455?sp_atk=4924474c-e809-453d-9ac4-a607c9a344d2&xptdk=4924474c-e809-453d-9ac4-a607c9a344d2
            /*CreateProductDTO dto = new()
            {
                Name = "Áo sơ mi ngắn tay form rộng, thời trang hiện đại unisex chất liệu vải lụa mềm chống nhăn",
                Price = 199000,
                Description = "ÁO SƠ MI NGẮN TAY FORM RỘNG, THỜI TRANG HIỆN ĐẠI UNISEX CHẤT LIỆU VẢI LỤA MỀM CHỐNG NHĂN\r\n-\tChất liệu: Lụa hàn.\r\n-\tCông dụng: Chống nhăn, giãn nhẹ, êm ái, mềm mịn và mát da.\r\n-\tPhong cách: Unisex, Form rộng, Sweetwear.\r\n-\tDành cho: Nam và Nữ.\r\n-\tXu xướng: Hiện đại 2023.\r\n-\tXuất xứ: Made in Việt Nam.\r\n-\tSize : M L XL XXL.\r\n•\tM : Dưới 55kg, Cao dưới 1m65.\r\n•\tL : 55 - 65kg, Cao 1m65 – 1m72.\r\n•\tXL: 65 - 75kg, Cao 1m68 – 1m75.\r\n•\tXXL: 75 - 85kg, Cao 1m70 – 1m80.\r\nÁO SƠ MI NGẮN TAY FORM RỘNG, THỜI TRANG HIỆN ĐẠI UNISEX CHẤT LIỆU VẢI LỤA MỀM CHỐNG NHĂN\r\n-\tQuần short -> Tạo nên phong cách vô cùng đơn giản nhưng không kém phần cuốn hút. Đặc biệt mang đến cảm giác thoải mái cho người mặc.",
                Discount = 0.49,
                Stock = 32255,
                Attributes = new AttributeRequestDTO[]
                {
                    new AttributeRequestDTO { Name = "Dáng kiểu áo", Value = "Rộng" },
                    new AttributeRequestDTO { Name = "Cổ áo", Value = "Cổ sơ mi" },
                    new AttributeRequestDTO { Name = "Kiểu cổ áo", Value = "Cổ áo truyền thống" },
                    new AttributeRequestDTO { Name = "Phong cách", Value = "Thể thao, Cơ bản, Đường phố, Công sở" },
                    new AttributeRequestDTO { Name = "Xuất xứ", Value = "Việt Nam" },
                    new AttributeRequestDTO { Name = "Mẫu", Value = "Khác, Trơn" }
                },
                //ShopCategoryId...
                //Images below
                CategoryId = 2
            };
            files.Add((FileHelper.GetFile("ao-so-mi-1.jfif", Dir.ProductImage), "ao-so-mi-1.jfif"));
            files.Add((FileHelper.GetFile("ao-so-mi-2.jfif", Dir.ProductImage), "ao-so-mi-2.jfif"));*/

            //https://shopee.vn/%C3%81O-KHO%C3%81C-NH%E1%BA%B8-NAM-N%E1%BB%AE-2-L%E1%BB%9AP-THU-%C4%90%C3%94NG-FOM-R%E1%BB%98NG-H%E1%BB%8CA-TI%E1%BA%BET-PH%E1%BB%90I-VI%E1%BB%80N-S%E1%BB%8CC-TAY-%C4%90%C6%A0N-GI%E1%BA%A2N-H%C3%93T-TRIEND-2021-i.17475845.13836258622?sp_atk=77293b3f-07e7-47eb-a97a-c8ee74b51dd1&xptdk=77293b3f-07e7-47eb-a97a-c8ee74b51dd1
            /*CreateProductDTO dto = new()
            {
                Name = "ÁO KHOÁC NHẸ NAM NỮ 2 LỚP THU ĐÔNG FOM RỘNG HỌA TIẾT PHỐI VIỀN SỌC TAY ĐƠN GIẢN HÓT TRIEND 2021",
                Price = 158000,
                Description = "Bên trong áo có lót dù thoáng khí mát mẻ, tạo cảm giác vận động thoải mái khi mặc.\r\nĐường chỉ may sắc sảo, cực kì chắc chắn & 2 lớp dày dặn.\r\nĐảm bảo không ra màu, không phai màu & không nhăn.\r\nDễ dàng Mix cùng nhiều kiểu trang phục yêu thích hằng ngày & tự tin đi đến mọi nơi mọi lúc với phong cách chất lừ của riêng bạn.\r\nHàng tại xưởng không qua trung gian, Hàng luôn có sẵn.\r\nKhách mua hàng được tặng các phần quà bất ngờ cho các đơn hàng ngẫu nhiên đặc biệt.\r\nHàng chuẩn đẹp như hình (cam kết hình chụp thật 100%)..\r\nShop luôn sẵn lòng hỗ trợ tư vấn giải đáp thắc mặc của bạn khi cần.\r\nXuất xứ: Việt Nam\r\n\r\n\r\nSIZE GỒM 3 SIZE : M ,L ,XL\r\n\r\n\r\nsize M : dành cho những bạn từ 43 đến 49kg < 1m 60\r\n\r\n\r\nsize L : dành cho những bạn từ 50 đến 58kg <1m7\r\n\r\n\r\nsize xl :dành cho những bạn từ 58 đến 69kg < 1m 76\r\nshop : ao.khoac.nam.nu.gia.si\r\nđịa chỉ 17/9a/6 đường 22 kp7 linh đông thủ đức hồ chí minh\r\nxuất xứ việt nam ",
                Discount = 0.4,
                Stock = 24287,
                ShopCategoryId = 2,
                Attributes = new AttributeRequestDTO[]
                {
                    new AttributeRequestDTO { Name = "Chất liệu", Value = "Nylon" },
                    //new AttributeRequestDTO { Name = "Mẫu", Value = "Sọc" },
                    //new AttributeRequestDTO { Name = "Phong cách", Value = "Thể thao, Boho, Đường phố" },
                    //new AttributeRequestDTO { Name = "Tall Fit", Value = "Có" },
                    //new AttributeRequestDTO { Name = "Rất lớn", Value = "Có" },
                    //new AttributeRequestDTO { Name = "Xuất xứ", Value = "Việt Nam" }
                },
                CategoryId = 2
            };
            files.Add((FileHelper.GetFile("ao-so-mi-3.jfif", Dir.ProductImage), "ao-so-mi-3.jfif"));*/

            //https://shopee.vn/BST-50-%C3%81o-thun-nam-n%E1%BB%AF-form-r%E1%BB%99ng-v%E1%BA%A3i-d%C3%A0y-m%E1%BB%8Bn-logo-c%C3%A1-t%C3%ADnh-c%C3%A1ch-%C4%91i%E1%BB%87u-i.76875639.6534135548?sp_atk=4613da88-17a4-41ec-8fb9-c30806e1fdff&xptdk=4613da88-17a4-41ec-8fb9-c30806e1fdff
            CreateProductDTO dto = new()
            {
                Name = "BST 50 - Áo thun nam nữ form rộng vải dày mịn logo cá tính cách điệu",
                Price = 0,
                Description = "BST 50 - Áo thun nam nữ form rộng vải dày mịn logo cá tính cách điệu\r\n🔜🔜 #aothunnamnu đẹp rẻ, độc lạ giống hình 100%\r\n☘  Bảng kích thước \r\n🔜 Size XS : < 25 kg chiều cao phù hợp từ 1m1 đến 1m3\r\n🔜 Size S : < 45 kg chiều cao phù hợp từ 1m5 đến 1m6\r\n🔜 Size M : < 55 kg chiều cao phù hợp từ 1m5 đến 1m65\r\n🔜 Size L : < 60 kg chiều cao phù hợp từ 1m5 đến 1m7\r\n🔜 Size XL : 60 kg  < 75kg chiều cao phù hợp từ 1m7 đến 1m75\r\n🔜 Size XXL: > 65 kg < 80kg chiều cao phù hợp từ 1m75 đến 1m80\r\n\r\n✨✨✨ FREESHUP ĐƠN HÀNG TRÊN 50K TẤT CẢ SẢN PHẨM CỦA SHOP\r\n☘ Xem thêm hơn 4000 mẫu đẹp độc lạ tai Shopee: https://shopee.vn/ao.thun",
                Discount = 0,
                Stock = 0,
                ShopCategoryId = 1,
                CategoryId = 2
            };







            Dictionary<string, string?> keyValuePairs = new()
            {
                { "Name", dto.Name },
                { "Price", dto.Price.ToString() },
                { "Description", dto.Description },
                { "Discount", dto.Discount.ToString() },
                { "Stock", dto.Stock.ToString() },
                { "ShopCategoryId", dto.ShopCategoryId.ToString() },
                { "CategoryId", dto.CategoryId.ToString() }
            };
            foreach (KeyValuePair<string, string?> kvp in keyValuePairs)
            {
                if (kvp.Value != null)
                    content.Add(new StringContent(kvp.Value), kvp.Key);
            }
            if (dto.Attributes != null)
            {
                for (i = 0; i < dto.Attributes.Length; i++)
                {
                    content.Add(new StringContent(dto.Attributes[i].Name), $"Attributes[{i}].Name");
                    content.Add(new StringContent(dto.Attributes[i].Value), $"Attributes[{i}].Value");
                }
            }
            for (i = 0; i < files.Count; i++)
            {
                content.Add(new ByteArrayContent(files[i].Item1), $"Images[{i}].Image", files[i].Item2);
                content.Add(new StringContent(i.ToString()), $"Images[{i}].Position");
            }

            //Act
            HttpResponseMessage res = await _client.RequestFormData(HttpMethod.Post, "api/products", content);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        public async Task Update()
        {
            //Arrange
            MultipartFormDataContent content = new();
            UpdateProductDTO dto = new();
            List<(byte[], string)> files = new();

            /*dto.Id = "4620545d-3b0e-4f24-b392-e1f35783d81b";
            dto.Name = "ÁO KHOÁC NHẸ NAM NỮ 2 LỚP THU ĐÔNG FOM RỘNG HỌA TIẾT PHỐI VIỀN SỌC TAY updated";
            dto.Price = 200000;
            dto.Description = "Updated description";
            dto.Discount = 0.5;
            dto.Stock = 2000;

            dto.DeletedAttributes = new int[] { 20, 21 };
            dto.AddedAttributes = new AttributeRequestDTO[] {
                new AttributeRequestDTO { Name = "Chất liệu", Value = "Cotton" },
                new AttributeRequestDTO { Name = "Phong cách", Value = "Đường phố, Công sở" },
            };
            //test - contains some not-existing files
            dto.DeletedImages = new string[] { "4f6c61bd-b58b-4fe8-bcff-4df98430b110_ao-so-mi-4.jfif", "241.jfif" };
            //dto.AddedImages
            files.Add((FileHelper.GetFile("ao-so-mi-3.jfif", Dir.ProductImage), "ao-so-mi-3.jfif"));*/

            dto.Id = "8b0d43da-55c1-4f92-bd36-9ee9e91cd911";
            files.Add((DirHelper.GetFile("ao-so-mi-4-notused.jfif", Dir.ProductImage), "ao-so-mi-4-notused.jfif"));
            dto.AddedAttributes = new AttributeRequestDTO[] {
                new AttributeRequestDTO { Name = "Chất liệu", Value = "Cotton" },
                new AttributeRequestDTO { Name = "Phong cách", Value = "Đường phố, Công sở" },
            };




            int i;
            Dictionary<string, string?> keyValuePairs = new()
            {
                { "Id", dto.Id },
                { "Name", dto.Name },
                { "Price", dto.Price.ToString() },
                { "Description", dto.Description },
                { "Discount", dto.Discount.ToString() },
                { "Stock", dto.Stock.ToString() }
            };
            foreach (KeyValuePair<string, string?> kvp in keyValuePairs)
            {
                if (kvp.Value != null)
                    content.Add(new StringContent(kvp.Value), kvp.Key);
            }
            if (dto.DeletedAttributes != null)
                for (i = 0; i < dto.DeletedAttributes.Length; i++)
                    content.Add(new StringContent(dto.DeletedAttributes[i].ToString()), $"DeletedAttributes[{i}]");
            if (dto.AddedAttributes != null)
                for (i = 0; i < dto.AddedAttributes.Length; i++)
                {
                    content.Add(new StringContent(dto.AddedAttributes[i].Name), $"AddedAttributes[{i}].Name");
                    content.Add(new StringContent(dto.AddedAttributes[i].Value), $"AddedAttributes[{i}].Value");
                }
            if (dto.DeletedImages != null)
                for (i = 0; i < dto.DeletedImages.Length; i++)
                    content.Add(new StringContent(dto.DeletedImages[i]), $"DeletedImages[{i}]");
            for (i = 0; i < files.Count; i++)
            {
                content.Add(new ByteArrayContent(files[i].Item1), $"AddedImages[{i}].Image", files[i].Item2);
                content.Add(new StringContent(i.ToString()), $"AddedImages[{i}].Position");
            }


            //Act
            HttpResponseMessage res = await _client.RequestFormData(HttpMethod.Put, "api/products", content);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        public async Task Delete()
        {
            //Arrange
            string id;
            id = "8b0d43da-55c1-4f92-bd36-9ee9e91cd911";

            //Act
            HttpResponseMessage res = await _client.DeleteAsync($"/api/products/{id}");

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
        }
    }
}
