using DiscordToExcel_RaidHelper.Datamodel;
using OfficeOpenXml;

namespace DiscordToExcel_RaidHelper.Excel
{
    // Class to handle exporting raid participants to Excel
    public class ExcelHelper
    {
        public void ExportRaidParticipantsToExcel(AllCurrentRaids raids)
        {
            // Loading the ExcelPackage and set the Workbook and Worksheet
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set licence context 
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Raid Participants");

            worksheet.Cells[1, 1].Value = "Participant Name";
            worksheet.Cells[1, 2].Value = "Role";

            // Write participant data to Excel
            int row = 2;
            foreach (var participant in raids.SignUps)
            {
                worksheet.Cells[row, 1].Value = participant.NameDiscord;
                worksheet.Cells[row, 2].Value = participant.NameMain;
                row++;
            }

            // Save the Excel file
            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Save Raid Participants"
            };

            if (saveDialog.ShowDialog() == true)
            {
                package.SaveAs(new System.IO.FileInfo(saveDialog.FileName));
            }
        }
    }
}