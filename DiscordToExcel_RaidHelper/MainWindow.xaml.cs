using DiscordToExcel_RaidHelper.View;
using System.Windows;

namespace DiscordToExcel_RaidHelper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainView();
        }

        private void LoadEvents_Click(object sender, RoutedEventArgs e)
        {
            ((MainView)DataContext).LoadRaidEvents();
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            ((MainView)DataContext).ExportToExcel();
        }
    }
}
