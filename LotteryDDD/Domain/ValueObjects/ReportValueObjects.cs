using LotteryDDD.Domain.Exceptions;

namespace LotteryDDD.Domain.ValueObjects
{
    public class ReportId
    {
        public Guid Value { get; }

        private ReportId(Guid value)
        {
            Value = value;
        }
        public static ReportId Of(Guid value)
        {
            if (value == Guid.Empty) throw new InvalidReportIdException(value);
            return new ReportId(value);
        }

        public static implicit operator Guid(ReportId reportId)
        {
            return reportId.Value;
        }
    }
}
