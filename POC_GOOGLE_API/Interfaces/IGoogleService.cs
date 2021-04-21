using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace POC_GOOGLE_API.Interfaces
{
    public interface IGoogleService
    {
        Task GoogleUploadFile(IFormFile file);
    }
}
