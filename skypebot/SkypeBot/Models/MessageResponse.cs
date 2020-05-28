namespace SkypeBot.Models
{
    public class MessageResponse
    {
        public static MessageResponse CreateValidMessage()
        {
            return new MessageResponse
            {
                Valid = true,
            };
        }

        public static MessageResponse CreateInvalidMessage(string message)
        {
            return new MessageResponse
            {
                Valid = false,
                Message = message
            };
        }
        public bool Valid { get; set; }
        public string Message { get; set; }
    }
}
