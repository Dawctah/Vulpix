namespace Vulpix.Domain.Models
{
    public record Programming : Hobby
    {
        public override HobbyType Type => HobbyType.Programming;
        public override string NotStartedHeader => "To Do";
        public override string InProgressHeader => "In Progress";
        public override string CompleteHeader => "Complete";

        public override string AddText => "Add Task";
        public override string StartText => "Start Working";
        public override string PauseText => "Take a Break";
        public override string FinishText => "Finish Task";

        public override string ToString() => $"{Type}";
    }
}