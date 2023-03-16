using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Sitecore.ProtobufBrowser.Models;
using Sitecore.ProtobufBrowser.Services;
using Sitecore.ProtobufBrowser.Settings;

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
            InitializeComponent();
        }

        public MainView Model { get; }

        private void FileOpenMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                DefaultExt = ".dat",
                Filter = "protobuf(*.dat)|*.dat",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                var result = dialog.FileNames;
                var validated = result.Any(file => Regex.IsMatch(Path.GetFileName(file), Databases.FilesRegex));

                if (!validated)
                {
                    MessageBox.Show(
                        $"You need to select at the corresponding default file: {Databases.FilesRegex}");
                    return;
                }

                Model.Children.Clear();
                string language = "en";
                var rootItem = _itemProvider.GetItems(dialog.FileNames, language);
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
            if (e.NewValue is BaseItem item) FieldView.ItemsSource = item.Fields;
        }

        private void FieldViewCopyName_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (e.OriginalSource as MenuItem)?.DataContext as BaseItem;
            if (item is null) return;
            System.Windows.Clipboard.SetText(item.Name);
        }

        private void FieldViewCopyValue_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (e.OriginalSource as MenuItem)?.DataContext as BaseItem;
            if (item is null) return;
            System.Windows.Clipboard.SetText(item.Value);
        }
    }
}