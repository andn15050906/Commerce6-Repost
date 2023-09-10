using Serilog;

namespace Commerce6.Web.Helpers.ServerFile
{
    public class FileHelper
    {
        /// <summary>
        /// Add a GUID to filename, separated by a '_'
        /// Client process FileName -> RetrieveFileName(string guidName) => guidName.Substring(0, guidName.LastIndexOf('_'));
        /// </summary>
        public async Task<string?> SaveFile(IFormFile? file, Dir dir)
        {
            if (file == null)
                return null;

            string guidName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string path = @$"{GetDir(dir)}\{guidName}";
            using (var stream = File.Create(path))
            {
                Log.Information("Saving " + path);
                await file.CopyToAsync(stream);
            }
            return guidName;
        }

        public async Task<string[]?> SaveFiles(IFormFile[]? files, Dir dir)
        {
            if (files == null || files.Length == 0)
                return null;
            string[] paths = new string[files.Length];
            int i;

            Task<string?>[] tasks = new Task<string?>[files.Length];
            for (i = 0; i < files.Length; i++)
                tasks[i] = SaveFile(files[i], dir);

            string?[] fileNames = await Task.WhenAll(tasks);
            for (i = 0; i < fileNames.Length; i++)
                paths[i] = fileNames[i]!;
            return paths;
        }

        public async Task DeleteFile(string guidName, Dir dir)
        {
            if (guidName == null)
                return;

            string path = @$"{GetDir(dir)}\{guidName}";
            try
            {
                Log.Information("Deleting " + path);
                //put the task to another thread
                await Task.Run(() => File.Delete(path));
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
            }
        }

        public async Task DeleteFiles(string[] guidNames, Dir dir)
        {
            if (guidNames == null || guidNames.Length == 0)
                return;
            Task[] tasks = new Task[guidNames.Length];
            int i;

            for (i = 0; i < guidNames.Length; i++)
                tasks[i] = DeleteFile(guidNames[i], dir);
            await Task.WhenAll(tasks);
        }






        private static string? GetDir(Dir dir)
        {
            return dir switch
            {
                Dir.User => @"wwwroot\upload-user",
                Dir.Shop => @"wwwroot\upload-shop",
                Dir.Product => @"wwwroot\upload-product",
                Dir.Comment => @"wwwroot\upload-comment",
                _ => null
            };
        }
    }
}
