namespace Sol.Domain.Models
{
    public record MoviesAndTV : Hobby
    {
        public override HobbyType Type => HobbyType.MoviesAndTV;
        public override string NotStartedHeader => "To Watch";
        public override string InProgressHeader => "Currently Watching";
        public override string CompleteHeader => "Done";

        public override string AddText => "Add Show or Film";
        public override string StartText => "Start Watching";
        public override string PauseText => "Stop Watching For Now";
        public override string FinishText => "Finish Watching";

        public override string ToString() => "Movies And TV";
    }
}