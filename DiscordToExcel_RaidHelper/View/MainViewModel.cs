using DiscordToExcel_RaidHelper.API;
using DiscordToExcel_RaidHelper.Controller;
using DiscordToExcel_RaidHelper.Datamodel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DiscordToExcel_RaidHelper.View
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private RaidController raidController;
        private ExcelController excelController = new ExcelController();
        private AllCurrentRaids selectedRaidEvent;

        public ObservableCollection<AllCurrentRaids> Raids { get; set; } = new ObservableCollection<AllCurrentRaids>();

        public AllCurrentRaids SelectedRaidEvent
        {
            get => selectedRaidEvent;
            set
            {
                if (selectedRaidEvent != value)
                {
                    selectedRaidEvent = value;
                    OnPropertyChanged(nameof(SelectedRaidEvent));
                    OnPropertyChanged(nameof(RaidMembers));
                }
            }
        }

        public ObservableCollection<SignUps> RaidMembers => new ObservableCollection<SignUps>(SelectedRaidEvent?.SignUps ?? new List<SignUps>());

        public MainViewModel()
        {
            raidController = new RaidController();
            LoadRaidEvents();
        }

        public async void LoadRaidEvents()
        {
            try
            {
                var events = await raidController.LoadRaidEventsAsync();
                Raids.Clear();
                foreach (var raidEvent in events)
                {
                    Raids.Add(raidEvent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportToExcel()
        {
            if (SelectedRaidEvent != null)
            {
                try
                {
                    excelController.ExportRaidParticipantsToExcel(SelectedRaidEvent);
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
