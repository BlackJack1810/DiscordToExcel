using System.Windows;

namespace DiscordToExcel_RaidHelper
{
    public partial class SettingsWindow : Window
    {
        public string ServerId { get; private set; }
        public string RaidHelperApi { get; private set; }
        private AppSettings _appSettings;

        public SettingsWindow(AppSettings appSettings)
        {
            InitializeComponent();
            _appSettings = appSettings;
            _appSettings = SettingsManager.LoadSettings();

            // Load current settings
            ServerIdTextBox.Text = _appSettings.ServerId;
            RaidHelperApiTextBox.Text = _appSettings.RaidHelperApi;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Werte aus Textfeldern speichern
            _appSettings.ServerId = ServerIdTextBox.Text;
            _appSettings.RaidHelperApi = RaidHelperApiTextBox.Text;

            // Save Setting
            SettingsManager.SaveSettings(_appSettings);

            // Fenster schließen
            DialogResult = true;
            Close();
        }
    }
}
