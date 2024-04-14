using LotteryDDD.Domain.Enums;
using LotteryDDD.Domain.ValueObjects;

namespace LotteryDDD.Domain.Aggregates
{
    public class Score
    {
        public UserId UserId { get; private set; }
        public GameId GameId { get; private set; }
        public ScorePoints Points { get; private set; }

        public static Score Create(UserId userId, GameId id, ScorePoints points)
        {
            var score = new Score
            {
                UserId = userId,
                GameId = id,
                Points = points
            };

            return score;
        }
    }
}
