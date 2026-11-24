using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BIMPlugins.Windows.Mvvm
{
    /// <summary>INotifyPropertyChanged for the package's own view models.</summary>
    internal abstract class ObservableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>Sets the field and tells the bindings; false when the value is the same.</summary>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
