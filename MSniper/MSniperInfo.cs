namespace MSniper
{
    public class MSniperInfo
    {
        public string Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1},{2}", Id, Latitude, Longitude);
        }
    }
}