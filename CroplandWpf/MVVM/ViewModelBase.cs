using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace CroplandWpf.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public bool Set<T>(ref T variable, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(value, variable)) return false;
            variable = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
