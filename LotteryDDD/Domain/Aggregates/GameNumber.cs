using LotteryDDD.Domain.Common;
using LotteryDDD.Domain.Enums;
using LotteryDDD.Domain.Exceptions;
using LotteryDDD.Domain.ValueObjects;

namespace LotteryDDD.Domain.Aggregates
{
    public class GameNumber : Aggregate<GameNumberId>
    {
        public GameId GameId { get; private set; }
        public GameNumberValue NumberValue { get; set; }

        public static GameNumber Create(GameNumberId id, GameId gameId, GameNumberValue number)
        {
            var gameNumber = new GameNumber
            {
                Id = id,
                GameId = gameId,
                NumberValue = number
            };

            return gameNumber;
        }
    }
}
