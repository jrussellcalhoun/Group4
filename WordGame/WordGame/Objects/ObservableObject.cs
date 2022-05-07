using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WordGame.Objects
{
    /// <summary>
    /// This is a base class made for convenience so that we can encapsulate the implementation of INotifyPropertyChanged
    /// INotifyPropertyChanged is a .Net interface which specifies that this object have the proper methods to perform
    /// data binding onto controls created in xaml. E.G. anything that implements INotifyPropertyChanges is available to
    /// inform the xaml code whenever the value or state of a member property has changed.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // If we have any listeners to the PropertyChanged event then call the PropertyChanged event handler for that event.
            // This utilizes reflection to update the UI with our logic properties.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // This is a special setter method which sets our new value if the field is not null and then triggers a OnPropertyChanged event
        // which will be picked up by any listeners such as xaml controls which may be bound to that property's data.
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
