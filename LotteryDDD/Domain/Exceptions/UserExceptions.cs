using LotteryDDD.Domain.Common;

namespace LotteryDDD.Domain.Exceptions
{
    public class InvalidUserIdException : BadRequestException
    {
        public InvalidUserIdException(Guid userId)
            : base($"Invalid userId: {userId}")
        {
        }
    }

    public class InvalidUsernameException : BadRequestException
    {
        public InvalidUsernameException(string username)
            : base($"Invalid username: {username}")
        {
        }
    }

    public class InvalidBalanceException : BadRequestException
    {
        public InvalidBalanceException(decimal amount)
            : base($"Sorry, the user is whether too poor or too rich to play our game - {amount}")
        {
        }
    }

    public class NotEnoughMoneyException : BadRequestException
    {
        public NotEnoughMoneyException(decimal betAmount)
            : base($"User has not enought money to make a bet - {betAmount}")
        {
        }
    }
}
