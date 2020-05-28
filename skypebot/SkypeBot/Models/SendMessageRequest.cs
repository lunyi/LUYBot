namespace SkypeBot.Models
{
    public class SendMessageRequest
    {
        public string[] Groups { get; set; }
        public string Message { get; set; }
        public string[] ImageUrls { get; set; }
    }
}
