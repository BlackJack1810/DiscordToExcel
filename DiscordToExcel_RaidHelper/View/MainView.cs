using DiscordToExcel_RaidHelper.API;
using DiscordToExcel_RaidHelper.Controller;
using DiscordToExcel_RaidHelper.Datamodel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DiscordToExcel_RaidHelper.View
{
    public class MainView : INotifyPropertyChanged
    {
        private RaidController raidController;
        private ExcelController excelController;
        private AllCurrentRaids selectedRaidEvent;
        private RaidMemberListModel raidMemberListModel;
        private AppSettings _appSettings;
        private DataGridRow draggedRow;

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
                    OnPropertyChanged(nameof(SignUps));
                    UpdateRaidMemberList();
                }
            }
        }

        private ObservableCollection<SignUps> signUpsStructure = new ObservableCollection<SignUps>();
        public ObservableCollection<SignUps> SignUpsStructure
        {
            get => signUpsStructure;
            set
            {
                signUpsStructure = value;
                OnPropertyChanged(nameof(SignUpsStructure));
            }
        }

        private ObservableCollection<SignUps> signUps = new ObservableCollection<SignUps>();
        public ObservableCollection<SignUps> SignUps
        {
            get => signUps;
            set
            {
                signUps = value;
                OnPropertyChanged(nameof(SignUps));
            }
        }

        public MainView()
        {
            // Load Settings
            _appSettings = SettingsManager.LoadSettings();
            raidController = new RaidController(_appSettings.RaidHelperApi, _appSettings.ServerId);
            excelController = new ExcelController();
            raidMemberListModel = new RaidMemberListModel();
            SignUpsStructure = raidMemberListModel.CreateRaidMemberList();
            LoadRaidEvents();
            UpdateRaidMemberList();
        }

        private void UpdateRaidMemberList()
        {
            if (SelectedRaidEvent != null)
            {
                var updatedList = raidMemberListModel.CreateRaidMemberList();
                int groupIndex = 0;
                foreach (var member in SelectedRaidEvent.SignUps)
                {
                    while (updatedList[groupIndex].IsGroupHeader)
                    {
                        groupIndex++;
                    }
                    // check for name mapping
                    var mapping = _appSettings.NameMappings.FirstOrDefault(m => m.NameInDiscord == member.NameDiscord);
                    if (mapping != null)
                    {
                        // if a mapping exists, use the main name
                        member.NameMain = mapping.NameOfMain;
                    }
                    updatedList[groupIndex] = member;
                    groupIndex++;
                }
                SignUpsStructure = updatedList;
            }
        }

        public async void LoadRaidEvents()
        {
            try
            {
                var events = await raidController.LoadRaidEventsAsync();
                Raids.Clear();
                foreach (var raidEvent in events)
                {
                    if (raidEvent.StartTime > DateTime.Now)
                    {
                        Raids.Add(raidEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveToExcel_Click()
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

        public void Settings_Click()
        {
            var settingsWindow = new SettingsWindow(_appSettings);

            // show modal dialog
            if (settingsWindow.ShowDialog() == true)
            {
                // Save after closing the window
                SettingsManager.SaveSettings(_appSettings);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Saving the Change of Main Names
        public void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // get the edited row and column
            var editedRow = e.Row.Item;
            var column = e.Column.Header.ToString();

            if (column == "Name of Main")
            {
                var textBox = e.EditingElement as TextBox;
                if (textBox != null)
                {
                    var mainName = textBox.Text;
                    var discordName = (editedRow as SignUps).NameDiscord; // change `YourModelClass` to the name of the class that represents a row in the DataGrid
                                        
                    AddNameMapping(discordName, mainName);
                }
            }
        }

        public void AddNameMapping(string discordName, string mainName)
        {
            // exclude group headers
            var excludedGroups = new[] { "Group 1", "Group 2", "Group 3", "Group 4", "Group 5" };
            if (excludedGroups.Contains(discordName) || excludedGroups.Contains(mainName))
            {
                return; 
            }

            // check for unique main name
            bool exists = _appSettings.NameMappings.Any(mapping =>
                mapping.NameOfMain == mainName);

            if (!exists && mainName != null)
            {
                // if no member with the same main name exists, add the mapping
                _appSettings.NameMappings.Add(new NameMapping
                {
                    NameInDiscord = discordName,
                    NameOfMain = mainName
                });
                                
                SettingsManager.SaveSettings(_appSettings);
            }
            else
            {
                MessageBox.Show("No Main was entered or this main name is already in use. Please choose another one or check for double registrations", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Drag & Drop für DataGrid
        public void DataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("PreviewMouseLeftButtonDown");
            // look for clicked row
            var dataGrid = sender as DataGrid;
            var hit = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));
            var row = FindVisualParent<DataGridRow>(hit.VisualHit);

            if (row != null && !((SignUps)row.Item).IsGroupHeader)
            {
                draggedRow = row;
                DragDrop.DoDragDrop(dataGrid, row, DragDropEffects.Move);
            }
        }

        public void DataGrid_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the data being dragged is of type DataGridRow
            if (!e.Data.GetDataPresent(typeof(DataGridRow)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        public void DataGrid_Drop(object sender, DragEventArgs e)
        {
            if (draggedRow == null)
                return;

            var dataGrid = sender as DataGrid;
            var dropPosition = e.GetPosition(dataGrid);

            var hit = VisualTreeHelper.HitTest(dataGrid, dropPosition);
            var targetRow = FindVisualParent<DataGridRow>(hit.VisualHit);

            if (targetRow != null && !((SignUps)targetRow.Item).IsGroupHeader)
            {
                // swap items
                var draggedItem = draggedRow.Item as SignUps;
                var targetItem = targetRow.Item as SignUps;

                if (draggedItem != null && targetItem != null)
                {
                    var list = (ObservableCollection<SignUps>)dataGrid.ItemsSource;
                    int draggedIndex = list.IndexOf(draggedItem);
                    int targetIndex = list.IndexOf(targetItem);

                    list[draggedIndex] = targetItem;
                    list[targetIndex] = draggedItem;
                }
            }

            draggedRow = null;
        }

        // helper method to find parent of specific type
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null) return null;

            return parent as T ?? FindVisualParent<T>(parent);
        }
    }
}