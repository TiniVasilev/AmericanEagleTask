using AsteroidsApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AsteroidsApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly INasaApiService _nasaApiService;
        private readonly IExcelExportService _excelExportService;

        public HomeController(INasaApiService nasaApiService, IExcelExportService excelExportService)
        {
            _nasaApiService = nasaApiService;
            _excelExportService = excelExportService;
        }

        public async Task<IActionResult> Index(DateTime? date)
        {
            ViewBag.SelectedDate = date?.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
            try
            {
                ViewBag.Asteroids = await _nasaApiService.GetAsteroidsAsync(date);
                ViewBag.Apod = await _nasaApiService.GetApodImageAsync(date);
            }
            catch (Exception)
            {
                TempData["Error"] = "Възникна проблем при зареждане на данните от NASA API.";
                ViewBag.Asteroids = null;
                ViewBag.Apod = null;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(DateTime? date)
        {
            try
            {
                var asteroids = await _nasaApiService.GetAsteroidsAsync(date);
                var bytes = _excelExportService.ExportAsteroidsToExcel(asteroids);
                var fileName = $"asteroids_{(date ?? DateTime.UtcNow):yyyyMMdd}.xlsx";
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch
            {
                TempData["Error"] = "Възникна проблем при експортиране на данните в Excel.";
                return RedirectToAction("Index", new { date });
            }
        }
    }
}
