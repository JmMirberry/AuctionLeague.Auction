using AuctionLeague.Service.Auction.Interfaces;
using System.Timers;

namespace AuctionLeague.Service.Auction
{
    public class AuctionTimer
    {
        private System.Timers.Timer _timer;
        private int _remainingTime;
        private readonly int _timeToFirstEvent = 10;
        private readonly int _timeToSecondEvent = 12;
        private readonly int _totalTime = 15;

        private readonly ITimerEventHandler _eventHandler;

        public AuctionTimer(ITimerEventHandler eventHandler)
        {
            _timer = new System.Timers.Timer(1000); // Timer interval set to 1 second
            _timer.Elapsed += TimerElapsed;
        }

        public AuctionTimer(ITimerEventHandler eventHandler, int totalTime, int timeToFirstEvent, int timeToSecondEvent)
        {
            _timer = new System.Timers.Timer(1000); // Timer interval set to 1 second
            _timer.Elapsed += TimerElapsed;
            _totalTime = totalTime;
            _timeToFirstEvent = timeToFirstEvent;
            _timeToSecondEvent = timeToSecondEvent;
        }

        public bool TimerRunning() => _timer.Enabled;

        public void StartTimer()
        {
            _remainingTime = _totalTime;
            _timer.Start();
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
                _eventHandler.HandleTimerEnd();
            }
        }
    }
}
