using Commerce6.Test.Helpers;
using Commerce6.Test.IntegrationTests;
using Commerce6.Web.Models.Contact.CommentDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Commerce6.Test.Seeder
{
    public class CommentSeeder : IClassFixture<WebApplicationFactory<Program>>
    {
        private class Data
        {
            public KeyValuePair<string, string> User { get; set; }
            public CreateCommentDTO Dto { get; set; }
            public List<(byte[], string)>? Files { get; set; }
        }

        private string[] ContentLst = new string[]
        {
            "Áo giao nhanh, chất vải tạm ổn, mỗi tội có điều là ko còn màu đen vs áo nhỏ, nên thôi đành cho cháu mặc vậy\r\n"
        };

        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public CommentSeeder(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Seed()
        {
            List<Data> data = PrepareData();
            MultipartFormDataContent content;
            Dictionary<string, string?> kvps;

            foreach (Data item in data)
            {
                _client.GetAuthHeaders(item.User.Key, item.User.Value);

                content = new();
                kvps = new()
                {
                    { "ProductId", item.Dto.ProductId },
                    { "Parent", item.Dto.Parent.ToString() },
                    { "Content", item.Dto.Content }
                };

                foreach (KeyValuePair<string, string?> kvp in kvps)
                    if (kvp.Value != null)
                        content.Add(new StringContent(kvp.Value), kvp.Key);
                if (item.Files != null)
                    foreach ((byte[], string) file in item.Files)
                        content.Add(new ByteArrayContent(file.Item1), $"Files", file.Item2);

                //Act
                HttpResponseMessage res = await _client.RequestFormData(HttpMethod.Post, "api/comments", content);

                //Assert
                _output.WriteLine(res.StatusCode.ToString());
                _output.WriteLine(res.Content.ReadAsStringAsync().Result);
            }
        }

        private List<Data> PrepareData()
        {
            List<Data> data = new();

            List<OrderSeeder.Data>? lst = SeedingHelper.ReadGeneratedOrders();
            if (lst == null)
                throw new Exception("No order found.");
            Random rd = new();

            foreach (OrderSeeder.Data item in lst)
            {
                data.Add(new Data
                {
                    User = new(item.CustomerName, AccountHelper.GetUserPassword(item.CustomerName)!),
                    Dto = new ()
                    {
                        ProductId = item.ProductIds.First(),
                        Parent = null,
                        Content = ContentLst[rd.Next(ContentLst.Length)]
                    },
                    Files = null
                });
            }

            data[0].Files = new List<(byte[], string)>
            {
                (DirHelper.GetFile("comment-2.jfif", Dir.Comment), "comment-2.jfif"),
                (DirHelper.GetFile("comment-3.jfif", Dir.Comment), "comment-3.jfif")
            };

            return data;
        }
    }
}
