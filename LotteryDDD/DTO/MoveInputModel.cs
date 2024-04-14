namespace LotteryDDD.DTO
{
    public class MoveInputModel
    {
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public List<int> Numbers { get; set; }
    }
}
