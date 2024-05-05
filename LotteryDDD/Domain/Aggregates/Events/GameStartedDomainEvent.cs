using LotteryDDD.Domain.Common;

namespace LotteryDDD.Domain.Aggregates.Events
{
    public record GameStartedDomainEvent
        (Guid id) : IDomainEvent
    {
    }
}
