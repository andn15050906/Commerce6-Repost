using Commerce6.Test.Seeder;
using System.Text.Json;

namespace Commerce6.Test.Helpers
{
    internal class SeedingHelper
    {
        private static readonly string USER_FILE = @$"{DirHelper.GetTestProjectDir()}\Data\Generated\Users.json";
        private static readonly string ORDER_FILE = @$"{DirHelper.GetTestProjectDir()}\Data\Generated\Orders.json";

        internal static void SaveGeneratedUsers(List<KeyValuePair<string, string>> users)
            => File.WriteAllText(USER_FILE, JsonSerializer.Serialize(users));

        internal static List<KeyValuePair<string, string>>? ReadGeneratedUsers()
            => JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(File.ReadAllText(USER_FILE));

        internal static void SaveGeneratedOrders(List<OrderSeeder.Data> orders)
            => File.WriteAllText(ORDER_FILE, JsonSerializer.Serialize(orders));

        internal static List<OrderSeeder.Data>? ReadGeneratedOrders()
            => JsonSerializer.Deserialize<List<OrderSeeder.Data>>(File.ReadAllText(ORDER_FILE));
    }
}
