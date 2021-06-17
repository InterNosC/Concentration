using System;
using System.Collections.Generic;
using System.Text;

namespace Concentration.ViewModels
{
    /// <summary>
    /// Start menu view.
    /// </summary>
    public class StartMenuViewModel
    {
        private MainWindow _mainWindow;
        public StartMenuViewModel(MainWindow main)
        {
            _mainWindow = main;
        }
        /// <summary>
        /// Start game with selected category.
        /// </summary>
        /// <param name="categoryIndex"></param>
        public void StartNewGame(int categoryIndex)
        {
            var category = (SlideCategories)categoryIndex;
            GameViewModel newGame = new GameViewModel(category);
            _mainWindow.DataContext = newGame;
        }
    }
}
