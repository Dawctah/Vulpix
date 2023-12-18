namespace Sol.Domain.Models
{
    public record Gaming : Hobby
    {
        public override HobbyType Type => HobbyType.Gaming;
        public override string NotStartedHeader => "Backlog";
        public override string InProgressHeader => "Currently Playing";
        public override string CompleteHeader { get; } = "Complete";

        public override string AddText => "Add Game";
        public override string StartText => "Start Playing";
        public override string PauseText => "Pause Game";
        public override string FinishText => "Beat Game";

        public override string ToString() => $"{Type}";
    }
}