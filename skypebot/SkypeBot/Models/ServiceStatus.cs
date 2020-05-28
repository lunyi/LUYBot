using System;

namespace SkypeBot.Models
{
    public class ServiceStatus
    {
        public int processor_count { get; set; }
        public DateTimeOffset current_server_time { get; set; }
        public string server_name { get; set; }
        public string location { get; set; }
        public string version { get; set; }
        public DateTimeOffset last_built { get; set; }
        public string environment { get; set; }
        public string nats_url { get; set; }
        public string system_api_url { get; set; }
    }
}
