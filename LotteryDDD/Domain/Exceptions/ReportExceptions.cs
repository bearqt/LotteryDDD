using LotteryDDD.Domain.Common;

namespace LotteryDDD.Domain.Exceptions
{
    public class InvalidReportIdException : BadRequestException
    {
        public InvalidReportIdException(Guid reportId)
            : base($"Invalid reportId: {reportId}")
        {
        }
    }
}
