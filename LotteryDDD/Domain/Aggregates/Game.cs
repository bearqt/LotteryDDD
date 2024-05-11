using LotteryDDD.Domain.Aggregates.Events;
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
        public RoundNumber RoundNumber { get; private set; }
        public List<GameUser> Users { get; private set; } = new();
        public List<GameNumber> Numbers { get; private set; } = new();
        public List<Score> Scores { get; private set; } = new();

        private const int usersToStart = 5;
        private const int numbersInGame = 5;
        private const int maxNumber = 100;
        private const int maxReportsAllowed = 5;

        private static readonly Random random = new();

        public static Game Create(GameId id, BetAmount bet)
        {
            var game = new Game
            {
                Id = id,
                BetAmount = bet,
                Status = GameStatus.Created,
                RoundNumber = RoundNumber.Of(0)
            };

            return game;
        }

        public static Game CreateOrReturnExisting(GameId id, BetAmount bet, List<Game> allGames)
        {
            var existingGame = allGames.FirstOrDefault(x => x.Status == GameStatus.Created);
            if (existingGame != null)
                return existingGame;
            return new Game
            {
                Id = id,
                BetAmount = bet,
                Status = GameStatus.Created
            };
        }

        public void AddUser(User user, List<GameUser> allGameUsers, List<Report> reports)
        {
            if (Status != GameStatus.Created)
                throw new GameAlreadyStartedException(Id);

            var userExists = allGameUsers.Any(x => x.UserId.Value == user.Id.Value);
            if (userExists)
                throw new UserIsAlreadyInGameException(Id, user.Id);

            if (reports.Count > maxReportsAllowed)
                throw new UserBannedException(user.Username);

            var newGameUser = GameUser.Create(user.Id, Id);
            user.PayBet(BetAmount);
            Users.Add(newGameUser);
            if (Users.Count >= usersToStart)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            Status = GameStatus.Started;
            RoundNumber = RoundNumber.Of(1);
            GenerateNumbersForRound();
            var @event = new GameStartedDomainEvent(Id);
            AddDomainEvent(@event);
        }

        public void Restart()
        {
            Status = GameStatus.Created;
            Scores.Clear();
        }

        private void GenerateNumbersForRound()
        {
            for (int i = 0; i < numbersInGame; i++)
            {
                var gameNumberId = GameNumberId.Of(Guid.NewGuid());
                var numberValue = GameNumberValue.Of(random.Next(maxNumber + 1));
                var gameNumber = GameNumber.Create(gameNumberId, Id, numberValue);
                Numbers.Add(gameNumber);
            }
        }

        public int GuessNumbers(UserId userId, List<int> numbers, List<Score> allScores)
        {
            if (numbers.Count != numbersInGame)
                throw new InvalidLengthOfNumbersException(numbers.Count);

            var totalScore = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                totalScore += maxNumber - Math.Abs(numbers[i] - Numbers[i].NumberValue.Value);
            }
            var newScore = Score.Create(userId, Id, ScorePoints.Of(totalScore));
            Scores.Add(newScore);

            var scoresInCurrentGame = allScores.Where(x => x.GameId == Id).ToList();
            if (scoresInCurrentGame.Count() >= usersToStart)
            {
                FinishGame();
            }

            return totalScore;
        }

        private void FinishGame()
        {
            Status = GameStatus.Finished;
        }

        public UserId? GetWinnerId()
        {
            if (Status != GameStatus.Finished)
                return null;
            var winnerScore = Scores.OrderByDescending(x => x.Points).First();
            return winnerScore.UserId;
        }

        public void RemoveUser(Guid userId)
        {
            Users.RemoveAll(x => x.UserId == userId);
            Status = GameStatus.Terminated;
            var @event = new UserLeftGameDomainEvent(Id, userId);
            AddDomainEvent(@event);
        }
    }
}
