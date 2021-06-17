using Concentration.Models;
using Concentration.ViewModels;
using NUnit.Framework;

namespace TestConcentration
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CreateSlides_SlidesAreExist_IsTrue()
        {
            var Slides = new SlideCollectionViewModel();
            Slides.CreateSlides(@"Assets/Animals");

            bool result = true;
            foreach (var i in Slides.MemorySlides)
            {
                if (i == null) result = false;
            }
            Assert.IsTrue(result);
        }

        [Test]
        public void AllSlidesMatched_MatcheOnlyFewSlides_IsFalse()
        {
            var Slides = new SlideCollectionViewModel();
            Slides.CreateSlides(@"Assets/Animals");

            bool result = true;
            foreach (var i in Slides.MemorySlides)
            {
                if (i.Id != 2) i.MarkMatched();
            }

            Assert.IsFalse(Slides.AllSlidesMatched);
        }

        [Test]
        public void AllSlidesMatched_MatchAllSlides_IsFalse()
        {
            var Slides = new SlideCollectionViewModel();
            Slides.CreateSlides(@"Assets/Animals");

            bool result = true;
            foreach (var i in Slides.MemorySlides)
            {
                i.MarkMatched();
            }

            Assert.IsTrue(Slides.AllSlidesMatched);
        }

        [Test]
        public void CreateSlides_OnlyOnePair_IsTrue()
        {
            var Slides = new SlideCollectionViewModel();
            Slides.CreateSlides(@"Assets/Animals");

            bool result = true;
            int count = 0;
            foreach (var i in Slides.MemorySlides)
            {
                foreach (var j in Slides.MemorySlides)
                {
                    if (i.Id != j.Id && i.SlideImage == j.SlideImage) count++;
                    if (count > 1) break;
                }
                if (count > 1)
                {
                    result = false;
                    break;
                }
                count = 0;
            }
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateSlides_CheckingThatShufflingIsWorking_DifferentArraysOfSlides()
        {
            var Slides1 = new SlideCollectionViewModel();
            Slides1.CreateSlides(@"Assets/Animals");
            var Slides2 = new SlideCollectionViewModel();
            Slides2.CreateSlides(@"Assets/Animals");

            bool result = Slides1.MemorySlides.Equals(Slides2.MemorySlides);
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckIfMatched_MatchTwoEqualPairs_MatchedCorect()
        {
            var Slides1 = new SlideCollectionViewModel();
            Slides1.CreateSlides(@"Assets/Animals");
            var Slide = new SlideCollectionViewModel();
            Slide.SelectSlide(new PictureViewModel(new PictureModel() { Id = 3, ImageSource = "second" }));
            Slide.SelectSlide(new PictureViewModel(new PictureModel() { Id = 4, ImageSource = "second" }));

            Assert.IsTrue(Slide.CheckIfMatched());
        }

        [Test]
        public void CheckIfMatched_MatchTwoDiffPairs_MatchFailed()
        {
            var Slides1 = new SlideCollectionViewModel();
            Slides1.CreateSlides(@"Assets/Animals");
            var Slide = new SlideCollectionViewModel();
            Slide.SelectSlide(new PictureViewModel(new PictureModel() { Id = 3, ImageSource = "second" }));
            Slide.SelectSlide(new PictureViewModel(new PictureModel() { Id = 2, ImageSource = "firsr" }));

            Assert.IsFalse(Slide.CheckIfMatched());
        }

        [Test]
        public void Award_AddFewTimes_TermsAreEqual()
        {
            var game = new GameInfoViewModel();
            int pointsAward = 75;
            int result = 2 * pointsAward;
            game.Award();
            game.Award();

            Assert.IsTrue(result == game.Score);
        }

        [Test]
        public void Penalize_FewTimes_TermsAreEqual()
        {
            var game = new GameInfoViewModel();
            int points = 0, addition = 15;
            int result = points - addition - addition;
            game.Penalize();
            game.Penalize();

            Assert.IsTrue(result == game.Score && game.MatchAttempts == -2);
        }
    }
}