using System.ComponentModel;

namespace Concentration.ViewModels
{
    /// <summary>
    /// This is intended to be the base class for ViewModel types, or
    /// any type that must provide property change notifications. It 
    /// implements INotifyPropertyChanged and, in debug builds, will 
    /// verify that all property names 
    /// passed through the PropertyChanged event are valid properties.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
