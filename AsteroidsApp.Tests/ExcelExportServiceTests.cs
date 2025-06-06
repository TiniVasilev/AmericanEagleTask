using AsteroidsApp.Application.DTOs;
using AsteroidsApp.Infrastructure.Services;
using System.Collections.Generic;
using Xunit;

namespace AsteroidsApp.Tests
{
    public class ExcelExportServiceTests
    {
        [Fact]
        public void ExportAsteroidsToExcel_ReturnsNonEmptyFile()
        {
            // Arrange
            var service = new ExcelExportService();
            var asteroids = new List<AsteroidDto>
            {
                new AsteroidDto { Name = "Test1", EstimatedDiameter = 1.23, IsPotentiallyHazardous = true, CloseApproachDate = new System.DateTime(2025, 6, 6), MissDistanceKm = 12345.67 },
                new AsteroidDto { Name = "Test2", EstimatedDiameter = 2.34, IsPotentiallyHazardous = false, CloseApproachDate = new System.DateTime(2025, 6, 7), MissDistanceKm = 23456.78 }
            };

            // Act
            var fileBytes = service.ExportAsteroidsToExcel(asteroids);

            // Assert
            Assert.NotNull(fileBytes);
            Assert.True(fileBytes.Length > 0);
        }
    }
}
