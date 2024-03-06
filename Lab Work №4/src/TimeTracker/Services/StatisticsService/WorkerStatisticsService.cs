using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.DBServices;

namespace Tracker.TelegramBot.Services
{
    public class WorkerStatisticsService : StatisticsService, IUserStatisticsService
    {
        private DBUserService mongoDBUserService;
        public WorkerStatisticsService()
        {
            mongoDBUserService = new DBUserService();
        }

        public async Task<string> getShortStatistics(DBUser user, string lineStart, DateTime startPeriod, DateTime endPeriod)
        {
            Dictionary<DateTime, int> stats = await getUserStatistics(user, startPeriod, endPeriod);
            int fullValue = stats.Values.Sum();
            lineStart += string.Format(": {0} ч {1} мин\n", fullValue / 60, fullValue % 60);
            return lineStart;
        }

        public async Task<Dictionary<DateTime, int>> getFullStatistics(DBUser user, DateTime startPeriod, DateTime endPeriod)
        {
            int daysInGivenPeriod = Convert.ToInt32((endPeriod - startPeriod).TotalDays);
            Dictionary<DateTime, int> stats = new Dictionary<DateTime, int>();
            List<DBTask> tasks = getGivenPeriodTasks(startPeriod, endPeriod);

            foreach (DateTime date in EachDay(startPeriod, endPeriod, false))
            {
                stats.Add(date, tasks
                    .Where(x => x.ChatId == user.IdChat && x.StartTime.Value.Day == date.Day)
                    .Select(x => Convert.ToInt32(x.EndTime.Value.Subtract(x.StartTime.Value).TotalMinutes))
                    .Sum());
            }

            DateTime today = DateTime.Now.Date;
            if (user.CurrentTask != null)
            {
                if (user.CurrentTask.StartTime != null && user.CurrentTask.EndTime == null && today >= startPeriod.Date && today <= endPeriod.Date)
                {
                    stats[today] += Convert.ToInt32(DateTime.UtcNow.Subtract(user.CurrentTask.StartTime.Value).TotalMinutes);
                }
            }

            return stats;
        }


        public Task<Dictionary<DateTime, int>> getVisualStats(DBUser user, DateTime startPeriod, DateTime endPeriod)
        {
            //Вывод диаграмм по выбранному периоду для сотрудника
            throw new NotImplementedException();
        }
    }
}
