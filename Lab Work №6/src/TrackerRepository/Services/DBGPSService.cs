namespace TrackerRepository
{
    public class DBGPSService : DBService<DBGPS>
    {
        private static string collectionName = "gps";
        public DBGPSService() : base(collectionName) { }
    }
}
