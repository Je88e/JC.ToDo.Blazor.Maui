namespace Blazor.Model.Dto
{
    public class SetImportantReq
    {
        public Guid TaskId { get; set; }

        public bool IsImportant { get; set; }
    }
}
