// MainWindow.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DiscordToExcel_RaidHelper
{
    public partial class MainWindow : Window
    {
        // Instances of helper classes for API and Excel operations
        private RaidHelperApi raidHelperApi;
        private ExcelHelper excelHelper;

        // ObservableCollection to bind raid events to the UI
        public ObservableCollection<RaidEvent> RaidEvents { get; set; } = new ObservableCollection<RaidEvent>();
        // The currently selected raid event
        public RaidEvent SelectedRaidEvent { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            // Initialize helper classes with the API token
            raidHelperApi = new RaidHelperApi("<your-api-token>");
            excelHelper = new ExcelHelper();
            // Load raid events on application start
            LoadRaidEvents();
        }

        // Asynchronously loads raid events from the API
        private async void LoadRaidEvents()
        {
            try
            {
                // Retrieve raid events from the API
                var events = await raidHelperApi.GetRaidEventsAsync();
                // Clear the current events in the ObservableCollection
                RaidEvents.Clear();
                // Add the retrieved events to the ObservableCollection
                foreach (var raidEvent in events)
                {
                    RaidEvents.Add(raidEvent);
                }
            }
            catch (Exception ex)
            {
                // Display an error message if loading events fails
                MessageBox.Show($"Failed to load events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the "Export to Excel" button click
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRaidEvent != null)
            {
                try
                {
                    // Export the participants of the selected raid event to Excel
                    excelHelper.ExportRaidParticipantsToExcel(SelectedRaidEvent);
                    // Display a success message
                    MessageBox.Show("Export successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Display an error message if the export fails
                    MessageBox.Show($"Failed to export: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Display a warning if no raid event is selected
                MessageBox.Show("Please select a raid event first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

