namespace Tracker.TelegramBot.Controllers.Entities
{
    public class GetInfoEntity
    {
        public string? FIO { get; internal set; }
        public string TaskName { get; internal set; }
        public string ProjectName { get; internal set; }
        public DateTime? StartTime { get; internal set; }
    }
}