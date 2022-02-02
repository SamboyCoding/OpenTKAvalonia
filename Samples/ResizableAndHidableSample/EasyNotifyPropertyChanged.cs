using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace NonFullScreenSample
{
    public class EasyNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}