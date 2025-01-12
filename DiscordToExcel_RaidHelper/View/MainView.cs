using DiscordToExcel_RaidHelper.API;
using DiscordToExcel_RaidHelper.Controller;
using DiscordToExcel_RaidHelper.Datamodel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            raidController = new RaidController();
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaidMemberList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                var row = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);
                if (row != null && row.IsSelected)
                {
                    DragDrop.DoDragDrop(dataGrid, row.Item, DragDropEffects.Move);
                }
            }
        }

        public void RaidMemberList_Drop(object sender, DragEventArgs e)
        {
            if (sender is DataGrid dataGrid && e.Data.GetDataPresent(typeof(SignUps)))
            {
                var droppedData = e.Data.GetData(typeof(SignUps)) as SignUps;
                var target = ((FrameworkElement)e.OriginalSource).DataContext as SignUps;

                if (droppedData != null && target != null && droppedData != target)
                {
                    var members = SignUpsStructure;
                    int removedIdx = members.IndexOf(droppedData);
                    int targetIdx = members.IndexOf(target);

                    if (removedIdx < targetIdx)
                    {
                        members.Insert(targetIdx + 1, droppedData);
                        members.RemoveAt(removedIdx);
                    }
                    else
                    {
                        int remIdx = removedIdx + 1;
                        if (members.Count + 1 > remIdx)
                        {
                            members.Insert(targetIdx, droppedData);
                            members.RemoveAt(remIdx);
                        }
                    }
                }
            }
        }

        private static T? FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent) return parent;
            return FindVisualParent<T>(parentObject);
        }
    }
}