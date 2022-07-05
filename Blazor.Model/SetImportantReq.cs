namespace Blazor.Model
{
    public class SetImportantReq
    {
        public Guid TaskId { get; set; }

        public bool IsImportant { get; set; }
    }
}
