using System.Collections.Generic;

namespace SkypeBot.Models
{
    public class ApiStatusResponse
    {
        public string PhysicalPath { get; set; }
        public string MachineName { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> UgsUrls { get; set; }
        public bool CanConnectToDb { get; set; }
        public string GeoIpDatabaseVersion { get; set; } = "NONE";
    }
}
