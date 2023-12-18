namespace Sol.Domain.Models
{
    public record BookClub : Reading
    {
        public override HobbyType Type => HobbyType.BookClub;
        public override string ToString() => "Book Club";
    }
}