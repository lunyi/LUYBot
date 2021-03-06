﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;

namespace SkypeBot.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {        
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IBot _bot;

        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot)
        {
            _adapter = adapter;
            _bot = bot;
        }

        [HttpPost, HttpGet]
        public Task PostAsync()
        {
            return _adapter.ProcessAsync(Request, Response, _bot);
        }
    }
}