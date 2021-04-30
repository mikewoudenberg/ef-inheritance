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
        public Model.BreadPreferences ToModel()
        {
            return new Model.BreadPreferences()
            {
                Wheat = Wheat,
                Seeds = Seeds
            };
        }
    }

    public static class BreadPreferencesExtensions
    {
        public static BreadPreferences ToDataContract(this Model.BreadPreferences preferences)
        {
            return new BreadPreferences(preferences.Wheat, preferences.Seeds);
        }
    }
}