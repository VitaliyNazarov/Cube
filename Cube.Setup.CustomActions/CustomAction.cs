using System;
using System.Diagnostics;
using Microsoft.Deployment.WindowsInstaller;

namespace Cube.Setup.CustomActions
{
    public class CustomActions
    {
        private const string RegistryKeyPath = "Software\\Cube\\";
        private static byte[] EtalonData = Guid.Parse("{B49C2E64-6C01-4481-AD0D-C0506C10402D}").ToByteArray();
        private static byte[] Entropy = Guid.Parse("67E18D1C-4596-4D7E-92C6-A8070014616A").ToByteArray();

        [CustomAction]
        public static ActionResult WriteProductToken(Session session)
        {
            //Debugger.Launch();
            session.Log("Begin WriteProductToken");
            var protection = new CopyProtection(RegistryKeyPath, EtalonData, Entropy);
            protection.Write();
            return ActionResult.Success;
        }
    }
}
