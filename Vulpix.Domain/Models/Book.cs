using Newtonsoft.Json;
using Vulpix.Domain.Common;

namespace Vulpix.Domain.Models
{
    [Obsolete("Book is no longer used and has been genericized into Item")]
    public record Book(Guid Key, string Title, DateOnly StartDate, DateOnly FinishDate) : AggregateRoot(Key)
    {
        public static Book Empty { get; } = new Book(Guid.Empty, string.Empty, DateOnly.MinValue, DateOnly.MinValue);

        public static Book Create(string title) => Create(title, DateOnly.MinValue, DateOnly.MinValue);

        public static Book Create(string title, DateOnly startDate, DateOnly finishDate) => Empty with
        {
            Key = Guid.NewGuid(),
            Title = title,
            StartDate = startDate,
            FinishDate = finishDate,
        };

        [JsonIgnore]
        public bool IsFinished => FinishDate != DateOnly.MinValue;

        public Book Start() => this with
        {
            StartDate = DateTime.Now.ToDateOnly()
        };

        public Book Stop() => this with
        {
            StartDate = DateOnly.MinValue
        };

        public Book Finish() => this with
        {
            FinishDate = DateTime.Now.ToDateOnly()
        };

        public Book Update(Book book) => this with
        {
            Title = book.Title,
            StartDate = book.StartDate,
            FinishDate = book.FinishDate
        };

        public bool Valid()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return false;
            }

            if (StartDate > FinishDate)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            var result = Title;

            if (!IsFinished)
            {
                if (StartDate != DateOnly.MinValue)
                {
                    result += $"\n\tStarted {StartDate}";
                }
            }
            else
            {
                result += $"\n\t{StartDate} - {FinishDate} ({((FinishDate.ToDateTime(new()) - StartDate.ToDateTime(new())).TotalDays) + 1} days)";
            }

            return result;
        }
    }
}