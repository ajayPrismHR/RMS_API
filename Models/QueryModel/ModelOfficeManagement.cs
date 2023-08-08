namespace SURAKSHA_API.Models.QueryModel
{
    public class ModelOfficeManagement
    {
        public Int64 OFFICE_ID { get; set; }
        public Int64 PARENT_OFFICE_ID { get; set; }
        public string OFFICE_NAME { get; set; }
        public string OFFICE_ADDRESS { get; set; }
        public string OFFICE_TYPE { get; set; }
        public string OFFICE_MOBILE_NO { get; set; }
        public string OFFICE_EMAIL_ID { get; set; }
        public string LATITIDE { get; set; }
        public string LONGITUDE { get; set; }
        public string ACTION { get; set; }

    }
}
