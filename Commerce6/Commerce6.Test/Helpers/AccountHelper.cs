namespace Commerce6.Test.Helpers
{
    internal class AccountHelper
    {
        //Do not call changePassword on these users
        private static readonly List<KeyValuePair<string, string>>? Users = SeedingHelper.ReadGeneratedUsers();

        internal static KeyValuePair<string, string> GetRandomUser()
        {
            if (Users == null)
                throw new Exception("No user found.");
            return Users.ToArray()[new Random().Next(0, Users.Count)];
        }

        internal static KeyValuePair<string, string>[] GetTargetedUsers()
        {
            if (Users == null)
                throw new Exception("No user found.");
            return Users.ToArray();
        }

        internal static string? GetUserPassword(string key)
            => Users?.FirstOrDefault(_ => _.Key == key).Value;
    }
}
