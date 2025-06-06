namespace AsteroidsApp.Domain.Entities
{
    public class ApodImage
    {
        public string Title { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string Explanation { get; set; } = default!;
        public DateTime Date { get; set; }
    }
}
