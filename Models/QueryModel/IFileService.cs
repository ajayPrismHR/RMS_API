namespace RMS_API.Models.QueryModel
{
    public interface IFileService
    {
        Task Upload(ModelFile modelFile , string imgfileName);

        Task<string> GetFile(string imgfileName);

    }
}
