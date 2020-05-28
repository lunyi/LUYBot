using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkypeBot.Models;
using SkypeBot.Service;

namespace SkypeBot.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/skypemessages")]
    public sealed class SkypeMessageController : ControllerBase
    {
        private readonly INotifier _notifier;

        public SkypeMessageController(
            INotifier notifier)
        {
            _notifier = notifier;
        }

        [HttpPost]
        public async Task<MessageResponse> SendMessageAsync([FromBody]SendMessageRequest request)
        {
            try
            {
                var groups = request.Groups.Select(group => _notifier.PostMessageAsync(request.Message, group, request.ImageUrls));
                await Task.WhenAll(groups);

                return MessageResponse.CreateValidMessage();
            }
            catch (Exception ex)
            {
                return MessageResponse.CreateInvalidMessage($"Group list:{string.Join("|", request.Groups)}, Message:{request.Message}, Exception:{ex}");
            }
        }
    }
}