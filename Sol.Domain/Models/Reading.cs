namespace Sol.Domain.Models
{
    public record Reading : Hobby
    {
        public override HobbyType Type => HobbyType.Reading;
        public override string NotStartedHeader => "To Be Read";
        public override string InProgressHeader => "Currently Reading";
        public override string CompleteHeader => "Finished";

        public override string AddText => "Add Book";
        public override string StartText => "Start Reading";
        public override string PauseText => "Pause Reading";
        public override string FinishText => "Finish Book";

        public override string ToString() => $"{Type}";
    }
}