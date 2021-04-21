using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using POC_GOOGLE_API.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace POC_GOOGLE_API.Services
{
    public class GoogleService : IGoogleService
    {
        private IConfiguration _config;

        public GoogleService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task GoogleUploadFile(IFormFile file)
        {
            var service = GetGoogleServiceDrive();
            var uploadFolderId = await GetFolderId(service, "TesteUpload");

            if (string.IsNullOrEmpty(uploadFolderId))
                throw new System.Exception("Upload Folder not found!");

            var fileUpload = new Google.Apis.Drive.v3.Data.File();
            fileUpload.Name = file.FileName;
            fileUpload.MimeType = "video/mp4";
            fileUpload.Parents = new List<string>() { uploadFolderId };

            var request = service.Files.Create(fileUpload, file.OpenReadStream(), "video/mp4");
            var eu = await request.UploadAsync();
        }
    
        public async Task<string> GetFolderId(DriveService service, string folderName)
        {
            var files = await service.Files.List().ExecuteAsync();
            var folderObj = files.Files.FirstOrDefault(x => x.Name == folderName && x.MimeType == "application/vnd.google-apps.folder");
            return folderObj != null ? folderObj.Id : string.Empty;
        }

        private DriveService GetGoogleServiceDrive()
        {
            var clientEmail = _config["GoogleServiceAccount:client_email"];
            var privateKey = _config["GoogleServiceAccount:private_key"];

            var cred = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(clientEmail)
            {
                Scopes = new[]
                {
                    DriveService.Scope.Drive,
                    DriveService.Scope.DriveFile,
                    DriveService.Scope.DriveMetadata,
                }
            }.FromPrivateKey(privateKey));

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = cred,
                ApplicationName = "POC GOOGLE API"
            });
        }
    }
}
