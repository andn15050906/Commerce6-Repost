namespace Commerce6.Test.Helpers
{
    internal class DirHelper
    {
        internal static void SetToWebProject()
            => Environment.CurrentDirectory = GetWebProjectDir();

        internal static byte[] GetFile(string name, Dir dir)
            => File.ReadAllBytes(@$"{GetTestProjectDir()}\{GetDir(dir)}\{name}");

        internal static string GetTestProjectDir()
        {
            string dir = Directory.GetCurrentDirectory();
            int endIndex = dir.LastIndexOf(@"Commerce6\") + @"ommerce6\".Length;
            return dir[..endIndex] + @"\Commerce6.Test";
        }






        private static string GetWebProjectDir()
        {
            string dir = Directory.GetCurrentDirectory();
            int endIndex = dir.LastIndexOf(@"Commerce6\") + @"ommerce6\".Length;
            return dir[..endIndex] + @"\Commerce6.Web";
        }

        private static string? GetDir(Dir dir)
        {
            return dir switch
            {
                Dir.Avatar => @"Data\Avatar\",
                Dir.ProductImage => @"Data\ProductImage\",
                Dir.Comment => @"Data\Comment\",
                _ => null
            };
        }
    }
}
