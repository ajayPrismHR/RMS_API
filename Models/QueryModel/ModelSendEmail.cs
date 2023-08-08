namespace SURAKSHA_API.Models.QueryModel
{
    public class ModelSendEmail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string attachment_path { get; set; }
    }
}
