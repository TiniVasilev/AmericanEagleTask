namespace AsteroidsApp.Domain.Entities
{
    public class Asteroid
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public double EstimatedDiameter { get; set; }
        public bool IsPotentiallyHazardous { get; set; }
        public DateTime CloseApproachDate { get; set; }
        public double MissDistanceKm { get; set; }
    }
}
