using Microsoft.EntityFrameworkCore;

namespace DataStores
{
    [Owned]
    public class BreadPreferences
    {
        public BreadPreferences(bool wheat, bool seeds)
        {
            Wheat = wheat;
            Seeds = seeds;
        }

        public bool Wheat { get; private set; }
        public bool Seeds { get; private set; }
    }
}