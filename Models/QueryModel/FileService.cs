using Azure.Storage.Blobs;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.WindowsAzure.Storage.Blob;

namespace RMS_API.Models.QueryModel
{
    public class FileService : IFileService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public FileService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
       
        public Task<string> GetFile(string imgfileName)
        {
            var containerInstance = _blobServiceClient.GetBlobContainerClient("punchmateappimg");
            var blobInstance = containerInstance.GetBlobClient(imgfileName);
            string a = blobInstance.Uri.AbsoluteUri;
            return Task.FromResult(a);

        }
        
          
                         
        public async Task Upload(ModelFile modelFile, string imgfileName  )
        {
            var containerInstance = _blobServiceClient.GetBlobContainerClient("punchmateappimg");
            var blobInstance = containerInstance.GetBlobClient(imgfileName);
            await blobInstance.UploadAsync(modelFile.ImageFile.OpenReadStream());
        }
    }
}
