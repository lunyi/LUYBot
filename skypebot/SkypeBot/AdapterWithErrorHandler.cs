using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Hosting;
using UGS.Infrastructures.Extensions;

namespace SkypeBot
{
    public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
    {
        public AdapterWithErrorHandler(
            IWebHostEnvironment env,
            ICredentialProvider credentialProvider)
            : base(credentialProvider)
        {
            OnTurnError = async (turnContext, exception) =>
            {
                if (env.IsDevelopment())
                {
                    // Send a trace activity, which will be displayed in the Bot Framework Emulator
                    await turnContext.TraceActivityAsync("OnTurnError Trace", exception.Message,
                        "https://www.botframework.com/schemas/error", "TurnError");
                }
            };
        }
    }
}
