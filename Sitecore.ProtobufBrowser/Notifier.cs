using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sitecore.ProtobufBrowser
{
    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string[] propertyNames = null)
        {
            if (propertyNames != null)
                foreach (var propertyName in propertyNames)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}