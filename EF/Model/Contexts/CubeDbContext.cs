using System.Data.Entity;
using Cube.Model.Model.Security;
using SQLite.CodeFirst;

namespace Cube.Model.Model.Contexts
{
	public class CubeDbContext : DbContext
    {
        public CubeDbContext() : base("CubeDbContext")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<ProductGroup> ProductGroups { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<PriceList> PriceLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            IDatabaseInitializer<CubeDbContext> initializer = null;
#if DEBUG
            initializer = new CubeDropCreateDatabaseAlways(modelBuilder);
#else
            initializer = new CubeCreateDatabaseIfNotExists(modelBuilder);
#endif
            Database.SetInitializer(initializer);
        }
    }
}
