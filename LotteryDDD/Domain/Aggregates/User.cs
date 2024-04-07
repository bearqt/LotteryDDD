using LotteryDDD.Domain.Common;
using LotteryDDD.Domain.ValueObjects;

namespace LotteryDDD.Domain.Aggregates
{
    public class User : Aggregate<UserId>
    {
        public Username Username { get; private set; }
        public Balance Balance { get; private set; }   

        public static User Create(UserId id, Username username, Balance balance)
        {
            var user = new User
            {
                Id = id,
                Username = username,
                Balance = balance
            };

            return user;
        }
    }
}
