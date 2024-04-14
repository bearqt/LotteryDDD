using LotteryDDD.Domain.Exceptions;

namespace LotteryDDD.Domain.ValueObjects
{
    public class ScorePoints
    {
        public int Value { get; }

        private ScorePoints(int value)
        {
            Value = value;
        }
        public static ScorePoints Of(int value)
        {
            if (value < 0 || value > 10000) 
                throw new InvalidScorePointsException(value);
            return new ScorePoints(value);
        }
    }
}
