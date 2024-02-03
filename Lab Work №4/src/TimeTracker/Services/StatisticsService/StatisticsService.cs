using System.Globalization;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.DBServices;

namespace Tracker.TelegramBot.Services
{
    public class StatisticsService
    {
        internal DBTaskService mongoDBTaskService;

        public StatisticsService()
        {
            mongoDBTaskService = new DBTaskService();
        }

        internal IEnumerable<DateTime> EachDay(DateTime startPeriod, DateTime endPeriod, bool orderAscending)
        {
            if (orderAscending)
            {
                for (var day = startPeriod.Date; day.Date <= endPeriod.Date; day = day.AddDays(1))
                    yield return day;
            }
            else
            {
                for (var day = endPeriod.Date; day.Date >= startPeriod.Date; day = day.AddDays(-1))
                    yield return day;
            }
        }

        internal List<DBTask> getGivenPeriodTasks(DateTime startPeriod, DateTime endPeriod)
        {
            List<DBTask> tasks = mongoDBTaskService.GetAll();
            return tasks.Where(x => x.StartTime.Value.Date >= startPeriod && x.StartTime.Value.Date <= endPeriod).ToList();

        }

        internal static DateTime[] ParseDates(string? text)
        {
            //дд.мм.гггг - дд.мм.гггг

            if (string.IsNullOrWhiteSpace(text))
                return new DateTime[0];

            try
            {
                string[] strings = text.Split("-");
                DateTime[] datePeriod = new DateTime[strings.Length];
                for (int i = 0; i < strings.Length; i++)
                {
                    datePeriod[i] = DateTime.ParseExact(strings[i].Trim(' '), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                }
                return datePeriod;
            }
            catch (Exception ex) { return new DateTime[0]; }
        }

        internal async Task<Dictionary<DateTime, int>> getUserStatistics(DBUser user, DateTime startPeriod, DateTime endPeriod)
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
    }
}
