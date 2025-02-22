using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

public static class SettingsManager
{
    private static string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RaidHelper");
    private static string configPath = Path.Combine(appDataPath, "AppSettings.json");

    public static AppSettings LoadSettings()
    {
        // Create default settings if file does not exist
        if (!File.Exists(configPath))
        {
            CreateDefaultSettings();
        }

        string json = File.ReadAllText(configPath);
        return JsonConvert.DeserializeObject<AppSettings>(json);
    }

    private static void CreateDefaultSettings()
    {
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath); // create directory if it does not exist
        }

        var defaultSettings = new AppSettings
        {
            ServerId = null,
            RaidHelperApi = null,
            GoogleSheetsID = null,
            saveButtonClicked = false,
        };

        string json = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented);
        File.WriteAllText(configPath, json);
    }

    public static void SaveSettings(AppSettings settings)
    {
        string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(configPath, json);
    }
}
