using Microsoft.EntityFrameworkCore;

namespace DataStores
{
    [Owned]
    public class PiePreferences
    {
        public PiePreferences(bool whippedCream, bool fruit)
        {
            WhippedCream = whippedCream;
            Fruit = fruit;
        }

        public bool WhippedCream { get; private set; }
        public bool Fruit { get; private set; }
        public Model.PiePreferences ToModel()
        {
            return new Model.PiePreferences()
            {
                Fruit = Fruit,
                WhippedCream = WhippedCream
            };
        }
    }

    public static class PiePreferencesExtensions
    {
        public static PiePreferences ToDataContract(this Model.PiePreferences preferences)
        {
            return new PiePreferences(preferences.WhippedCream, preferences.Fruit);
        }
    }
}