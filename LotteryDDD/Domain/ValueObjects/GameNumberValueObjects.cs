using LotteryDDD.Domain.Exceptions;

namespace LotteryDDD.Domain.ValueObjects
{
    public class GameNumberId
    {
        public Guid Value { get; }

        private GameNumberId(Guid value)
        {
            Value = value;
        }
        public static GameNumberId Of(Guid value)
        {
            if (value == Guid.Empty)
                throw new InvalidGameNumberIdException(value);
            return new GameNumberId(value);
        }

        public static implicit operator Guid(GameNumberId gameId)
        {
            return gameId.Value;
        }
    }

    public class GameNumberValue
    {
        public int Value { get; }

        private GameNumberValue(int value)
        {
            Value = value;
        }
        public static GameNumberValue Of(int value)
        {
            if (value < 0 || value > 1000)
                throw new InvalidGameNumberException(value);
            return new GameNumberValue(value);
        }

        public static implicit operator int(GameNumberValue number)
        {
            return number.Value;
        }
    }
}
