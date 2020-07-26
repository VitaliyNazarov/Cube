using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace Cube.Win
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

        public void CheckCorruption()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(_registryPath))
            {
                if (key == null)
                {
                    throw new Exception("Не найдена ветка реестра.");
                }
                var dataKey = key.GetValue(RegistryKeyName);
                if (dataKey == null)
                {
                    throw new Exception("Не найден ключ реестра.");
                }

                var dataValue = dataKey.ToString();
                if (string.IsNullOrWhiteSpace(dataValue))
                {
                    throw new Exception("Не найдено значение ключа реестра");
                }

                var encryptedData = Convert.FromBase64String(dataValue);
                var decryptData = ProtectedData.Unprotect(encryptedData, _entropy, DataProtectionScope.LocalMachine);

                if (!decryptData.SequenceEqual(_expectedData))
                {
                    throw new Exception("Подмена ключа");
                }
            }
        }
    }
}