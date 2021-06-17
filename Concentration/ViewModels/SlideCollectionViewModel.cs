using Concentration.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Concentration.ViewModels
{
    /// <summary>
    /// Work with array of images. 
    /// </summary>
    public class SlideCollectionViewModel : ObservableObject
    {
        /// <summary>
        /// Collection of picture slides.
        /// </summary>
        public ObservableCollection<PictureViewModel> MemorySlides { get; private set; }

        /// <summary>
        /// Selected slides for matching.
        /// </summary>
        private PictureViewModel SelectedSlide1;
        private PictureViewModel SelectedSlide2;

        /// <summary>
        /// Timers for peeking at slides and initial display for memorizing.
        /// </summary>
        private DispatcherTimer _peekTimer;
        private DispatcherTimer _openingTimer;

        /// <summary>
        /// Interval for how long a user peeks at selections.
        /// </summary>
        private const int _peekSeconds = 3;

        /// <summary>
        /// Interval for how long a user has to memorize slides.
        /// </summary>
        private const int _openSeconds = 5;

        /// <summary>
        /// Are selected slides still being displayed.
        /// </summary>
        public bool areSlidesActive
        {
            get
            {
                if (SelectedSlide1 == null || SelectedSlide2 == null)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Have all slides been matched.
        /// </summary>
        public bool AllSlidesMatched
        {
            get
            {
                foreach (var slide in MemorySlides)
                {
                    if (!slide.isMatched)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Can user select a slide.
        /// </summary>
        public bool canSelect { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SlideCollectionViewModel()
        {
            _peekTimer = new DispatcherTimer();
            _peekTimer.Interval = new TimeSpan(0, 0, _peekSeconds);
            _peekTimer.Tick += PeekTimer_Tick;

            _openingTimer = new DispatcherTimer();
            _openingTimer.Interval = new TimeSpan(0, 0, _openSeconds);
            _openingTimer.Tick += OpeningTimer_Tick;
        }

        /// <summary>
        /// Create slides from images in file directory.
        /// </summary>
        /// <param name="imagesPath"></param>
        public void CreateSlides(string imagesPath)
        {
            //New list of slides
            MemorySlides = new ObservableCollection<PictureViewModel>();
            var models = GetModelsFrom(@imagesPath);

            //Create slides with matching pairs from models
            for (int i = 0; i < 12; i += 2)
            {
                //Create 2 matching slides
                var newSlide = new PictureViewModel(models[i]);
                var newSlideMatch = new PictureViewModel(models[i + 1]);
                //Add new slides to collection
                MemorySlides.Add(newSlide);
                MemorySlides.Add(newSlideMatch);
                //Initially display images for user
                newSlide.PeekAtImage();
                newSlideMatch.PeekAtImage();
            }

            ShuffleSlides();
            OnPropertyChanged("MemorySlides");
        }

        /// <summary>
        /// Select a slide to be matched.
        /// </summary>
        /// <param name="slide"></param>
        public void SelectSlide(PictureViewModel slide)
        {
            slide.PeekAtImage();

            if (SelectedSlide1 == null)
            {
                SelectedSlide1 = slide;
            }
            else if (SelectedSlide2 == null && SelectedSlide1.Id != slide.Id)
            {
                SelectedSlide2 = slide;
                HideUnmatched();
            }

            OnPropertyChanged("areSlidesActive");
        }

        /// <summary>
        /// Are the selected slides a match.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfMatched()
        {
            if ((SelectedSlide1.Id == SelectedSlide2.Id - 1 || SelectedSlide1.Id - 1 == SelectedSlide2.Id) && SelectedSlide1.SlideImage == SelectedSlide2.SlideImage)
            {
                MatchCorrect();
                return true;
            }
            else
            {
                MatchFailed();
                return false;
            }
        }

        /// <summary>
        /// Selected slides did not match.
        /// </summary>
        private void MatchFailed()
        {
            SelectedSlide1.MarkFailed();
            SelectedSlide2.MarkFailed();
            ClearSelected();
        }

        /// <summary>
        /// Selected slides matched.
        /// </summary>
        private void MatchCorrect()
        {
            SelectedSlide1.MarkMatched();
            SelectedSlide2.MarkMatched();
            ClearSelected();
        }

        /// <summary>
        /// Clear selected slides.
        /// </summary>
        private void ClearSelected()
        {
            SelectedSlide1 = null;
            SelectedSlide2 = null;
            canSelect = false;
        }

        /// <summary>
        /// Reveal all unmatched slides.
        /// </summary>
        public void RevealUnmatched()
        {
            foreach (var slide in MemorySlides)
            {
                if (!slide.isMatched)
                {
                    _peekTimer.Stop();
                    slide.MarkFailed();
                    slide.PeekAtImage();
                }
            }
        }

        /// <summary>
        /// Hid all slides that are unmatched.
        /// </summary>
        public void HideUnmatched()
        {
            _peekTimer.Start();
        }

        /// <summary>
        /// Display slides for memorizing.
        /// </summary>
        public void Memorize()
        {
            _openingTimer.Start();
        }

        /// <summary>
        /// Get slide picture models for creating picture views.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        private List<PictureModel> GetModelsFrom(string relativePath)
        {
            //List of models for picture slides
            var models = new List<PictureModel>();
            //Get all image URIs in folder
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            startupPath = startupPath.Replace("bin\\Debug\\netcoreapp3.1", "");
            var images = Directory.GetFiles(startupPath + @relativePath, "*.jpg", SearchOption.AllDirectories);
            //Slide id begin at 0
            var id = 0;

            foreach (string i in images)
            {
                string tmp = i.Replace(@"\", "/");
                models.Add(new PictureModel() { Id = id, ImageSource = tmp });
                id++;
                models.Add(new PictureModel() { Id = id, ImageSource = tmp });
                id++;
            }

            return models;
        }

        /// <summary>
        /// Randomize the location of the slides in collection.
        /// </summary>
        private void ShuffleSlides()
        {
            //Randomizing slide indexes
            var rnd = new Random();
            //Shuffle memory slides
            for (int i = 0; i < 64; i++)
            {
                MemorySlides.Reverse();
                MemorySlides.Move(rnd.Next(0, MemorySlides.Count), rnd.Next(0, MemorySlides.Count));
            }
        }

        /// <summary>
        /// Close slides being memorized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpeningTimer_Tick(object sender, EventArgs e)
        {
            foreach (var slide in MemorySlides)
            {
                slide.ClosePeek();
                canSelect = true;
            }
            OnPropertyChanged("areSlidesActive");
            _openingTimer.Stop();
        }

        /// <summary>
        /// Display selected card.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PeekTimer_Tick(object sender, EventArgs e)
        {
            foreach (var slide in MemorySlides)
            {
                if (!slide.isMatched)
                {
                    slide.ClosePeek();
                    canSelect = true;
                }
            }
            OnPropertyChanged("areSlidesActive");
            _peekTimer.Stop();
        }
    }
}
