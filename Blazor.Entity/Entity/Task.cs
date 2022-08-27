namespace Blazor.Entity.Entity
{
    public partial class Task
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime PlanTime { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsImportant { get; set; }
        public bool IsFinish { get; set; }
    }
}
