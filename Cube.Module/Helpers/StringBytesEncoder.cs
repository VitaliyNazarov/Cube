using System;
using System.Text;

namespace Cube.Model.Helpers
{
    public static class StringBytesEncoder
    {
        public static byte[] EncodeToUtf8Bytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string DecodeUtf8ToString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string EncodeBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}