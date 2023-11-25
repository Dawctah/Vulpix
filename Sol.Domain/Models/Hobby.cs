namespace Sol.Domain.Models
{
    public abstract record Hobby
    {
        public abstract HobbyType Type { get; }

        public abstract string NotStartedHeader { get; }
        public abstract string InProgressHeader { get; }
        public abstract string CompleteHeader { get; }

        public abstract string AddText { get; }
        public abstract string StartText { get; }
        public abstract string PauseText { get; }
        public abstract string FinishText { get; }
    }

    public record Reading : Hobby
    {
        public override HobbyType Type => HobbyType.Books;
        public override string NotStartedHeader => "To Be Read";
        public override string InProgressHeader => "Currently Reading";
        public override string CompleteHeader => "Finished";

        public override string AddText => "Add Book";
        public override string StartText => "Start Reading";
        public override string PauseText => "Pause Reading";
        public override string FinishText => "Finish Book";

        public override string ToString() => $"{Type}";
    }

    public record BookClub : Reading
    {
        public override HobbyType Type => HobbyType.BookClub;
        public override string ToString() => "Book Club";
    }

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

    public record Gaming : Hobby
    {
        public override HobbyType Type => HobbyType.VideoGames;
        public override string NotStartedHeader => "Backlog";
        public override string InProgressHeader => "Currently Playing";
        public override string CompleteHeader { get; } = "Complete";

        public override string AddText => "Add Game";
        public override string StartText => "Start Playing";
        public override string PauseText => "Pause Game";
        public override string FinishText => "Beat Game";

        public override string ToString() => $"Video Games";
    }

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

    public record Developing : Hobby
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

    public enum HobbyType
    {
        Miscellaneous,
        Books,
        BookClub,
        Writing,
        VideoGames,
        MoviesAndTV,
        Programming,
    }
}