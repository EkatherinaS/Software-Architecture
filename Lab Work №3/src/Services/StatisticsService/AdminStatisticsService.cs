using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.DBServices;

namespace Tracker.TelegramBot.Services
{
    public class AdminStatisticsService : StatisticsService, IUserStatisticsService
    {
        private DBUserService mongoDBUserService;

        public AdminStatisticsService()
        {
            mongoDBUserService = new DBUserService();
        }

        public async Task<Dictionary<DateTime, int>> getFullStatistics(DBUser user, DateTime startPeriod, DateTime endPeriod)
        {
            //Полная личная статистика и статистика по сотрудникам
            throw new NotImplementedException();
        }

        public async Task<string> getShortStatistics(DBUser user, string firstLine, DateTime startPeriod, DateTime endPeriod)
        {
            //Краткая статистика личного результата и результатов по проектам
            throw new NotImplementedException();
        }

        public async Task<Dictionary<DateTime, int>> getVisualStats(DBUser user, DateTime startPeriod, DateTime endPeriod)
        {
            //Вывод личных диаграмм по выбранному периоду и диаграмм по результатам подчиненных
            throw new NotImplementedException();
        }
    }
}
