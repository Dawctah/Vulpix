using Knox.Extensions;
using Newtonsoft.Json;

namespace Sol.Domain.Models
{
    public record Item : AggregateRoot
    {
        public Item() : base(Guid.NewGuid())
        { }

        public string Name { get; init; } = string.Empty;
        public HobbyType HobbyType { get; init; } = HobbyType.Miscellaneous;
        public int Index { get; init; } = 0;
        public DateOnly AddedDate { get; init; } = DateTime.Today.ToDateOnly();
        public DateOnly StartedDate { get; init; } = DateOnly.MinValue;
        public DateOnly CompletionDate { get; init; } = DateOnly.MinValue;

        [JsonIgnore]
        public bool IsFinished => CompletionDate != DateOnly.MinValue;

        public override string ToString()
        {
            var result = Name;

            if (!IsFinished)
            {
                if (StartedDate != DateOnly.MinValue)
                {
                    result += $"\n\tStarted {StartedDate}";
                }
            }
            else
            {
                result += $"\n\t{StartedDate} - {CompletionDate} ({((StartedDate.ToDateTime(new()) - StartedDate.ToDateTime(new())).TotalDays) + 1} days)";
            }

            return result;
        }
    }
}
