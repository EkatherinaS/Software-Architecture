namespace TrackerRepository
{
    public class DBGPS : DBEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Equals(DBGPS obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.Latitude.Equals(obj.Latitude) && this.Longitude.Equals(obj.Longitude);
        }
    }
}
