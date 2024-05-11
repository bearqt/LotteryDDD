using LotteryDDD.Domain.ValueObjects;
using LotteryDDD.DTO;
using LotteryDDD.Infrastructure.Data;
using MediatR;

namespace LotteryDDD.Features
{
    public record GetGameInfoQuery(Guid gameId) : IRequest<GameInfoDTO>;

    public class GetGameInfoQueryHandler : IRequestHandler<GetGameInfoQuery, GameInfoDTO>
    {
        private readonly EfDbContext _dbContext;
        public GetGameInfoQueryHandler(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GameInfoDTO> Handle(GetGameInfoQuery request, CancellationToken cancellationToken)
        {
            var game = _dbContext.Games.FirstOrDefault(x => x.Id == request.gameId);
            var winnerId = game.GetWinnerId();
            var winner = _dbContext.Users.FirstOrDefault(x => x.Id == winnerId);
            return new GameInfoDTO
            {
                GameId = game.Id,
                Status = game.Status,
                WinnerUsername = winner?.Username.Value
            };
        }
    }
}
