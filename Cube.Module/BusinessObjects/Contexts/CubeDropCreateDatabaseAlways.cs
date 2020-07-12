using System.Data.Entity;
using SQLite.CodeFirst;

namespace Cube.Model.Contexts
{
    internal sealed class CubeDropCreateDatabaseAlways : SqliteDropCreateDatabaseAlways<CubeDbContext>
    {
        public CubeDropCreateDatabaseAlways(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        {

        }

        protected override void Seed(CubeDbContext context)
        {
            CubeDbContextSeeder.Seed(context);
        }
    }
}