using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WordGame.Objects
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // If we have any listeners to the PropertyChanged event then call the PropertyChanged event handler for that event.
            // This utilizes reflection to update the UI with our logic properties.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, string propertyName = default)
        {
            ArgumentNullException.ThrowIfNull(newValue);

            field ??= newValue;

            if(!field.Equals(newValue))
            {
                field = newValue;
                OnPropertyChanged(propertyName);
                return true;
            }
            
            return false;
        }
    }
}
