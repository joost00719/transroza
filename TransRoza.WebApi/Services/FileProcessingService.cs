using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.AccessControl;
using TransRoza.Shared;

namespace TransRoza.WebApi.Services
{
    public class FileProcessingService
    {
        private DirectoryInfo DestinationDir { get; set; }
        private DirectoryInfo WorkingDirectory { get; set; }

        public FileProcessingService()
        {
            DestinationDir = Directory.CreateDirectory(Path.Combine("data", "tmp"));
            WorkingDirectory = Directory.CreateDirectory(Path.Combine("data", "uploads"));
        }

        public Guid CreateFileHandle(RozaFileInformation fileInfo)
        {
            if (fileInfo is null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            fileInfo.Handle = Guid.NewGuid();
            var fi = GetWorkingFileInfo(fileInfo.Handle.Value);

            // Create a file and close it to ensure it exists
            File.Create(fi.FullName).Close();

            return fileInfo.Handle.Value;
        }

        public async Task UploadChunkAsync(Guid handle, int beginInclusive, Stream data)
        {
            var fi = GetWorkingFileInfo(handle);
            if (!fi.Exists)
            {
                throw new FileNotFoundException(string.Format("No file found at '{0}'", fi.FullName));
            }

            using (var fs = fi.OpenWrite())
            {
                fs.Seek(beginInclusive, SeekOrigin.Begin);
                await data.CopyToAsync(fs);
                await fs.FlushAsync();
            }
        }

        private FileInfo GetWorkingFileInfo(Guid handle)
        {
            var pathData = Path.Combine(WorkingDirectory.FullName, $"{handle}");
            var pathMeta = Path.Combine(WorkingDirectory.FullName, $"{handle}-metadata.json");
            return new FileInfo(pathData);
        }
    }
}
