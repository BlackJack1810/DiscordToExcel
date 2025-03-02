using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace DiscordToExcel_RaidHelper.API;
public class GoogleSheetsService
{
    private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    private const string ApplicationName = "RaidHelperToExcel";
    private readonly SheetsService _service;
    private readonly string _spreadsheetId;

    public GoogleSheetsService(string credentials, string spreadsheetId)
    {
        _spreadsheetId = spreadsheetId;

        GoogleCredential credential;
        using (var stream = new FileStream(credentials, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
        }

        _service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
    }

    public async Task WriteDataAsync(string range, IList<IList<object>> values)
    {
        var requestBody = new ValueRange { Values = values };
        var request = _service.Spreadsheets.Values.Update(requestBody, _spreadsheetId, range);
        request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

        try
        {
            UpdateValuesResponse response = await request.ExecuteAsync();

            if (response != null && response.UpdatedCells > 0)
            {
                MessageBox.Show($"Export successfully! {response.UpdatedCells} Cells updated",
                                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Export failed. No Data was updated",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while exporting: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    // read data from Google Sheets
    public async Task<List<List<string>>> ReadDataAsync(string range)
    {
        try
        {
            var request = _service.Spreadsheets.Values.Get(_spreadsheetId, range);
            var response = await request.ExecuteAsync();

            if (response.Values == null || response.Values.Count == 0)
            {
                MessageBox.Show("No Data Found.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            List<List<string>> result = new();
            foreach (var row in response.Values)
            {
                List<string> rowData = new();
                foreach (var cell in row)
                {
                    rowData.Add(cell.ToString());
                }
                result.Add(rowData);
            }

            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error reading the data: {ex.Message}");
            return null;
        }
    }

    // load JSON file with credentials
    public static string ExtractJsonKey()
    {
        string tempPath = Path.Combine(Path.GetTempPath(),"google_auth.json");

        // don't save if already exists
        if (!File.Exists(tempPath))
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach(string name in assembly.GetManifestResourceNames())
{
                Debug.WriteLine(name);
            }
            // setting the path to the json file of embedded ressource 
            string ressourceName = "DiscordToExcel_RaidHelper.Key.prime-depot-450413-q9-b8d94937cf22.json";

            using (Stream stream = assembly.GetManifestResourceStream(ressourceName))
            {
                if (stream == null)
                {
                    throw new Exception("json file not found");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    File.WriteAllText(tempPath, reader.ReadToEnd()); ;
                }
            }
        }

        return tempPath;
    }
}
