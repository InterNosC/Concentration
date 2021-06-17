using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Concentration.ViewModels
{
    /// <summary>
    /// About score, attemps and time.
    /// </summary>
    public class GameInfoViewModel : ObservableObject
    {
        // const of maximum attemps
        private const int _maxAttempts = 4;
        // const of award in game
        private const int _pointAward = 75;
        // const of good match
        private const int _pointDeduction = 15;

        /// <summary>
        /// Game attempts value.
        /// </summary>
        private int _matchAttempts;

        /// <summary>
        /// Game score value.
        /// </summary>
        private int _score;

        /// <summary>
        /// Is game lost value.
        /// </summary>
        private bool _gameLost;

        /// <summary>
        /// Is game won value.
        /// </summary>
        private bool _gameWon;

        /// <summary>
        /// Game attempts property.
        /// </summary>
        public int MatchAttempts
        {
            get
            {
                return _matchAttempts;
            }
            private set
            {
                _matchAttempts = value;
                OnPropertyChanged("MatchAttempts");
            }
        }

        /// <summary>
        /// Game score property.
        /// </summary>
        public int Score
        {
            get
            {
                return _score;
            }
            private set
            {
                _score = value;
                OnPropertyChanged("Score");
            }
        }

        /// <summary>
        /// Is game lost property.
        /// </summary>
        public Visibility LostMessage
        {
            get
            {
                if (_gameLost)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }
        }

        /// <summary>
        /// Is game won property.
        /// </summary>
        public Visibility WinMessage
        {
            get
            {
                if (_gameWon)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }
        }

        /// <summary>
        /// Game status property.
        /// </summary>
        /// <param name="win"></param>
        public void GameStatus(bool win)
        {
            if (!win)
            {
                _gameLost = true;
                OnPropertyChanged("LostMessage");
            }

            if (win)
            {
                _gameWon = true;
                OnPropertyChanged("WinMessage");
            }
        }

        /// <summary>
        /// To Default values.
        /// </summary>
        public void ClearInfo()
        {
            Score = 0;
            MatchAttempts = _maxAttempts;
            _gameLost = false;
            _gameWon = false;
            OnPropertyChanged("LostMessage");
            OnPropertyChanged("WinMessage");
        }

        /// <summary>
        /// Award method to add points.
        /// </summary>
        public void Award()
        {
            Score += _pointAward;
        }

        /// <summary>
        /// Lose method of points.
        /// </summary>
        public void Penalize()
        {
            Score -= _pointDeduction;
            MatchAttempts--;
        }
    }
}
