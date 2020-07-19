using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Cube.Model.Contexts;
using DevExpress.ExpressApp.EF.DesignTime;

namespace Cube.Module.Contexts
{
    public class CubeContextInitializer : DbContextTypesInfoInitializerBase
    {
        protected override DbContext CreateDbContext()
        {
            var contextInfo = new DbContextInfo(typeof(CubeDbContext), new DbConnectionInfo(nameof(CubeDbContext), "System.Data.SQLite"));
            //new DbProviderInfo(providerInvariantName: "System.Data.SQLite", providerManifestToken: string.Empty));
            return contextInfo.CreateInstance();
        }
    }
}