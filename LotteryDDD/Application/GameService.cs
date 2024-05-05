using LotteryDDD.Domain.Aggregates;
using LotteryDDD.Domain.Enums;
using LotteryDDD.Domain.ValueObjects;
using LotteryDDD.DTO;
using LotteryDDD.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LotteryDDD.Application
{
    public interface IGameService
    {
        Guid AddUserToGame(Guid userId);
        int MakeMove(Guid userId, Guid gameId, List<int> numbers);
        GameInfoDTO GetGameInfo(Guid gameId);
        Task LeaveGame(Guid userId);
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
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);

            var gameId = GameId.Of(Guid.NewGuid());
            var betAmount = BetAmount.Of(1000);
            var allGames = _dbContext.Games.ToList();
            var game = Game.CreateOrReturnExisting(gameId, betAmount, allGames);
            var gameIsAttached = _dbContext.Games.Local.Any(x => x.Id == game.Id);
            if (!gameIsAttached)
                _dbContext.Games.Add(game);
            var allGameUsers = _dbContext.GameUsers.Where(x => x.GameId == game.Id).ToList();
            var userReports = _dbContext.Reports.Where(x => x.UserId == userId).ToList();
            game.AddUser(user, allGameUsers, userReports);
            _dbContext.SaveChanges();
            return game.Id;
        }

        public int MakeMove(Guid userId, Guid gameId, List<int> numbers) {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            var game = _dbContext.Games.FirstOrDefault(x => x.Id == gameId);
            var allScores = _dbContext.Scores.ToList();

            var score = game.GuessNumbers(user.Id, numbers, allScores);
            _dbContext.SaveChanges();
            return score;
        }

        public GameInfoDTO GetGameInfo(Guid gameId)
        {
            var game = _dbContext.Games.FirstOrDefault(x => x.Id == gameId);
            var winnerId = game.GetWinnerId();
            var winner = _dbContext.Users.FirstOrDefault(x => x.Id == winnerId);
            return new GameInfoDTO
            {
                GameId = game.Id,
                Status = game.Status,
                WinnerUsername = winner?.Username.Value
            };
        }

        public async Task LeaveGame(Guid userId)
        {
            //var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            var game = _dbContext.Games.SingleOrDefault(x => x.Users.Any(u => u.UserId == userId));
            game.RemoveUser(userId);
            await _dbContext.SaveChangesAsync();
        }
    }
}
