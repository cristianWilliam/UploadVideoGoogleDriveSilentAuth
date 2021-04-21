using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POC_GOOGLE_API.Interfaces;
using System.Threading.Tasks;

namespace POC_GOOGLE_API.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        [HttpPost]
        public async Task UploadDriveFile(
            [FromForm] IFormFile file,
            [FromServices] IGoogleService service)
        {
            await service.GoogleUploadFile(file);
        }
    }
}
