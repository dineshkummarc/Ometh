using System.Windows;
using System.Windows.Forms;

namespace Ometh.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            dialog.ShowDialog();

            this.viewModel.RepositoryPath = dialog.SelectedPath;
        }
    }
}