using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace TransRoza.FileHandling
{
    public static class FilePermissionHelper
    {
        public static void RemoveExecutePermission(string filePath)
        {
            // TODO Figure out why this does not work

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty.");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var fileInfo = new FileInfo(filePath);

            // Check if the operating system is Windows.
            if (OperatingSystem.IsWindows())
            {
                var fileSecurity = fileInfo.GetAccessControl();

                // Remove execute permissions for everyone.
                fileSecurity.RemoveAccessRule(new FileSystemAccessRule(
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                    FileSystemRights.ExecuteFile, AccessControlType.Allow));

                fileInfo.SetAccessControl(fileSecurity);
            }
            // Check if the operating system is Linux or Unix.
            else if (OperatingSystem.IsLinux() || IsUnixLikePlatform())
            {
                // Linux/Unix specific code to remove execute permissions using 'chmod'.
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "chmod";
                process.StartInfo.Arguments = "-x " + filePath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
            }
            else
            {
                throw new NotSupportedException("This operating system is not supported.");
            }
        }

        private static bool IsUnixLikePlatform()
        {
            int platform = (int)Environment.OSVersion.Platform;
            return (platform == 4) || (platform == 6) || (platform == 128); // Chat-gpt recommended this. not sure if it works.
        }
    }
}
