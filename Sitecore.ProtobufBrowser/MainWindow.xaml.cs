using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;
using Sitecore.ProtobufBrowser.Models;
using Sitecore.ProtobufBrowser.Services;

namespace Sitecore.ProtobufBrowser
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISitecoreItemProvider _itemProvider;

        public MainWindow(ILogger<MainWindow> logger, ISitecoreItemProvider itemProvider)
        {
            _itemProvider = itemProvider;
            Model = new MainView(this);
            DataContext = Model;
            InitializeComponent();
        }

        public MainView Model { get; }

        private void FileOpenMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var sourceDialog = new SourceFileWizard();

            if (sourceDialog.ShowDialog() == true)
            {
                Model.Children.Clear();
                var language = "en";
                var rootItem = _itemProvider.GetItems(sourceDialog.MainFile, sourceDialog.ModuleFile,
                    sourceDialog.SecondaryFile, language);
                Model.Children.Add(rootItem);
                DataContext = Model;
            }
        }

        private void FileExitMenu_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ContentTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is BaseItem item)
            {
                FieldView.ItemsSource = item.Fields;
                LocationView.ItemsSource = item.Locations;
            }
        }

        private void FieldViewCopyName_OnClick(object sender, RoutedEventArgs e)
        {
            if (!((e.OriginalSource as MenuItem)?.DataContext is BaseItem item))
            {
                return;
            }

            Clipboard.SetText(item.Name);
        }

        private void FieldViewCopyValue_OnClick(object sender, RoutedEventArgs e)
        {
            if (!((e.OriginalSource as MenuItem)?.DataContext is BaseItem item))
            {
                return;
            }

            Clipboard.SetText(item.Value);
        }

        private void HelpMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ShowHelp();
        }

        private static void ShowHelp()
        {
            new Help().ShowDialog();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ShowHelp();
        }
    }
}