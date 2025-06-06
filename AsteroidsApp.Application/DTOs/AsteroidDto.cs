namespace AsteroidsApp.Application.DTOs
{
    public class AsteroidDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public double EstimatedDiameter { get; set; }
        public bool IsPotentiallyHazardous { get; set; }
        public DateTime CloseApproachDate { get; set; }
        public double MissDistanceKm { get; set; }
    }
}
