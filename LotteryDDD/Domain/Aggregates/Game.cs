using LotteryDDD.Domain.Common;
using LotteryDDD.Domain.Enums;
using LotteryDDD.Domain.Exceptions;
using LotteryDDD.Domain.ValueObjects;

namespace LotteryDDD.Domain.Aggregates
{
    public class Game : Aggregate<GameId>
    {
        public GameStatus Status { get; private set; }
        public BetAmount BetAmount { get; private set; }
        public List<GameUser> Users { get; set; } = new();

        public static Game Create(GameId id, BetAmount bet)
        {
            var game = new Game
            {
                Id = id,
                BetAmount = bet,
                Status = GameStatus.Created
            };

            return game;
        }

        public void AddUser(User user, List<GameUser> allGameUsers)
        {
            if (Status != GameStatus.Created)
                throw new GameAlreadyStartedException(Id);

            var userExists = allGameUsers.Any(x => x.UserId.Value == user.Id.Value);
            if (userExists)
                throw new UserIsAlreadyInGameException(Id, user.Id);

            var newGameUser = GameUser.Create(user.Id, Id);
            allGameUsers.Add(newGameUser);
        }
    }
}
