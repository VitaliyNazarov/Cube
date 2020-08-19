using System.Data.Entity;
using System.Data.SQLite;
using DevExpress.ExpressApp.EF.Updating;
using DevExpress.Persistent.BaseImpl.EF;
using Role = Cube.Model.Security.Role;
using User = Cube.Model.Security.User;

namespace Cube.Model.Contexts
{
    //[TypesInfoInitializer(typeof(CubeContextInitializer))]
    [DbConfigurationType(typeof(CubeSQLiteConfiguration))]
    public class CubeDbContext : DbContext
    {
        private readonly bool _dropAndCreate = false;

        public CubeDbContext(string connectionString)
            : base(CreateConnection(), true)
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        public CubeDbContext(string connectionString, bool dropAndCreate)
            : base(CreateConnection(), true)
        {
            _dropAndCreate = dropAndCreate;
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
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

        public virtual DbSet<PriceGroup> PriceGroups { get; set; }

        public virtual DbSet<Price> Prices { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderRow> OrderRows { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var initializer = _dropAndCreate 
                ? (IDatabaseInitializer<CubeDbContext>)new CubeDropCreateDatabaseAlways(modelBuilder) 
                : new CubeCreateDatabaseIfNotExists(modelBuilder);

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
