using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Sitecore.ProtobufBrowser.Settings;

namespace Sitecore.ProtobufBrowser
{
    /// <summary>
    ///     Interaction logic for SourceFileWizard.xaml
    /// </summary>
    public partial class SourceFileWizard : Window
    {
        public SourceFileWizard()
        {
            InitializeComponent();
        }

        public string MainFile { get; set; }
        public string ModuleFile { get; set; }
        public string SecondaryFile { get; set; }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            MainPath.Text = GetFiles(true);
        }

        private void ModuleButton_Click(object sender, RoutedEventArgs e)
        {
            ModulePath.Text = GetFiles(false);
        }

        private void SecondaryButton_Click(object sender, RoutedEventArgs e)
        {
            SecondaryPath.Text = GetFiles(false);
        }

        private string GetFiles(bool mainFiles)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                DefaultExt = ".dat",
                Filter = "protobuf(*.dat)|*.dat",
                Multiselect = false
            };

            if (dialog.ShowDialog() == true)
            {
                var file = dialog.FileName;
                if (mainFiles)
                {
                    var validated = Databases.MainFiles.Contains(Path.GetFileName(file),
                        StringComparer.InvariantCultureIgnoreCase);

                    if (!validated)
                    {
                        MessageBox.Show(
                            $"You need to select at the corresponding default file:{string.Join(" or", Databases.MainFiles)}");
                        return string.Empty;
                    }
                }

                return file;
            }

            return string.Empty;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            MainFile = MainPath.Text;
            ModuleFile = ModulePath.Text;
            SecondaryFile = SecondaryPath.Text;

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}