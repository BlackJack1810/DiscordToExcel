using System.IO;
using System.Text.Json;

public static class SettingsManager
{
    private static readonly string SettingsFilePath = "AppSettings.json";    

    public static AppSettings LoadSettings()
    {
        if (!File.Exists(SettingsFilePath))
        {
            var defaultSettings = new AppSettings();
            SaveSettings(defaultSettings);
            return defaultSettings;
        }

        var json = File.ReadAllText(SettingsFilePath);
        return JsonSerializer.Deserialize<AppSettings>(json);
    }

    public static void SaveSettings(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsFilePath, json);        
    }
}
