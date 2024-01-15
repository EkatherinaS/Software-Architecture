namespace Tracker.MongoDB.DBEntities;
public class DBUser : DBEntity
{
    public long IdChat { get; set; }
    public string? FIO { get; set; }
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public string? Status { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string? Company { get; set; }
    public DBTask? CurrentTask { get; set; }
}
