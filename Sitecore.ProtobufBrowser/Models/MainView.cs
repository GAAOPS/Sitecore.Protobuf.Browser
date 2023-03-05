using System.Collections.ObjectModel;

namespace Sitecore.ProtobufBrowser.Models
{
    public class MainView : BaseView<MainWindow>, IParentItem
    {
        private ObservableCollection<BaseItem> _children = new ObservableCollection<BaseItem>();

        public MainView(MainWindow view) : base(view)
        {
        }

        public ObservableCollection<BaseItem> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged(nameof(Children));
            }
        }
    }
}