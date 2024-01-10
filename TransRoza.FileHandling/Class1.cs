using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TransRoza.FileHandling
{
    public class Class1
    {
        private DirectoryInfo _dir;

        public Class1(FileHandlingSettings settings)
        {
            _dir = new DirectoryInfo(settings.FileStoragePath);
            _dir.Create();
            var tempdir = _dir.CreateSubdirectory("tmp");
            tempdir.Delete(true);
        }

        public async Task<string> SaveFileAsync(Stream stream, string fileName)
        {
            var tempdir = _dir.CreateSubdirectory("tmp");
            var tempFile = Path.Combine(tempdir.FullName, fileName);
            using (var fileStream = File.Create(tempFile))
            {
                await stream.CopyToAsync(fileStream);
            }
            var finalFile = Path.Combine(_dir.FullName, fileName);
            File.Move(tempFile, finalFile);
            return finalFile;
        }

        public FileStream Test(string fileName)
        {
            var filePath = Path.Combine(_dir.FullName, fileName);
            var fs = File.Create(filePath);

            FilePermissionHelper.RemoveExecutePermission(filePath);

            return fs;
        }
    }
}
