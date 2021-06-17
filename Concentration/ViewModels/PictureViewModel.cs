using Concentration.Models;
using System.Windows.Media;

namespace Concentration.ViewModels
{
    public class PictureViewModel : ObservableObject
    {
        /// <summary>
        /// Model for this view.
        /// </summary>
        private PictureModel _model;

        /// <summary>
        /// ID of this slide.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Slide status.
        /// </summary>
        private bool _isViewed;
        private bool _isMatched;
        private bool _isFailed;

        /// <summary>
        /// Is being viewed by user.
        /// </summary>
        public bool isViewed
        {
            get
            {
                return _isViewed;
            }
            private set
            {
                _isViewed = value;
                OnPropertyChanged("SlideImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        /// <summary>
        /// Has been matched.
        /// </summary>
        public bool isMatched
        {
            get
            {
                return _isMatched;
            }
            private set
            {
                _isMatched = value;
                OnPropertyChanged("SlideImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        /// <summary>
        /// Has failed to be matched.
        /// </summary>
        public bool isFailed
        {
            get
            {
                return _isFailed;
            }
            private set
            {
                _isFailed = value;
                OnPropertyChanged("SlideImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        /// <summary>
        /// User can select this slide
        /// </summary>
        public bool isSelectable
        {
            get
            {
                if (isMatched)
                    return false;
                if (isViewed)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Image to be displayed
        /// </summary>
        public string SlideImage
        {
            get
            {
                if (isMatched)
                    return _model.ImageSource;
                if (isViewed)
                    return _model.ImageSource;


                return "/Concentration;component/Assets/mystery_image.jpg";
            }
        }

        /// <summary>
        /// Brush color of border based on status.
        /// </summary>
        public Brush BorderBrush
        {
            get
            {
                if (isFailed)
                    return Brushes.Red;
                if (isMatched)
                    return Brushes.Green;
                if (isViewed)
                    return Brushes.Yellow;

                return Brushes.Black;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model"></param>
        public PictureViewModel(PictureModel model)
        {
            _model = model;
            Id = model.Id;
        }

        /// <summary>
        /// Has been matched.
        /// </summary>
        public void MarkMatched()
        {
            isMatched = true;
        }

        /// <summary>
        /// Has failed to match.
        /// </summary>
        public void MarkFailed()
        {
            isFailed = true;
        }

        /// <summary>
        /// No longer being viewed.
        /// </summary>
        public void ClosePeek()
        {
            isViewed = false;
            isFailed = false;
            OnPropertyChanged("isSelectable");
            OnPropertyChanged("SlideImage");
        }

        /// <summary>
        /// Let user view
        /// </summary>
        public void PeekAtImage()
        {
            isViewed = true;
            OnPropertyChanged("SlideImage");
        }
    }
}
