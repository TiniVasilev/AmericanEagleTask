using AsteroidsApp.Application.DTOs;
using AsteroidsApp.Application.Interfaces;
using ClosedXML.Excel;

namespace AsteroidsApp.Infrastructure.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportAsteroidsToExcel(IEnumerable<AsteroidDto> asteroids)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Asteroids");
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Diameter (km)";
            worksheet.Cell(1, 3).Value = "Hazardous";
            worksheet.Cell(1, 4).Value = "Close Approach";
            worksheet.Cell(1, 5).Value = "Miss Distance (km)";
            int row = 2;
            foreach (var a in asteroids)
            {
                worksheet.Cell(row, 1).Value = a.Name;
                worksheet.Cell(row, 2).Value = a.EstimatedDiameter;
                worksheet.Cell(row, 2).Style.NumberFormat.Format = "0.00";
                worksheet.Cell(row, 3).Value = a.IsPotentiallyHazardous ? "Yes" : "No";
                worksheet.Cell(row, 4).Value = a.CloseApproachDate.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 5).Value = a.MissDistanceKm;
                worksheet.Cell(row, 5).Style.NumberFormat.Format = "0.00";
                row++;
            }
            worksheet.Range(1, 1, row - 1, 5).SetAutoFilter();
            worksheet.Columns().AdjustToContents();
            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }
    }
}
