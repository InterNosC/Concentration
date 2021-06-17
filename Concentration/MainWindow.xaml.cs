using Concentration.ViewModels;
using System.Windows;

namespace Concentration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new StartMenuViewModel(this);
        }
    }
}
