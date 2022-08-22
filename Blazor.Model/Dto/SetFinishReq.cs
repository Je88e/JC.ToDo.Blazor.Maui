namespace Blazor.Model.Dto
{
    public class SetFinishReq
    {
        public Guid TaskId { get; set; }

        public bool IsFinish { get; set; }
    }
}
