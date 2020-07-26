using System;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace Cube.Setup.CustomActions
{
    public class CopyProtection
    {
        private const string RegistryKeyName = "Protection";
        private readonly string _registryPath;
        private readonly byte[] _expectedData;
        private readonly byte[] _entropy;

        public CopyProtection(string registryPath, byte[] expectedData, byte[] entropy)
        {
            _registryPath = registryPath;
            _expectedData = expectedData;
            _entropy = entropy;
        }

        public void Write()
        {
            using (var key = Registry.LocalMachine.CreateSubKey(_registryPath))
            {
                if (key == null)
                {
                    throw new Exception("Не найдена ветка реестра.");
                }

                var encryptedData = ProtectedData.Protect(_expectedData, _entropy, DataProtectionScope.LocalMachine);

                key.SetValue(RegistryKeyName, Convert.ToBase64String(encryptedData), RegistryValueKind.String);
            }
        }
    }
}