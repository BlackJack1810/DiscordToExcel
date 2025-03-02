using System.Windows;

namespace DiscordToExcel_RaidHelper
{
    public partial class SettingsWindow : Window
    {
        public string? ServerId { get; private set; }
        public string? RaidHelperApi { get; private set; }
        public string? GoogleSheetsID { get; private set; }
        public AppSettings UpdatedSettings { get; private set; }

        private AppSettings _appSettings;

        public SettingsWindow( )
        {
            InitializeComponent();
            _appSettings = new AppSettings();
            _appSettings = SettingsManager.LoadSettings();

            // Load current settings
            ServerIdTextBox.Text = _appSettings.ServerId;
            RaidHelperApiTextBox.Text = _appSettings.RaidHelperApi;
            GoogleSpreadsheetIDTextBox.Text = _appSettings.GoogleSheetsID;
        }
        

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Werte aus Textfeldern speichern
            _appSettings.ServerId = ServerIdTextBox.Text;
            _appSettings.RaidHelperApi = RaidHelperApiTextBox.Text;
            _appSettings.GoogleSheetsID = GoogleSpreadsheetIDTextBox.Text;
            _appSettings.saveButtonClicked = true;

            // Save Setting
            SettingsManager.SaveSettings(_appSettings);
            MessageBox.Show("Settings saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
        }

        private void OnClose(object sender, EventArgs e)
        {
            UpdatedSettings = _appSettings;
            //Close();
        }
    }
}
