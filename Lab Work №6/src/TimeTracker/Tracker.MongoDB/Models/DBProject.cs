namespace Tracker.MongoDB.DBEntities;
public class DBProject : DBEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Equals(DBProject obj)
    {
        if (obj == null)
        {
            return false;
        }

        return this.Name.Equals(obj.Name) &&
            (this.Description.Equals(obj.Description) || (this.Description is null && obj.Description is null));
    }
}
