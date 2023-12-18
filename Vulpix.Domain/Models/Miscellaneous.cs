namespace Vulpix.Domain.Models
{
    public record Miscellaneous : Hobby
    {
        public override HobbyType Type => HobbyType.Miscellaneous;

        public override string NotStartedHeader => "Not Started";
        public override string InProgressHeader => "In Progress";
        public override string CompleteHeader => "Completed";

        public override string AddText => "Add Item";

        public override string StartText => "Start Item";

        public override string PauseText => "Pause Item";

        public override string FinishText => "Complete Item";
        public override string ToString() => $"{Type}";
    }
}