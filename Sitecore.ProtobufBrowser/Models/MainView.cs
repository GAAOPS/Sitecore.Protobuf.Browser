using System.Collections.ObjectModel;

namespace Sitecore.ProtobufBrowser.Models
{
    public class MainView : BaseView<MainWindow>, IParentItem
    {
        private ObservableCollection<BaseItem> _children = new ObservableCollection<BaseItem>();
        private bool _hidePrimaryFileItems;

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


        public bool HidePrimaryFileItems
        {
            get => _hidePrimaryFileItems;
            set
            {
                _hidePrimaryFileItems = value;
                OnPropertyChanged(nameof(HidePrimaryFileItems));
            }
        }
    }
}