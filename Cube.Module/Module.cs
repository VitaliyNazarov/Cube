using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using Cube.Model.Contexts;
using Cube.Model.Enums;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.EF;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;

namespace Cube.Module
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class CubeModule : ModuleBase
    {
        static CubeModule()
        {
            //DevExpress.Data.Linq.CriteriaToEFExpressionConverter.SqlFunctionsType = typeof(System.Data.Entity.SqlServer.SqlFunctions);
            DevExpress.Data.Linq.CriteriaToEFExpressionConverter.EntityFunctionsType = typeof(System.Data.Entity.DbFunctions);
            DevExpress.ExpressApp.SystemModule.ResetViewSettingsController.DefaultAllowRecreateView = false;
            // Uncomment this code to delete and recreate the database each time the data model has changed.
            // Do not use this code in a production environment to avoid data loss.
            // #if DEBUG
            // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CubeDbContext>());
            // #endif 
            //var efDemoDbContext = new CubeDbContext(null);  
            //MetadataWorkspace metadataWorkspace = ((IObjectContextAdapter)efDemoDbContext).ObjectContext.MetadataWorkspace;  
            //EFTypeInfoSource efTypeInfoSource = new EFTypeInfoSource(XafTypesInfo.Instance, typeof(CubeDbContext), metadataWorkspace);  
            //((TypesInfo)XafTypesInfo.Instance).AddEntityStore(efTypeInfoSource);  
        }

        public CubeModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            EnumProcessingHelper.RegisterEnum(typeof(OrderState));
            EnumProcessingHelper.RegisterEnum(typeof(OrderType));
            EnumProcessingHelper.RegisterEnum(typeof(ProductUnit));
            EnumProcessingHelper.RegisterEnum(typeof(FacadeType));
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }

        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
        }
    }
}
