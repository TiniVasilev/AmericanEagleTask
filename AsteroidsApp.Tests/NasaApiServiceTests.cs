using AsteroidsApp.Application.DTOs;
using AsteroidsApp.Application.Interfaces;
using AsteroidsApp.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsteroidsApp.Tests
{
    public class NasaApiServiceTests
    {
        [Fact]
        public async Task GetAsteroidsAsync_ReturnsFromCache()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var config = new ConfigurationBuilder().AddInMemoryCollection(new[] { new KeyValuePair<string, string?>("NasaApi:ApiKey", "DEMO_KEY") }).Build();
            var httpClient = new HttpClient(new MockHttpMessageHandler());
            var service = new NasaApiService(httpClient, config, memoryCache);

            // Първо извикване - ще се кешира
            var result1 = await service.GetAsteroidsAsync(new DateTime(2025, 6, 6));
            // Второ извикване - трябва да върне кеша (няма нова заявка)
            var result2 = await service.GetAsteroidsAsync(new DateTime(2025, 6, 6));

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(result1, result2);
        }

        // Mock HttpMessageHandler, който връща фиксиран отговор
        private class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var json = "{\"near_earth_objects\":{\"2025-06-06\":[{\"id\":\"1\",\"name\":\"TestAsteroid\",\"estimated_diameter\":{\"kilometers\":{\"estimated_diameter_max\":1.23}},\"is_potentially_hazardous_asteroid\":true,\"close_approach_data\":[{\"close_approach_date\":\"2025-06-06\",\"miss_distance\":{\"kilometers\":\"12345.67\"}}]}]}}";
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
                });
            }
        }
    }
}
