using DiscordToExcel_RaidHelper.View;
using System.Windows;

namespace DiscordToExcel_RaidHelper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void LoadEvents_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).LoadRaidEvents();
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            ((MainViewModel)DataContext).ExportToExcel();
        }
    }
}
