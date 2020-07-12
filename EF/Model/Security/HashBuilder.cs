using System.Security.Cryptography;

namespace Cube.Model.Model.Security
{
    /// <summary>
    /// Helper class for hash generation.
    /// </summary>
    public static class HashBuilder
    {
        /// <summary>
        /// Computes the SHA256 hash value for the specified byte array.
        /// </summary>
        /// <param name="data">The input to compute the hash code for.</param>
        /// <returns>The computed SHA256 hash code.</returns>
        public static byte[] CreateSha256(byte[] data)
        {
            using (var sha = new SHA512Managed())
            {
                return sha.ComputeHash(data);
            }
        }
    }
}