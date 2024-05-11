using LotteryDDD.Domain.Aggregates;
using LotteryDDD.Domain.ValueObjects;
using LotteryDDD.Infrastructure.Data;
using MediatR;

namespace LotteryDDD.Features
{
    public record AddUserToGameCommand(Guid userId) : IRequest<AddUserToGameResult>
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }

    public record AddUserToGameResult(Guid gameId);

    public class AddUserToGameCommandHandler : IRequestHandler<AddUserToGameCommand, AddUserToGameResult>
    {
        private readonly EfDbContext _dbContext;
        public AddUserToGameCommandHandler(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AddUserToGameResult> Handle(AddUserToGameCommand request, CancellationToken cancellationToken)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == request.userId);

            var gameId = GameId.Of(Guid.NewGuid());
            var betAmount = BetAmount.Of(1000);
            var allGames = _dbContext.Games.ToList();
            
            var game = Game.CreateOrReturnExisting(gameId, betAmount, allGames);
            var gameIsAttached = _dbContext.Games.Local.Any(x => x.Id == game.Id);
            if (!gameIsAttached)
                _dbContext.Games.Add(game);
            var allGameUsers = _dbContext.GameUsers.Where(x => x.GameId == game.Id).ToList();
            var userReports = _dbContext.Reports.Where(x => x.UserId == request.userId).ToList();
            game.AddUser(user, allGameUsers, userReports);
            await _dbContext.SaveChangesAsync();
            return new AddUserToGameResult(game.Id.Value);
        }
    }
}
