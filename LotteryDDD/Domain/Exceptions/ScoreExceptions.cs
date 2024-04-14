using LotteryDDD.Domain.Common;

namespace LotteryDDD.Domain.Exceptions
{
    public class InvalidScorePointsException : BadRequestException
    {
        public InvalidScorePointsException(int score)
            : base($"Invalid score: {score}")
        {
        }
    }
}
