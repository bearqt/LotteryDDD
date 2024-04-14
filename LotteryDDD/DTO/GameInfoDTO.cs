using LotteryDDD.Domain.Enums;

namespace LotteryDDD.DTO
{
    public class GameInfoDTO
    {
        public Guid GameId { get; set; }
        public GameStatus Status { get; set; }
        public string? WinnerUsername { get; set; }
    }
}
