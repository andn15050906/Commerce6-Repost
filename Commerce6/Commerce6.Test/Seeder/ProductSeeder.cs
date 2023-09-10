using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Commerce6.Web.Models.Merchant.AttributeDTOs;
using Commerce6.Web.Models.Merchant.ProductDTOs;
using Commerce6.Test.IntegrationTests;
using Commerce6.Test.Helpers;

namespace Commerce6.Test.Seeder
{
    public class ProductSeeder : IClassFixture<WebApplicationFactory<Program>>
    {
        private class Data
        {
            public KeyValuePair<string, string> User { get; set; }
            public CreateProductDTO Dto { get; set; }
            public List<(byte[], string)> Images { get; set; }
        }

        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public ProductSeeder(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            DirHelper.SetToWebProject();

            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Seed()
        {
            List<Data> data = PrepareData();
            MultipartFormDataContent content;
            Dictionary<string, string?> kvps;
            int i;

            foreach (Data product in data)
            {
                //Arrange
                _client.GetAuthHeaders(product.User.Key, product.User.Value);
                _client.GetShopAuthHeaders();

                CreateProductDTO dto = product.Dto;
                List<(byte[], string)> images = product.Images;

                content = new();
                kvps = new()
                {
                    { "Name", dto.Name },
                    { "Price", dto.Price.ToString() },
                    { "Description", dto.Description },
                    { "Discount", dto.Discount.ToString() },
                    { "Stock", dto.Stock.ToString() },
                    { "ShopCategoryId", dto.ShopCategoryId.ToString() },
                    { "CategoryId", dto.CategoryId.ToString() }
                };

                foreach (KeyValuePair<string, string?> kvp in kvps)
                    if (kvp.Value != null)
                        content.Add(new StringContent(kvp.Value), kvp.Key);
                if (dto.Attributes != null)
                    for (i = 0; i < dto.Attributes.Length; i++)
                    {
                        content.Add(new StringContent(dto.Attributes[i].Name), $"Attributes[{i}].Name");
                        content.Add(new StringContent(dto.Attributes[i].Value), $"Attributes[{i}].Value");
                    }
                for (i = 0; i < images.Count; i++)
                {
                    content.Add(new ByteArrayContent(images[i].Item1), $"Images[{i}].Image", images[i].Item2);
                    content.Add(new StringContent(i.ToString()), $"Images[{i}].Position");
                }

                //Act
                HttpResponseMessage res = await _client.RequestFormData(HttpMethod.Post, "api/products", content);
                
                //Assert
                _output.WriteLine(res.StatusCode.ToString());
                _output.WriteLine(res.Content.ReadAsStringAsync().Result);
            }
        }

        private List<Data> PrepareData()
        {
            //not yet shopCategoryId

            List<Data> data = new();

            data.Add(new Data
            {
                User = AccountHelper.GetRandomUser(),
                //https://shopee.vn/%C3%81o-s%C6%A1-mi-ng%E1%BA%AFn-tay-form-r%E1%BB%99ng-th%E1%BB%9Di-trang-hi%E1%BB%87n-%C4%91%E1%BA%A1i-unisex-ch%E1%BA%A5t-li%E1%BB%87u-v%E1%BA%A3i-l%E1%BB%A5a-m%E1%BB%81m-ch%E1%BB%91ng-nh%C4%83n-i.289383259.10816146455?sp_atk=4924474c-e809-453d-9ac4-a607c9a344d2&xptdk=4924474c-e809-453d-9ac4-a607c9a344d2
                Dto = new CreateProductDTO
                {
                    Name = "Áo sơ mi ngắn tay form rộng, thời trang hiện đại unisex chất liệu vải lụa mềm chống nhăn",
                    Price = 199000,
                    Description = "ÁO SƠ MI NGẮN TAY FORM RỘNG, THỜI TRANG HIỆN ĐẠI UNISEX CHẤT LIỆU VẢI LỤA MỀM CHỐNG NHĂN\r\n-\tChất liệu: Lụa hàn.\r\n-\tCông dụng: Chống nhăn, giãn nhẹ, êm ái, mềm mịn và mát da.\r\n-\tPhong cách: Unisex, Form rộng, Sweetwear.\r\n-\tDành cho: Nam và Nữ.\r\n-\tXu xướng: Hiện đại 2023.\r\n-\tXuất xứ: Made in Việt Nam.\r\n-\tSize : M L XL XXL.\r\n•\tM : Dưới 55kg, Cao dưới 1m65.\r\n•\tL : 55 - 65kg, Cao 1m65 – 1m72.\r\n•\tXL: 65 - 75kg, Cao 1m68 – 1m75.\r\n•\tXXL: 75 - 85kg, Cao 1m70 – 1m80.\r\nÁO SƠ MI NGẮN TAY FORM RỘNG, THỜI TRANG HIỆN ĐẠI UNISEX CHẤT LIỆU VẢI LỤA MỀM CHỐNG NHĂN\r\n-\tQuần short -> Tạo nên phong cách vô cùng đơn giản nhưng không kém phần cuốn hút. Đặc biệt mang đến cảm giác thoải mái cho người mặc.",
                    Discount = 0.49,
                    Stock = 32255,
                    CountUnit = "Cái",
                    Attributes = new AttributeRequestDTO[]
                    {
                        new AttributeRequestDTO { Name = "Dáng kiểu áo", Value = "Rộng" },
                        new AttributeRequestDTO { Name = "Cổ áo", Value = "Cổ sơ mi" },
                        new AttributeRequestDTO { Name = "Kiểu cổ áo", Value = "Cổ áo truyền thống" },
                        new AttributeRequestDTO { Name = "Phong cách", Value = "Thể thao, Cơ bản, Đường phố, Công sở" },
                        new AttributeRequestDTO { Name = "Xuất xứ", Value = "Việt Nam" },
                        new AttributeRequestDTO { Name = "Mẫu", Value = "Khác, Trơn" }
                    },
                    CategoryId = 2
                },
                Images = new List<(byte[], string)>
                {
                    (DirHelper.GetFile("ao-so-mi-1.jfif", Dir.ProductImage), "ao-so-mi-1.jfif"),
                    (DirHelper.GetFile("ao-so-mi-2.jfif", Dir.ProductImage), "ao-so-mi-2.jfif")
                }
            });

            data.Add(new Data
            {
                User = AccountHelper.GetRandomUser(),
                //https://shopee.vn/%C3%81O-KHO%C3%81C-NH%E1%BA%B8-NAM-N%E1%BB%AE-2-L%E1%BB%9AP-THU-%C4%90%C3%94NG-FOM-R%E1%BB%98NG-H%E1%BB%8CA-TI%E1%BA%BET-PH%E1%BB%90I-VI%E1%BB%80N-S%E1%BB%8CC-TAY-%C4%90%C6%A0N-GI%E1%BA%A2N-H%C3%93T-TRIEND-2021-i.17475845.13836258622?sp_atk=77293b3f-07e7-47eb-a97a-c8ee74b51dd1&xptdk=77293b3f-07e7-47eb-a97a-c8ee74b51dd1
                Dto = new CreateProductDTO
                {
                    Name = "ÁO KHOÁC NHẸ NAM NỮ 2 LỚP THU ĐÔNG FOM RỘNG HỌA TIẾT PHỐI VIỀN SỌC TAY ĐƠN GIẢN HÓT TRIEND 2021",
                    Price = 158000,
                    Description = "Bên trong áo có lót dù thoáng khí mát mẻ, tạo cảm giác vận động thoải mái khi mặc.\r\nĐường chỉ may sắc sảo, cực kì chắc chắn & 2 lớp dày dặn.\r\nĐảm bảo không ra màu, không phai màu & không nhăn.\r\nDễ dàng Mix cùng nhiều kiểu trang phục yêu thích hằng ngày & tự tin đi đến mọi nơi mọi lúc với phong cách chất lừ của riêng bạn.\r\nHàng tại xưởng không qua trung gian, Hàng luôn có sẵn.\r\nKhách mua hàng được tặng các phần quà bất ngờ cho các đơn hàng ngẫu nhiên đặc biệt.\r\nHàng chuẩn đẹp như hình (cam kết hình chụp thật 100%)..\r\nShop luôn sẵn lòng hỗ trợ tư vấn giải đáp thắc mặc của bạn khi cần.\r\nXuất xứ: Việt Nam\r\n\r\n\r\nSIZE GỒM 3 SIZE : M ,L ,XL\r\n\r\n\r\nsize M : dành cho những bạn từ 43 đến 49kg < 1m 60\r\n\r\n\r\nsize L : dành cho những bạn từ 50 đến 58kg <1m7\r\n\r\n\r\nsize xl :dành cho những bạn từ 58 đến 69kg < 1m 76\r\nshop : ao.khoac.nam.nu.gia.si\r\nđịa chỉ 17/9a/6 đường 22 kp7 linh đông thủ đức hồ chí minh\r\nxuất xứ việt nam ",
                    Discount = 0.4,
                    Stock = 24287,
                    CountUnit = "Cái",
                    Attributes = new AttributeRequestDTO[]
                    {
                        new AttributeRequestDTO { Name = "Chất liệu", Value = "Nylon" },
                        new AttributeRequestDTO { Name = "Mẫu", Value = "Sọc" },
                        new AttributeRequestDTO { Name = "Phong cách", Value = "Thể thao, Boho, Đường phố" },
                        new AttributeRequestDTO { Name = "Tall Fit", Value = "Có" },
                        new AttributeRequestDTO { Name = "Xuất xứ", Value = "Việt Nam" }
                    },
                    CategoryId = 2
                },
                Images = new List<(byte[], string)>
                {
                    (DirHelper.GetFile("ao-so-mi-3.jfif", Dir.ProductImage), "ao-so-mi-3.jfif"),
                    (DirHelper.GetFile("ao-so-mi-3p1.jfif", Dir.ProductImage), "ao-so-mi-3p1.jfif")
                }
            });

            data.Add(new Data
            {
                User = AccountHelper.GetRandomUser(),
                Dto = new CreateProductDTO
                {
                    Name = "QUẦN JEANS NAM ỐNG RỘNG GÂN SÓC XANH LƯỢN VINTAGE TAY LOOSE FIT PHỐI DÂY CHUYỀN",
                    Price = 450000,
                    Description = "Kiểu dáng quần jeans nam ống rộng gân sóc hiện đang được ưa chuộng bởi sự thoải mái và phong cách đầy cá tính. Chất liệu vải denim cao cấp, thấm hút mồ hôi tốt, giúp bạn cảm thấy thoải mái khi mặc. Quần được thiết kế với kiểu dáng tay loose fit phối dây chuyền trẻ trung, năng động. Màu xanh lươn vintage đậm chất điệu đà, phù hợp cho những buổi gặp mặt bạn bè hay đi chơi cùng gia đình.\r\nSize S: dành cho những bạn từ 45 đến 55kg < 1m7\r\nSize M : dành cho những bạn từ 55 đến 65kg < 1m75\r\nSize L : dành cho những bạn từ 65 đến 75kg < 1m8\r\n",
                    Discount = 0.1,
                    Stock = 503,
                    CountUnit = "Cái",
                    Attributes = new AttributeRequestDTO[]
                    {
                        new AttributeRequestDTO { Name = "Chất liệu", Value = "Denim" },
                        new AttributeRequestDTO { Name = "Mẫu", Value = "Gân sóc" },
                        new AttributeRequestDTO { Name = "Phong cách", Value = "Thể thao, Vintage, Đường phố" },
                        new AttributeRequestDTO { Name = "Tall Fit", Value = "Không" },
                        new AttributeRequestDTO { Name = "Xuất xứ", Value = "Việt Nam" }
                    },
                    CategoryId = 3
                },
                Images = new List<(byte[], string)>
                {
                    (DirHelper.GetFile("quan-nam-1.jfif", Dir.ProductImage), "quan-nam-1.jfif"),
                    (DirHelper.GetFile("quan-nam-2.jfif", Dir.ProductImage), "quan-nam-2.jfif")
                }
            });

            data.Add(new Data
            {
                User = AccountHelper.GetRandomUser(),
                Dto = new CreateProductDTO
                {
                    Name = "ÁO KHOÁC DẠ NAM NỮ 2 LỚP ĐÔI FOM ÔM CỔ TÀU HỌA TIẾT TRẺ TRUNG 2023",
                    Price = 450000,
                    Description = "Bên trong áo có lót dù thoáng khí mát mẻ, giữ ấm tốt khi trời lạnh.\r\nĐường chỉ may sắc sảo, cực kì chắc chắn & 2 lớp dày dặn.\r\nChất liệu dạ cao cấp, mềm mại và bền đẹp.\r\nDễ dàng Mix cùng nhiều kiểu trang phục yêu thích hằng ngày & tự tin đi đến mọi nơi mọi lúc với phong cách chất lừ của riêng bạn.\r\nHàng tại xưởng không qua trung gian, Hàng luôn có sẵn.\r\nKhách mua hàng được tặng các phần quà bất ngờ cho các đơn hàng ngẫu nhiên đặc biệt.\r\nHàng chuẩn đẹp như hình (cam kết hình chụp thật 100%)..\r\nShop luôn sẵn lòng hỗ trợ tư vấn giải đáp thắc mặc của bạn khi cần.\r\nXuất xứ: Việt Nam\r\n\r\n\r\nSIZE GỒM 3 SIZE : M ,L ,XL\r\n\r\n\r\nsize M : dành cho những bạn từ 50 đến 60kg < 1m7\r\n\r\n\r\nsize L : dành cho những bạn từ 61 đến 70kg <1m78\r\n\r\n\r\nsize xl :dành cho những bạn từ 71 đến 80kg < 1m 85\r\nshop : ao.khoac.danh.cho.nam.va.nu\r\nđịa chỉ 123/4 đường ABC kp9 P.QWE TPHCM\r\nxuất xứ việt nam ",
                    Discount = 0.25,
                    Stock = 500,
                    CountUnit = "Cái",
                    Attributes = new AttributeRequestDTO[]
                    {
                        new AttributeRequestDTO { Name = "Chất liệu", Value = "Dạ cao cấp" },
                        new AttributeRequestDTO { Name = "Mẫu", Value = "Họa tiết" },
                        new AttributeRequestDTO { Name = "Phong cách", Value = "Streetwear, Vintage, Urban" },
                        new AttributeRequestDTO { Name = "Tall Fit", Value = "Không" },
                        new AttributeRequestDTO { Name = "Xuất xứ", Value = "Việt Nam" }
                    },
                    CategoryId = 2
                },
                Images = new List<(byte[], string)>
                {
                    (DirHelper.GetFile("aokhoac-namnu-1.jfif", Dir.ProductImage), "aokhoac-namnu-1.jfif"),
                    (DirHelper.GetFile("aokhoac-namnu-2.jfif", Dir.ProductImage), "aokhoac-namnu-2.jfif"),
                    (DirHelper.GetFile("aokhoac-namnu-3.jfif", Dir.ProductImage), "aokhoac-namnu-3.jfif")
                }
            });

            return data;
        }
    }
}
