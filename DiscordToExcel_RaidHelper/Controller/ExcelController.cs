using DiscordToExcel_RaidHelper.API;
using DiscordToExcel_RaidHelper.Datamodel;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

public class ExcelController
{
    private readonly GoogleSheetsService _googleSheetsService;

    public ExcelController(string credentials, string spreadsheetId)
    {
        _googleSheetsService = new GoogleSheetsService(credentials, spreadsheetId);
    }

    public async Task WriteRaidmemberToExcel(IEnumerable<SignUps> selectedRaidEvent)
    {
        if (selectedRaidEvent == null)
        {
            throw new System.Exception("no raiddata to export.");
        }

        var values = new List<IList<object>>();

        // Copy the names of the participants to Excel
        foreach (var member in selectedRaidEvent)
        {
          
            {
                values.Add(new List<object> { member.NameMain ?? "Unknown" });
            }
        }


        // Bereich in der Tabelle, in den die Namen geschrieben werden (A5:A34)
        string range = "HD Raid!A5:A34";
        await _googleSheetsService.WriteDataAsync(range, values);
    }

    public async Task<bool> CheckForExistingRaidData(string spreadsheetId)
    {
        string range = "HD Raid!A5:A34";
        List<List<string>> HDRaidParticipants = await _googleSheetsService.ReadDataAsync(range);


        if (HDRaidParticipants != null)
        {
            foreach (var row in HDRaidParticipants)
            {
                string cellValue = row.Count > 0 ? row[0].ToString() : "";
                if (!string.IsNullOrWhiteSpace(cellValue) && !cellValue.StartsWith("Group"))
                {
                    // Warnung ausgeben und Nutzer fragen
                    return MessageBox.Show("There are already Entries in 'HD Raid' Do you want to override this data?",
                                           "Warnung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
                }
            }
        }
        return true; // Keine Einträge vorhanden, Speichern erlaubt
    }
}
