using Microsoft.AspNetCore.Mvc;
using SlackAPI.Models;
using SlackNet;
using SlackNet.AspNetCore;

namespace AuctionLeague.Controllers
{
    [ApiController]
    public class SlackController : ControllerBase
    {
        private readonly ISlackRequestHandler _requestHandler;
        private readonly SlackEndpointConfiguration _endpointConfig;
        private readonly ISlackApiClient _slack;
    
        public SlackController(ISlackRequestHandler requestHandler, SlackEndpointConfiguration endpointConfig, ISlackApiClient slack)
        {
            _requestHandler = requestHandler;
            _endpointConfig = endpointConfig;
            _slack = slack;
        }
    
        [HttpPost]
        [Route("[Controller]/Submit")]
        public async Task<ActionResult> Submit([FromBody] SlackMessage request)
        {
            await _slack.Chat.PostMessage(new SlackNet.WebApi.Message() { Text = request.Message, Channel = request.SlackChannel }, null);
            return Ok();
        }
    
        // [HttpPost]
        // [Route("[Controller]/Event")]
        // public async Task<IActionResult> Event()
        // {
        //      return await _requestHandler.HandleEventRequest(HttpContext.Request, _endpointConfig);
        // }
    
        [HttpPost]
        [Route("[Controller]/Command")]
        public async Task<IActionResult> Command()
        {
           return await _requestHandler.HandleSlashCommandRequest(HttpContext.Request, _endpointConfig);
        }
    }
}
