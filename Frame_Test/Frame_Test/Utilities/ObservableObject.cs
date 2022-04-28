using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Word_Game.Utilities
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
            ArgumentNullException.ThrowIfNull(newValue, nameof(newValue));

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
