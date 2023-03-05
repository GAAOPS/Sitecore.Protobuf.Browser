namespace Sitecore.ProtobufBrowser.Models
{
    public class BaseView<T> : Notifier
    {
        public BaseView(T view)
        {
            View = view;
        }

        public T View { get; set; }
    }
}