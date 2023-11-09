using Microsoft.AspNetCore.SignalR;
using System.IO;
using System.Runtime.CompilerServices;

namespace TransRoza.WebApi.Hubs
{
    public class UploadHub : Hub<IUploader>
    {
        public async IAsyncEnumerable<double> Test(int loops, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            for (int i = 0; i < loops; i++)
            {
                yield return i;
                await Task.Delay(1000);
            }
        }

        public async IAsyncEnumerable<double> UploadStream(IAsyncEnumerable<byte[]> byteStream,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var nextPing = DateTime.Now.AddSeconds(1);

            var fi = new FileInfo("/data/upload/test.zip");
            Directory.CreateDirectory(fi.Directory!.FullName);

            if (fi.Exists)
            {
                fi.Delete();
            }

            using (var fs = fi.Create())
            {
                await foreach (var bytes in byteStream)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await fs.WriteAsync(bytes, 0, bytes.Length);

                    if (nextPing < DateTime.Now)
                    {
                        nextPing = DateTime.Now.AddSeconds(1);
                        yield return 12;
                    }
                }
            }
        }
    }

    public interface IUploader
    {
        Task SendProgress(double percent, string message);
    }
}
