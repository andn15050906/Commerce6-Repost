using Commerce6.Infrastructure.Models.Merchant;
using System.Net.Http.Json;

namespace Commerce6.Test.Helpers
{
    internal class ProductHelper
    {
        internal static async Task<List<ProductDTO>?> GetProducts(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync("/api/products?FromDate=2023-03-01");
            return await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
        }
    }
}
