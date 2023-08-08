namespace SURAKSHA_API.Models.QueryModel
{
    public class ModelCreateIncident
    {
        public int INCIDENT_CATEGORY { get; set; }
        public int INCIDENT_SUBCATEGORY { get; set; }
        public int INCIDENT_PRIORITY { get; set; }
        public string INCIDENT_DESCRIPTION { get; set; }
        public string INCIDENT_LANDMARK { get; set; }
        public string INCIDENT_NEAREST_POLE_NO { get; set; }
        public string HAS_SOME_INJURY { get; set; }
        public Int64 ASSIGNEE_OFFICE { get; set; }
        public string INCIDENT_LATITUDE { get; set; }
        public string INCIDENT_LONGITUDE { get; set; }
        public Int64 CREATED_BY { get; set; }
        public string INCIDENT_IMAGE { get; set; }
    }
}
