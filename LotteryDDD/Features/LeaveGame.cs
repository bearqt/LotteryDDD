using LotteryDDD.Domain.ValueObjects;
using LotteryDDD.Infrastructure.Data;
using MediatR;

namespace LotteryDDD.Features
{
    public record LeaveGameCommand(Guid userId) : IRequest<LeaveGameResult>
    {
    }
    
    public record LeaveGameResult(Guid gameId);

    public class LeaveGameCommandHandler : IRequestHandler<LeaveGameCommand, LeaveGameResult>
    {
        private readonly EfDbContext _dbContext;
        public LeaveGameCommandHandler(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LeaveGameResult> Handle(LeaveGameCommand request, CancellationToken cancellationToken)
        {
            var game = _dbContext.Games.SingleOrDefault(x => x.Users.Any(u => u.UserId == request.userId));
            game.RemoveUser(request.userId);
            await _dbContext.SaveChangesAsync();
            return new LeaveGameResult(game.Id.Value);
        }
    }
}
