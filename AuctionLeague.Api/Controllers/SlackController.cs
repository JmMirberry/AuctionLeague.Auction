using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Mvc;
using SlackAPI.Models;
using SlackNet;
using SlackNet.AspNetCore;
using SlackNet.Interaction;

namespace AuctionLeague.Controllers
{
    [ApiController]
    public class SlackController : ControllerBase
    {
        private readonly ISlackRequestHandler _requestHandler;
        private readonly SlackEndpointConfiguration _endpointConfig;
        private readonly ISlackApiClient _slack;
        private readonly ISlashCommandHandler _commandHandler;

        public SlackController(ISlackRequestHandler requestHandler, SlackEndpointConfiguration endpointConfig, ISlackApiClient slack, ISlashCommandHandler commandHandler)
        {
            _requestHandler = requestHandler;
            _endpointConfig = endpointConfig;
            _slack = slack;
            _commandHandler = commandHandler;
        }

        [HttpPost]
        [Route("[Controller]/Submit")]
        public async Task<ActionResult> Submit([FromBody] SlackMessage request)
        {
            await _slack.Chat.PostMessage(new SlackNet.WebApi.Message() { Text = request.Message, Channel = request.SlackChannel }, null);
            return Ok();
        }

        [HttpPost]
        [Route("[Controller]/Event")]
        public async Task<IActionResult> Event()
        {
             return await _requestHandler.HandleEventRequest(HttpContext.Request, _endpointConfig);
        }

        [HttpPost]
        [Route("[Controller]/Command")]
        public async Task<IActionResult> Command()
        {
            //Shouldn't need to do this, but can't get it to work
            var command = new SlashCommand
            {
                Command = HttpContext.Request.Form["command"],
                ChannelId = HttpContext.Request.Form["channel_id"],
                ChannelName = HttpContext.Request.Form["channel_name"],
                Text = HttpContext.Request.Form["text"]
            };
            
            var result = await _commandHandler.Handle(command);
            return Ok(result);
        }

    }
}