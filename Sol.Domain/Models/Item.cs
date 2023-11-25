using Knox.Extensions;
using Newtonsoft.Json;

namespace Sol.Domain.Models
{
    public record Item : AggregateRoot
    {
        public Item() : base(Guid.NewGuid())
        { }

        public static Item Empty { get; } = new Item();

        public string Name { get; init; } = string.Empty;
        public HobbyType HobbyType { get; init; } = HobbyType.Miscellaneous;
        public int Index { get; init; } = 0;
        public DateOnly AddedDate { get; init; } = DateTime.Today.ToDateOnly();
        public DateOnly StartedDate { get; init; } = DateOnly.MinValue;
        public DateOnly CompletionDate { get; init; } = DateOnly.MinValue;

        public ItemStatus Status { get; init; } = ItemStatus.NotStarted;

        [JsonIgnore]
        public bool IsFinished => CompletionDate != DateOnly.MinValue;

        public Item ChangeStatus(ItemStatus status)
        {
            var completeDate = CompletionDate;
            var startDate = StartedDate;

            if (status == ItemStatus.Complete)
            {
                completeDate = DateTime.Now.ToDateOnly();
            }

            if (status == ItemStatus.InProgress)
            {
                startDate = DateTime.Now.ToDateOnly();
            }

            return this with
            {
                Status = status,
                StartedDate = startDate,
                CompletionDate = completeDate,
            };
        }

        /// <summary>
        /// Automatically updates the index as the last item in the list as it's current status.
        /// </summary>
        public Item UpdateIndex(int index) => this with
        {
            Index = index
        };

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
                result += $"\n\t{StartedDate} - {CompletionDate} ({(CompletionDate.ToDateTime(new()) - StartedDate.ToDateTime(new())).TotalDays + 1} days)";
            }

            return result;
        }
    }

    public enum ItemStatus
    {
        NotStarted,
        InProgress,
        Complete,
    }
}
