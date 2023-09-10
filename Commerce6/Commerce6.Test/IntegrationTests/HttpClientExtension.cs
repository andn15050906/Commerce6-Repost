using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Commerce6.Web.Models.AppUser.UserDTOs;

namespace Commerce6.Test.IntegrationTests
{
    public static class HttpClientExtension
    {
        public static void GetAuthHeaders(this HttpClient client, string phoneOrEmail, string password)
        {
            LoginDTO dto = new() { PhoneOrEmail = phoneOrEmail, Password = password };
            SetCookieHeaderValue? bearer = GetBearer(client, dto).Result;
            if (bearer == null)
                throw new Exception("Error logging in");
            client.DefaultRequestHeaders.Remove("Cookie");
            client.DefaultRequestHeaders.Add("Cookie",
                new CookieHeaderValue(bearer!.Name.ToString(), bearer!.Value.ToString()).ToString());
        }

        public static HttpResponseMessage GetShopAuthHeaders(this HttpClient client)
        {
            HttpResponseMessage response = client.PostAsync("/api/auth/LoginMerchant", null).Result;
            return response;
        }

        public static async Task<HttpResponseMessage> RequestJson(this HttpClient client, HttpMethod method, string uri, string content)
        {
            HttpRequestMessage request = new(method, uri)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            return await client.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> RequestFormData(this HttpClient client, HttpMethod method, string uri, MultipartFormDataContent content)
        {
            HttpRequestMessage request = new(method, uri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }






        private static async Task<SetCookieHeaderValue?> GetBearer(HttpClient client, LoginDTO dto)
        {
            HttpResponseMessage response = await client.RequestJson(HttpMethod.Post, "/api/auth/Login",  JsonSerializer.Serialize(dto));

            if (!response.IsSuccessStatusCode)
                return null;

            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string>? values))
            {
                SetCookieHeaderValue? bearer = SetCookieHeaderValue.ParseList(values.ToList())
                    .SingleOrDefault(c => c.Name.StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase));
                if (bearer != null)
                    return bearer;
            }
            return null;
        }
    }
}
