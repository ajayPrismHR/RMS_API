namespace RMS_API.Models.QueryModel
{
    public class UserDetailModel
    {
        public Int64 ID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string MOBILENO { get; set; }
        public string MAILID { get; set; }
        public string ADDRESS { get; set; }
        public string AREA_CODE { get; set; }
        public string IMAGE { get; set; }
        public string GENDER { get; set; }

        public DateTime DOB { get; set; }
    }
}
