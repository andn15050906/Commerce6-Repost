using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Commerce6.Web.Models.Contact.CommentDTOs;
using Commerce6.Test.Helpers;

namespace Commerce6.Test.IntegrationTests.Contact
{
    public class CommentTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public CommentTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            string dir = Directory.GetCurrentDirectory();
            int end = dir.LastIndexOf(@"Commerce6\");
            string path = dir.Substring(0, end + @"ommerce6\".Length) + @"\Commerce6.Web";
            Environment.CurrentDirectory = path;

            _output = output;
            _client = factory.CreateClient();

            //login before testing
            _client.GetAuthHeaders("Phanquynhngan575@gmail.com", "Phanquynhngan575");
        }

        //[Fact]
        public async Task Create()
        {
            //Arrange
            MultipartFormDataContent content = new();
            CreateCommentDTO dto = new();
            List<(byte[], string)> files = new();

            dto.ProductId = "21d6a2db-7d20-40b9-a37b-d863a478b365";
            dto.Content = "Áo giao nhanh, chất vải tạm ổn, mỗi tội có điều là ko còn màu đen vs áo nhỏ, nên thôi đành cho cháu mặc vậy\r\n";
            files.Add((DirHelper.GetFile("comment-2.jfif", Dir.Comment), "comment-2.jfif"));
            files.Add((DirHelper.GetFile("comment-3.jfif", Dir.Comment), "comment-3.jfif"));







            Dictionary<string, string?> keyValuePairs = new()
            {
                { "ProductId", dto.ProductId },
                { "Parent", dto.Parent.ToString() },
                { "Content", dto.Content }
            };
            foreach (KeyValuePair<string, string?> kvp in keyValuePairs)
                if (kvp.Value != null)
                    content.Add(new StringContent(kvp.Value), kvp.Key);
            foreach ((byte[], string) file in files)
                content.Add(new ByteArrayContent(file.Item1), $"Files", file.Item2);



            //Act
            HttpResponseMessage res = await _client.RequestFormData(HttpMethod.Post, "api/comments", content);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        public async Task Update()
        {
            //Arrange
            MultipartFormDataContent content = new();
            UpdateCommentDTO dto = new();
            List<(byte[], string)> files = new();
            int i;

            /*dto.Id = 4;
            dto.Hidden = true;
            dto.Content = "Updated";
            dto.DeletedFiles = new[] { "de92a746-b8ba-470b-a047-4db74ebec9ab_comment-2-updated.jfif", "cf0c7133-b58b-42c5-b171-a4fc6d82af11_comment-3-updated.jfif" };
            files.Add((FileHelper.GetFile("comment-2.jfif", Dir.Comment), "comment-2-updated.jfif"));
            files.Add((FileHelper.GetFile("comment-3.jfif", Dir.Comment), "comment-3-updated.jfif"));*/

            dto.Id = 3;
            dto.Content = "Rất mong bạn sẽ tiếp tục ủng hộ Shop!";


            Dictionary<string, string?> keyValuePairs = new()
            {
                { "Id", dto.Id.ToString() },
                { "Hidden", dto.Hidden.ToString() },
                { "Content", dto.Content }
            };
            foreach (KeyValuePair<string, string?> kvp in keyValuePairs)
            {
                if (kvp.Value != null)
                    content.Add(new StringContent(kvp.Value), kvp.Key);
            }
            if (dto.DeletedFiles != null)
                for (i = 0; i < dto.DeletedFiles.Length; i++)
                    content.Add(new StringContent(dto.DeletedFiles[i].ToString()), $"DeletedFiles[{i}]");
            for (i = 0; i < files.Count; i++)
                content.Add(new ByteArrayContent(files[i].Item1), $"AddedFiles", files[i].Item2);



            //Act
            HttpResponseMessage res = await _client.RequestFormData(HttpMethod.Put, "api/comments", content);

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
            _output.WriteLine(res.Content.ReadAsStringAsync().Result);
        }

        //[Fact]
        public async Task Delete()
        {
            //Arrange
            int id = 3;

            //Act
            HttpResponseMessage res = await _client.DeleteAsync($"/api/comments/{id}");

            //Assert
            _output.WriteLine(res.StatusCode.ToString());
        }
    }
}
