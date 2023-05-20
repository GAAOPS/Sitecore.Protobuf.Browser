using System.Collections.ObjectModel;

namespace Sitecore.ProtobufBrowser.Models
{
    public class MainView : BaseView<MainWindow>, IParentItem
    {
        private ObservableCollection<BaseItem> _children = new ObservableCollection<BaseItem>();
        private bool _hideMainItems;
        private bool _hideModuleItems;

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


        public bool HideMainItems
        {
            get => _hideMainItems;
            set
            {
                _hideMainItems = value;
                OnPropertyChanged(nameof(HideMainItems));
            }
        }

        public bool HideModuleItems
        {
            get => _hideModuleItems;
            set
            {
                _hideModuleItems = value;
                OnPropertyChanged(nameof(HideModuleItems));
            }
        }
    }
}