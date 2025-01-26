using DiscordToExcel_RaidHelper.View;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiscordToExcel_RaidHelper
{
    public partial class MainWindow : Window
    {
        private MainView mainView;
        public MainWindow()
        {
            InitializeComponent();

            mainView = new MainView();
            DataContext = mainView;
        }

        private void LoadEvents_Click(object sender, RoutedEventArgs e)
        {
            mainView.LoadRaidEvents();
        }

        private void SaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            mainView.SaveToExcel_Click();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            mainView.Settings_Click();
        }

        private void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mainView.DataGrid_PreviewMouseLeftButtonDown(sender, e);
        }

        private void DataGrid_DragEnter(object sender, DragEventArgs e)
        {
            mainView.DataGrid_DragEnter(sender, e);
        }

        private void DataGrid_Drop(object sender, DragEventArgs e)
        {
            mainView.DataGrid_Drop(sender, e);
        }
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            mainView.DataGrid_CellEditEnding(sender, e);
        }
    }
}
