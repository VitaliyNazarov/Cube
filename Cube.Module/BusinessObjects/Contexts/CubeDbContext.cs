using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SQLite;
using DevExpress.ExpressApp.EF.Updating;
using DevExpress.Persistent.BaseImpl.EF;
using Role = Cube.Model.Security.Role;
using User = Cube.Model.Security.User;

namespace Cube.Model.Contexts
{
    [DbConfigurationType(typeof(SQLiteConfiguration))]
    public class CubeDbContext : DbContext
    {
        public CubeDbContext(string connectionString)
            : base(CreateConnection(), true)
        {
        }

        // wondering what to fix here  
        public CubeDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        // wondering what to fix here  
        public CubeDbContext()
            : base($"name={nameof(CubeDbContext)}")
        {
        }

        private static SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[nameof(CubeDbContext)].ConnectionString
            };
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<ProductGroup> ProductGroups { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<PriceList> PriceLists { get; set; }

        public virtual DbSet<Price> Prices { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderRow> OrderRows { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            IDatabaseInitializer<CubeDbContext> initializer = null;
#if DEBUG
            initializer = new CubeDropCreateDatabaseAlways(modelBuilder);
            //initializer = new CubeCreateDatabaseIfNotExists(modelBuilder);
#else
            initializer = new CubeCreateDatabaseIfNotExists(modelBuilder);
#endif
            Database.SetInitializer(initializer);
        }

        #region XAF
        
        public virtual DbSet<ReportDataV2> ReportDataV2 { get; set; } 

        public DbSet<ModuleInfo> ModulesInfo { get; set; }

        public DbSet<ModelDifference> ModelDifferences { get; set; }

        public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }

        #endregion
    }
}
