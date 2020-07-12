using System.Data.Entity;
using SQLite.CodeFirst;

namespace Cube.Model.Model.Contexts
{
    internal sealed class CubeCreateDatabaseIfNotExists : SqliteCreateDatabaseIfNotExists<CubeDbContext>
    {
        public CubeCreateDatabaseIfNotExists(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        {

        }

        protected override void Seed(CubeDbContext context)
        {
            CubeDbContextSeeder.Seed(context);
        }
    }
}