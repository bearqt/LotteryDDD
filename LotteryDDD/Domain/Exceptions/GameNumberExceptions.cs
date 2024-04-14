using LotteryDDD.Domain.Common;

namespace LotteryDDD.Domain.Exceptions
{
    public class InvalidGameNumberException : BadRequestException
    {
        public InvalidGameNumberException(int number)
            : base($"Invalid game number: {number}")
        {
        }
    }

    public class InvalidGameNumberIdException : BadRequestException
    {
        public InvalidGameNumberIdException(Guid guid)
            : base($"Invalid game number id: {guid}")
        {
        }
    }
    public class InvalidLengthOfNumbersException : BadRequestException
    {
        public InvalidLengthOfNumbersException(int length)
            : base($"The required length of numbers list is {length}")
        {
        }
    }
}
