using AuctionLeague.Service.Auction.Interfaces;
using System.Timers;
using Microsoft.Extensions.Options;
using SlackNet;

namespace AuctionLeague.Service.Auction
{    
    // This should be a singleton. If multiple timers are needed implement a timer manager singleton instead that uses a concurrent dictionary
    public class AuctionTimer : IAuctionTimer
    {       
        private readonly System.Timers.Timer _timer;
        private readonly int _timeToFirstEvent; 
        private readonly int _timeToSecondEvent;
        private readonly int _totalTime;
        private int _remainingTime;
        private readonly ITimerEventHandler _eventHandler;
        private readonly ISlackApiClient _slackClient;

        public AuctionTimer(ITimerEventHandler eventHandler, IOptions<AuctionSettings> settings, ISlackApiClient slackClient)
        { 
            _eventHandler = eventHandler;
            _totalTime = settings.Value.TimeToSoldMs / 1000;  
            _timeToFirstEvent = settings.Value.TimeToOnceMs;  
            _timeToSecondEvent = settings.Value.TimeToTwiceMs;    
            _timer = new System.Timers.Timer(1000); // Timer interval set to 1 second
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            _slackClient = slackClient;
        }
        
        public bool TimerRunning() => _timer.Enabled;

        public void StartTimer()
        { 
            _remainingTime = _totalTime;  
            _timer.Start();
            SendMessage("started");
        }
        
        public void RestartTimer() 
        {  
            _timer.Stop();  
            _remainingTime = _totalTime;
            _timer.Start();
        }
        
        public void ResetTimer()
        {      
            _timer.Stop();
            _remainingTime = _totalTime;
        }  
        
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            SendMessage(_remainingTime.ToString());
            _remainingTime--;

            if (_remainingTime == _timeToFirstEvent) 
            { 
                _eventHandler.HandleFirstEvent();   
            }
            else if (_remainingTime == _timeToSecondEvent) 
            {      
                _eventHandler.HandleSecondEvent(); 
            }  
            else if (_remainingTime <= 0)  
            {
                _timer.Stop();
                _eventHandler.HandleTimerEnd();  
            } 
        }

        private async Task SendMessage(string message)
        {
            var slackMessage = new SlackNet.WebApi.Message()
            {
                Text = message,
                Channel = "dev"
            };

            await _slackClient.Chat.PostMessage(slackMessage, null);
        }
    }
}
