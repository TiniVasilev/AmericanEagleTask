using AsteroidsApp.Application.DTOs;
using AsteroidsApp.Application.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace AsteroidsApp.Infrastructure.Services
{
    public class NasaApiService : INasaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;

        public NasaApiService(HttpClient httpClient, IConfiguration config, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _config = config;
            _cache = cache;
            _apiKey = _config["NasaApi:ApiKey"] ?? "DEMO_KEY";
        }

        public async Task<IEnumerable<AsteroidDto>> GetAsteroidsAsync(DateTime? date = null)
        {
            var dateStr = (date ?? DateTime.UtcNow).ToString("yyyy-MM-dd");
            var cacheKey = $"asteroids_{dateStr}";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<AsteroidDto> cached))
                return cached;

            var url = $"https://api.nasa.gov/neo/rest/v1/feed?start_date={dateStr}&end_date={dateStr}&api_key={_apiKey}";
            var response = await _httpClient.GetFromJsonAsync<NasaNeoFeedResponse>(url);
            if (response == null || response.near_earth_objects == null || !response.near_earth_objects.ContainsKey(dateStr))
                return Enumerable.Empty<AsteroidDto>();
            var asteroids = response.near_earth_objects[dateStr]?
                .Where(a => a != null)
                .Select(a => new AsteroidDto
                {
                    Id = a.id,
                    Name = a.name,
                    EstimatedDiameter = a.estimated_diameter?.kilometers?.estimated_diameter_max ?? 0,
                    IsPotentiallyHazardous = a.is_potentially_hazardous_asteroid,
                    CloseApproachDate = DateTime.TryParse(a.close_approach_data?.FirstOrDefault()?.close_approach_date, out var dt) ? dt : (date ?? DateTime.UtcNow),
                    MissDistanceKm = double.TryParse(a.close_approach_data?.FirstOrDefault()?.miss_distance?.kilometers, out var km) ? km : 0
                })
                .ToList() ?? new List<AsteroidDto>();
            _cache.Set(cacheKey, asteroids, TimeSpan.FromMinutes(10));
            return asteroids;
        }

        public async Task<ApodImageDto> GetApodImageAsync(DateTime? date = null)
        {
            var dateStr = (date ?? DateTime.UtcNow).ToString("yyyy-MM-dd");
            var cacheKey = $"apod_{dateStr}";
            if (_cache.TryGetValue(cacheKey, out ApodImageDto cached))
                return cached;

            var url = $"https://api.nasa.gov/planetary/apod?date={dateStr}&api_key={_apiKey}";
            var apod = await _httpClient.GetFromJsonAsync<NasaApodResponse>(url);
            if (apod == null || string.IsNullOrEmpty(apod.title) || string.IsNullOrEmpty(apod.url) || string.IsNullOrEmpty(apod.date))
                return null;
            var result = new ApodImageDto
            {
                Title = apod.title,
                Url = apod.url,
                Explanation = apod.explanation ?? string.Empty,
                Date = DateTime.TryParse(apod.date, out var dt) ? dt : (date ?? DateTime.UtcNow)
            };
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }

        // Вътрешни класове за десериализация
        public class NasaNeoFeedResponse
        {
            public Dictionary<string, List<NasaNeoObject>> near_earth_objects { get; set; } = default!;
        }
        public class NasaNeoObject
        {
            public string id { get; set; } = default!;
            public string name { get; set; } = default!;
            public EstimatedDiameter estimated_diameter { get; set; } = default!;
            public bool is_potentially_hazardous_asteroid { get; set; }
            public List<CloseApproachData>? close_approach_data { get; set; }
        }
        public class EstimatedDiameter
        {
            public Kilometers kilometers { get; set; } = default!;
        }
        public class Kilometers
        {
            public double estimated_diameter_max { get; set; }
        }
        public class CloseApproachData
        {
            public string close_approach_date { get; set; } = default!;
            public MissDistance miss_distance { get; set; } = default!;
        }
        public class MissDistance
        {
            public string kilometers { get; set; } = default!;
        }
        public class NasaApodResponse
        {
            public string title { get; set; } = default!;
            public string url { get; set; } = default!;
            public string explanation { get; set; } = default!;
            public string date { get; set; } = default!;
        }
    }
}
