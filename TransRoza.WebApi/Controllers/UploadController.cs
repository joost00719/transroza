using Microsoft.AspNetCore.Mvc;
using TransRoza.Shared;
using TransRoza.WebApi.Services;

namespace TransRoza.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        public UploadController(FileProcessingService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }

        private readonly FileProcessingService fileUploadService;

        [HttpPost]
        [Route("CreateHandle")]
        public ActionResult<Guid> CreateFileHandle(RozaFileInformation fileInfo)
        {
            if (fileInfo is null)
            {
                return BadRequest();
            }

            fileUploadService.CreateFileHandle(fileInfo);

            return fileInfo.Handle.Value;
        }

        [HttpPatch]
        [Route("UploadChunk/{handle}/{beginInclusive}")]
        public async Task<IActionResult> UploadChunk([FromQuery] Guid handle, [FromQuery] int beginInclusive, [FromBody] Stream data)
        {
            try
            {
                await fileUploadService.UploadChunkAsync(handle, beginInclusive, data);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
