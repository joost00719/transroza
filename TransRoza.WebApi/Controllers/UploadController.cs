using Microsoft.AspNetCore.Mvc;
using TransRoza.Shared;

namespace TransRoza.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        [HttpPost(Name = "CreateHandle")]
        public Guid CreateFileHandle(RozaFileInformation fileInfo)
        {
            fileInfo.Handle = Guid.NewGuid();

            return fileInfo.Handle.Value;
        }

        [HttpPatch(Name = "UploadChunk/{handle}/{beginInclusive}/{endInclusive}")]
        public async Task<IActionResult> UploadChunk([FromQuery] Guid handle, [FromQuery] int beginInclusive, [FromBody] Stream data)
        {
            Directory.CreateDirectory("/data/uploads");

            using (var fs = System.IO.File.OpenWrite($@"/data/uploads/{handle}.txt"))
            {
                fs.Seek(beginInclusive, SeekOrigin.Begin);
                await data.CopyToAsync(fs);
                await fs.FlushAsync();
            }

            return Ok();
        }
    }
}
