namespace AuctionLeague.Service;

using Amazon.Runtime.Internal;
using SlackNet;
using SlackNet.AspNetCore;

public class SlackAuctionService
{ 
    private readonly TimerService _timerService;
    private readonly ISlackApiClient _slack;

    public SlackAuctionService(TimerService timerService, ISlackApiClient slack)
    {
        _timerService = TimerService.Instance;
        _timerService.TimerFinished += AuctionEnded();
        _slack = slack;
    }

    public void StartAuction()
    {
        _timerService.StartTimer();
    }

    private Action AuctionEnded()
    {
        return _slack.Chat.PostMessage(new SlackNet.WebApi.Message() { Text = request.Message, Channel = request.SlackChannel }, null).Result();
    }
}
