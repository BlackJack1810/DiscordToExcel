// MainWindow.xaml.cs
using DiscordToExcel_RaidHelper.API;
using DiscordToExcel_RaidHelper.Datamodel;
using DiscordToExcel_RaidHelper.Excel;
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
        public ObservableCollection<AllCurrentRaids> Raids { get; set; } = new ObservableCollection<AllCurrentRaids>();
        // The currently selected raid event
        public AllCurrentRaids SelectedRaidEvent { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize helper classes with the API token
            raidHelperApi = new RaidHelperApi("v7ydHm1q2ZKzthA5svdhPCv5e2s6HUo5Zlpcj8LL");
            excelHelper = new ExcelHelper();
            // Load raid events on application start
            LoadRaidEvents();
        }

        // Load Events on button click
        private void LoadEvents_Click(object sender, RoutedEventArgs e)
        {
            LoadRaidEvents();
        }

        // Asynchronously loads raid events from the API
        private async void LoadRaidEvents()
        {
            try
            {
                // Retrieve raid events from the API and include participants
                var events = await raidHelperApi.GetRaidEventsAsync();
                events = await raidHelperApi.GetRaidParticipantsAsync(events);

                // Clear the current events in the ObservableCollection
                Raids.Clear();
                // Add the retrieved events to the ObservableCollection
                foreach (var raidEvent in events)
                {
                    Raids.Add(raidEvent);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

