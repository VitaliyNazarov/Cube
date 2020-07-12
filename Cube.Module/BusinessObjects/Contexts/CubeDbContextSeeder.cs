using Cube.Model.Helpers;
using Cube.Model.Security;

namespace Cube.Model.Contexts
{
    internal static class CubeDbContextSeeder
    {
        public static void Seed(CubeDbContext context)
        {
            context.PriceLists.Add(new PriceList {Name = "Основной прайс-лист"});

            context.SaveChanges();
        }
    }
}