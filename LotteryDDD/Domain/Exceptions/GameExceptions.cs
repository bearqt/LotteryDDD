using LotteryDDD.Domain.Common;

namespace LotteryDDD.Domain.Exceptions
{
    public class InvalidGameIdException : BadRequestException
    {
        public InvalidGameIdException(Guid gameId)
            : base($"Invalid gameId: {gameId}")
        {
        }
    }
    public class InvalidBetException : BadRequestException
    {
        public InvalidBetException(decimal bet)
            : base($"Invalid bet anount: {bet}")
        {
        }
    }

    public class UserIsAlreadyInGameException : BadRequestException
    {
        public UserIsAlreadyInGameException(Guid gameGuid, Guid userGuid)
            : base($"This user ({userGuid}) has already been connected to the game {gameGuid}")
        {
        }
    }

    public class GameAlreadyStartedException : BadRequestException
    {
        public GameAlreadyStartedException(Guid gameGuid)
            : base($"Game with id {gameGuid} has already been started")
        {
        }
    }
}
