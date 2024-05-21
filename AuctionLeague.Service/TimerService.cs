using System.Timers;

namespace AuctionLeague.Service
{


    public class TimerService
    {
        private static TimerService _instance;
        private static readonly object _lock = new object();
        private System.Timers.Timer _timer;
        private int _remainingTime;
        private readonly int _timeToFirstEvent;
        private readonly int _timeToSecondEvent;
        private readonly int _totalTime;

        public event Action EventOne;
        public event Action EventTwo;
        public event Action TimerFinished;

        private TimerService()
        {
            _timer = new System.Timers.Timer(1000); // Timer interval set to 1 second
            _timer.Elapsed += TimerElapsed;
        }

        public static TimerService Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new TimerService();
                }
            }
        }

        public void StartTimer()
        {
            _remainingTime = _totalTime;
            _timer.Start();
        }

        public void ResetTimer()
        {
            _timer.Stop();
            _remainingTime = _totalTime;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _remainingTime--;

            if (_remainingTime == _timeToFirstEvent)
            {
                EventOne?.Invoke();
            }
            else if (_remainingTime == _timeToSecondEvent)
            {
                EventTwo?.Invoke();
            }
            else if (_remainingTime <= 0)
            {
                _timer.Stop();
                TimerFinished?.Invoke();
            }
        }
    }
}
