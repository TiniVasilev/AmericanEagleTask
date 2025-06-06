using System.Collections.Generic;
using AsteroidsApp.Application.DTOs;

namespace AsteroidsApp.Application.Interfaces
{
    public interface IExcelExportService
    {
        byte[] ExportAsteroidsToExcel(IEnumerable<AsteroidDto> asteroids);
    }
}
