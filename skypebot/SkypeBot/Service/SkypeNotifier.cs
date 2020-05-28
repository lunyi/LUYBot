using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using SkypeBot.Config;

namespace SkypeBot.Service
{
    public interface INotifier
    {
        Task<ResourceResponse> PostMessageAsync(string message, string channelId, string[] imageUrls);
    }

    public class SkypeNotifier : INotifier
    {
        private readonly ITGPBotConfig _config;

        public SkypeNotifier(ITGPBotConfig config)
        {
            _config = config;
        }

        Task<ResourceResponse> INotifier.PostMessageAsync(string message, string channelId, string[] imageUrls)
        {
            AppCredentials.TrustServiceUrl(_config.SkypeServiceUrl, DateTime.Now.AddDays(7));
            var account = new MicrosoftAppCredentials(_config.AppId, _config.AppPassword);
            var connectorClient = new ConnectorClient(new Uri(_config.SkypeServiceUrl), account);

            var activity = MessageFactory.Text(message);
            activity.Conversation = new ConversationAccount(true, "announce", channelId, "Lester announcement 平台公告", null);

            if (imageUrls?.Length > 0)
            {
                activity.Attachments = imageUrls.Select(imageUrl => new Attachment
                {
                    ContentUrl = imageUrl,
                    ContentType = "image/png"
                }).ToList();
            }

            return connectorClient.Conversations.SendToConversationAsync(activity);
        }
    }
}
