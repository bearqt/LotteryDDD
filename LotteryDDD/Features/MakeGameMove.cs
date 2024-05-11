using LotteryDDD.Domain.ValueObjects;
using LotteryDDD.Infrastructure.Data;
using MediatR;

namespace LotteryDDD.Features
{
    public record MakeMoveCommand(Guid userId, Guid gameId, List<int> numbers) : IRequest<MakeMoveResult>
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }

    public record MakeMoveResult(int score);

    public class MakeMoveCommandHandler : IRequestHandler<MakeMoveCommand, MakeMoveResult>
    {
        private readonly EfDbContext _dbContext;
        public MakeMoveCommandHandler(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MakeMoveResult> Handle(MakeMoveCommand request, CancellationToken cancellationToken)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == request.userId);
            var game = _dbContext.Games.FirstOrDefault(x => x.Id == request.gameId);
            var allScores = _dbContext.Scores.ToList();

            var score = game.GuessNumbers(user.Id, request.numbers, allScores);
            await _dbContext.SaveChangesAsync();
            return new MakeMoveResult(score);
        }
    }
}
