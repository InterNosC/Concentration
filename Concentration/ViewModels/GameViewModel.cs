using System;
using System.Collections.Generic;
using System.Text;

namespace Concentration.ViewModels
{
    public enum SlideCategories
    {
        Animals,
        Cars,
        Foods
    }
    public class GameViewModel : ObservableObject
    {
        /// <summary>
        /// Collection of slides we are playing with.
        /// </summary>
        public SlideCollectionViewModel Slides { get; private set; }

        /// <summary>
        /// Game information scores, attempts etc.
        /// </summary>
        public GameInfoViewModel GameInfo { get; private set; }

        /// <summary>
        /// Game timer for elapsed time.
        /// </summary>
        public TimerViewModel Timer { get; private set; }

        /// <summary>
        /// Category we are playing in.
        /// </summary>
        public SlideCategories Category { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="category"></param>
        public GameViewModel(SlideCategories category)
        {
            Category = category;
            SetupGame(category);
        }

        /// <summary>
        /// Initialize game essentials.
        /// </summary>
        /// <param name="category"></param>
        private void SetupGame(SlideCategories category)
        {

            Slides = new SlideCollectionViewModel();
            Timer = new TimerViewModel(new TimeSpan(0, 0, 1));
            GameInfo = new GameInfoViewModel();

            //Set attempts to the maximum allowed
            GameInfo.ClearInfo();

            //Create slides from image folder then display to be memorized
            Slides.CreateSlides("Assets/" + category.ToString());
            Slides.Memorize();

            //Game has started, begin count.
            Timer.Start();

            //Slides have been updated
            OnPropertyChanged("Slides");
            OnPropertyChanged("Timer");
            OnPropertyChanged("GameInfo");
        }

        /// <summary>
        /// Slide has been clicked.
        /// </summary>
        /// <param name="slide"></param>
        public void ClickedSlide(object slide)
        {
            if (Slides.canSelect)
            {
                var selected = slide as PictureViewModel;
                Slides.SelectSlide(selected);
            }

            if (!Slides.areSlidesActive)
            {
                if (Slides.CheckIfMatched())
                    GameInfo.Award(); //Correct match
                else
                    GameInfo.Penalize(); //Incorrect match
            }

            GameStatus();
        }

        /// <summary>
        /// Status of the current game.
        /// </summary>
        private void GameStatus()
        {
            if (GameInfo.MatchAttempts < 0)
            {
                GameInfo.GameStatus(false);
                Slides.RevealUnmatched();
                Timer.Stop();
            }

            if (Slides.AllSlidesMatched)
            {
                GameInfo.GameStatus(true);
                Timer.Stop();
            }
        }

        /// <summary>
        /// Restart game.
        /// </summary>
        public void Restart()
        {
            SetupGame(Category);
        }
    }
}
