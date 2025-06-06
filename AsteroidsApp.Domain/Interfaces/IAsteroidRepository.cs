using AsteroidsApp.Domain.Entities;

namespace AsteroidsApp.Domain.Interfaces
{
    public interface IAsteroidRepository
    {
        Task<IEnumerable<Asteroid>> GetAsteroidsAsync(DateTime? date = null);
    }
}
