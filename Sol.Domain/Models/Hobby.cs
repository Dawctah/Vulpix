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

    public enum HobbyType
    {
        Reading,
        BookClub,
        Gaming,
        MoviesAndTV,
        Writing,
        Programming,
        Miscellaneous,
    }
}