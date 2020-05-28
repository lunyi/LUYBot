using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using UGS.Infrastructures.Extensions;

namespace SkypeBot.Bots
{
    public class ListenerBot : ActivityHandler
    {
        public ListenerBot()
        {

        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var text = turnContext.Activity.Text;
            var action = text.Replace("Lester announcement 平台公告  ", string.Empty)
                .Trim()
                .Replace("&amp;", "&")
                .ToUpper()
                .Trim();

            var actions = action.Split(" ");
            if (actions.Length < 2)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Hello."), cancellationToken: cancellationToken);
                return;
            }

            if (actions[0].IgnoreCaseNotEquals("ADD"))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text($"I don't understand."), cancellationToken: cancellationToken);
                return;
            }

            await turnContext.SendActivityAsync($"{turnContext.Activity.Conversation.Id}", cancellationToken: cancellationToken);
        }
    }
}
