using AsteroidsApp.Application.DTOs;

namespace AsteroidsApp.Application.Interfaces
{
    public interface INasaApiService
    {
        Task<IEnumerable<AsteroidDto>> GetAsteroidsAsync(DateTime? date = null);
        Task<ApodImageDto> GetApodImageAsync(DateTime? date = null);
    }
}
