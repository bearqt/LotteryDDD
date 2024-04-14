using LotteryDDD.Domain.Exceptions;

namespace LotteryDDD.Domain.ValueObjects
{
    public class GameId
    {
        public Guid Value { get; }

        private GameId(Guid value)
        {
            Value = value;
        }
        public static GameId Of(Guid value)
        {
            if (value == Guid.Empty) throw new InvalidGameIdException(value);
            return new GameId(value);
        }

        public static implicit operator Guid(GameId gameId)
        {
            return gameId.Value;
        }
    }

    public class BetAmount
    {
        public decimal Value { get; }

        private BetAmount(decimal value)
        {
            Value = value;
        }

        public static BetAmount Of(decimal value)
        {
            if (value <= 0 || value > 1000000)
                throw new InvalidBetException(value);
            return new BetAmount(value);
        }
    }

    public class RoundNumber
    {
        public int Value { get; }

        private RoundNumber(int value)
        {
            Value = value;
        }

        public static RoundNumber Of(int value)
        {
            if (value < 0 || value > 100)
                throw new InvalidRoundNumberException(value);
            return new RoundNumber(value);
        }
    }
}
