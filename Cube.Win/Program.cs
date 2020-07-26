using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.XtraEditors;

namespace Cube.Win
{
    static class Program
    {
        private const string RegistryKeyPath = "Software\\Cube\\";
        private static byte[] EtalonData = Guid.Parse("{B49C2E64-6C01-4481-AD0D-C0506C10402D}").ToByteArray();
        private static byte[] Entropy = Guid.Parse("67E18D1C-4596-4D7E-92C6-A8070014616A").ToByteArray();
        private static CopyProtection _protection;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _protection = new CopyProtection(RegistryKeyPath, EtalonData, Entropy);
            _protection.Write();

            WindowsFormsSettings.LoadApplicationSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            if (Tracing.GetFileLocationFromSettings() == FileLocation.CurrentUserApplicationDataFolder)
            {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            CubeWindowsFormsApplication winApplication = new CubeWindowsFormsApplication();
            winApplication.SetLanguage("ru");
            SecurityStrategy security = (SecurityStrategy)winApplication.Security;
            security.RegisterEFAdapterProviders();
            if (ConfigurationManager.ConnectionStrings["CubeDbContext"] != null)
            {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["CubeDbContext"].ConnectionString;
            }
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            try
            {
                CheckCorruption();
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.StopSplash();
                winApplication.HandleException(e);
            }
        }

        private static void CheckCorruption()
        {
            try
            {
                _protection.CheckCorruption();
            }
            catch (Exception exception)
            {
                Tracing.LogError(Guid.NewGuid(), exception);
                throw new Exception("Обнаружена попытка несанкционированного доступа. Обратитесь к администратору.");
            }
        }
    }
}
