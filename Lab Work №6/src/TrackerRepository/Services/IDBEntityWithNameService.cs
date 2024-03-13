namespace TrackerRepository
{
    public interface IDBEntityWithNameService<T>
    {
        public Task<T> getByName(string name);
    }
}
