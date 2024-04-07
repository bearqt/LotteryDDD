using LotteryDDD.Domain.Exceptions;

namespace LotteryDDD.Domain.ValueObjects
{
    public class UserId
    {
        public Guid Value { get; }

        private UserId(Guid value)
        {
            Value = value;
        }
        public static UserId Of(Guid value)
        {
            if (value == Guid.Empty) throw new InvalidUserIdException(value);
            return new UserId(value);
        }

        public static implicit operator Guid(UserId userId)
        {
            return userId.Value;
        }
    }

    public class Username
    {
        public string Value { get; }

        private Username(string value)
        {
            Value = value;
        }

        public static Username Of(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
                throw new InvalidUsernameException(value);
            return new Username(value);
        }
    }

    public class Balance
    {
        public decimal Value { get; }
        private Balance(decimal value)
        {
            Value = value;
        }

        public static Balance Of(decimal value)
        {
            if (value < 0 || value > 100000000)
                throw new InvalidBalanceException(value);
            return new Balance(value);
        }
    }
}
