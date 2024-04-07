using LotteryDDD.Domain.Enums;
using LotteryDDD.Domain.ValueObjects;

namespace LotteryDDD.Domain.Aggregates
{
    public class GameUser
    {
        public GameId GameId { get; private set; }
        public UserId UserId { get; private set; }
        public UserSessionStatus SessionStatus { get; private set; }

        public static GameUser Create(UserId userId, GameId gameId)
        {
            var gameUser = new GameUser
            {
                GameId = gameId,
                UserId = userId
            };
            return gameUser;
        }
    }
}
