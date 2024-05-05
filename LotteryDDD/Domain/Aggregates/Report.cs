using LotteryDDD.Domain.Common;
using LotteryDDD.Domain.ValueObjects;

namespace LotteryDDD.Domain.Aggregates
{
    public class Report : Aggregate<ReportId>
    {
        public UserId UserId { get; private set; }
        public string Message { get; private set; }

        public static Report Create(ReportId id, UserId userId, string message)
        {
            var report = new Report { 
                Id = id,
                UserId = userId, 
                Message = message
            };

            return report;
        }
    }
}
