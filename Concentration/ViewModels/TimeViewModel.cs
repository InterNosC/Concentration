using System;
using System.Windows.Threading;

namespace Concentration.ViewModels
{
    /// <summary>
    /// Timer for game.
    /// </summary>
    public class TimerViewModel : ObservableObject
    {
        /// <summary>
        /// Full game timer.
        /// </summary>
        private DispatcherTimer _playedTimer;

        /// <summary>
        ///  Session timer.
        /// </summary>
        private TimeSpan _timePlayed;

        private const int _playSeconds = 1;

        /// <summary>
        /// Property for session timer.
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                return _timePlayed;
            }
            set
            {
                _timePlayed = value;
                OnPropertyChanged("Time");
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="time"></param>
        public TimerViewModel(TimeSpan time)
        {
            _playedTimer = new DispatcherTimer();
            _playedTimer.Interval = time;
            _playedTimer.Tick += PlayedTimer_Tick;
            _timePlayed = new TimeSpan();
        }

        /// <summary>
        /// Start timer.
        /// </summary>
        public void Start()
        {
            _playedTimer.Start();
        }

        /// <summary>
        /// Stop timer.
        /// </summary>
        public void Stop()
        {
            _playedTimer.Stop();
        }

        /// <summary>
        /// Tick.
        /// </summary>
        private void PlayedTimer_Tick(object sender, EventArgs e)
        {
            Time = _timePlayed.Add(new TimeSpan(0, 0, 1));
        }
    }
}
