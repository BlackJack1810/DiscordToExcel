
using OfficeOpenXml;

namespace DiscordToExcel_RaidHelper
{
    public class ExcelHelper
    {
        public void ExportRaidParticipantsToExcel(RaidEvent raidEvent)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Lizenzkontext festlegen
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Raid Participants");

            worksheet.Cells[1, 1].Value = "Participant Name";
            worksheet.Cells[1, 2].Value = "Role";

            int row = 2;
            foreach (var participant in raidEvent.Participants)
            {
                worksheet.Cells[row, 1].Value = participant.Name;
                worksheet.Cells[row, 2].Value = participant.Role;
                row++;
            }

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