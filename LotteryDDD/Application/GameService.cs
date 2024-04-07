using LotteryDDD.Domain.Aggregates;
using LotteryDDD.Domain.Enums;
using LotteryDDD.Domain.ValueObjects;
using LotteryDDD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LotteryDDD.Application
{
    public interface IGameService
    {
        public Guid AddUserToGame(Guid userId);
    }

    public class GameService : IGameService
    {
        private readonly EfDbContext _dbContext;
        public GameService(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid AddUserToGame(Guid userId)
        {
            var game = _dbContext.Games
                .Where(x => x.Status == GameStatus.Created)
                .FirstOrDefault();

            var allGameUsers = _dbContext.GameUsers.Where(x => x.GameId == game.Id).ToList();
            
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (game == null)
            {
                var gameId = GameId.Of(Guid.NewGuid());
                var betAmount = BetAmount.Of(1000);
                game = Game.Create(gameId, betAmount);
                _dbContext.Games.Add(game);
            }
            game.AddUser(user, allGameUsers);
            _dbContext.SaveChanges();
            return game.Id;
        }

    }
}
