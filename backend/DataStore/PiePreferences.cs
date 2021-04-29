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
    }
}