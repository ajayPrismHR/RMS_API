using Azure.Storage.Blobs;

namespace RMS_API.Models.QueryModel
{
    public class FileService : IFileService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public FileService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Upload(ModelFile modelFile, string imgfileName  )
        {
            var containerInstance = _blobServiceClient.GetBlobContainerClient("punchmateappimg");
            var blobInstance = containerInstance.GetBlobClient(imgfileName);
            await blobInstance.UploadAsync(modelFile.ImageFile.OpenReadStream());
        }
    }
}
