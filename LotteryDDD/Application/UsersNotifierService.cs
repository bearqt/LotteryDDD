namespace LotteryDDD.Application
{
    public interface IUsersNotifierService
    {
        void NotifyUser(Guid userGuid, string message);
    }

    public class UsersNotifierService : IUsersNotifierService
    {
        public void NotifyUser(Guid userGuid, string message)
        {
            Console.WriteLine($"[{userGuid}]: ${message}");
        }
    }
}
