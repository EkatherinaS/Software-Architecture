using System.Globalization;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.DBServices;

namespace Tracker.TelegramBot.Services
{
    public interface IUserStatisticsService
    {
        public Task<string> getShortStatistics(DBUser user, string firstLine, DateTime startPeriod, DateTime endPeriod);
        public Task<Dictionary<DateTime, int>> getFullStatistics(DBUser user, DateTime startPeriod, DateTime endPeriod);
        public Task<Dictionary<DateTime, int>> getVisualStats(DBUser user, DateTime startPeriod, DateTime endPeriod);

    }
}
