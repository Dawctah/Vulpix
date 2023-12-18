namespace Vulpix.Domain.Models
{
    public record Writing : Hobby
    {
        public override HobbyType Type => HobbyType.Writing;
        public override string NotStartedHeader => "Planned";
        public override string InProgressHeader => "In Progress";
        public override string CompleteHeader => "Complete";

        public override string AddText => "Add Story";
        public override string StartText => "Start Writing";
        public override string PauseText => "Pause Writing";
        public override string FinishText => "Finish Story";

        public override string ToString() => $"{Type}";
    }
}