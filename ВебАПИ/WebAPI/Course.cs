namespace WebAPI
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Duration { get; set; }
        public string? Description { get; set; }
    }
}
