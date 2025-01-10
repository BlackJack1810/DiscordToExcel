// MainWindow.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DiscordToExcel_RaidHelper
{
    public partial class MainWindow : Window
    {
        private RaidHelperApi raidHelperApi;
        private ExcelHelper excelHelper;

        public ObservableCollection<RaidEvent> RaidEvents { get; set; } = new ObservableCollection<RaidEvent>();
        public RaidEvent SelectedRaidEvent { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            raidHelperApi = new RaidHelperApi("<your-api-token>");
            excelHelper = new ExcelHelper();
            //DataContext = this;
            LoadRaidEvents();
        }

        private async void LoadRaidEvents()
        {
            try
            {
                var events = await raidHelperApi.GetRaidEventsAsync();
                RaidEvents.Clear();
                foreach (var raidEvent in events)
                {
                    RaidEvents.Add(raidEvent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRaidEvent != null)
            {
                try
                {
                    excelHelper.ExportRaidParticipantsToExcel(SelectedRaidEvent);
                    MessageBox.Show("Export successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a raid event first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
