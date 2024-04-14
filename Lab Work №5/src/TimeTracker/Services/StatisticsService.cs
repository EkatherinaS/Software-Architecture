namespace Tracker.TelegramBot.Services
{
    public abstract class StatisticsService
    {
        public abstract string getStatistics();

    }

    public class WorkerService : StatisticsService
    {
        public override string getStatistics()
        {
            return "Worker Statistics";
        }
    }

    public class ManagerService : StatisticsService
    {
        public override string getStatistics()
        {
            return "Manager Statistics";
        }
    }
}
