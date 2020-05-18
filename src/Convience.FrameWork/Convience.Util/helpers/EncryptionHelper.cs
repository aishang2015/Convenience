using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Convience.Util.Helpers
{
    public static class EncryptionHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        public static string MD5Encrypt(string str)
        {
            using (var md5 = MD5.Create())
            {
                byte[] buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var sb = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Des加密
        /// </summary>
        public static string DesEncrypt(string str, string key)
        {
            using (var des = DES.Create())
            {
                var array = Encoding.UTF8.GetBytes(str);

                // 设置密钥和向量
                des.Key = Encoding.ASCII.GetBytes(MD5Encrypt(key).Substring(0, 8));
                des.IV = Encoding.ASCII.GetBytes(MD5Encrypt(MD5Encrypt(key)).Substring(0, 8));
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                cStream.Write(array, 0, array.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
        }

        /// <summary>
        /// Des解密
        /// </summary>
        public static string DesDecrypt(string str, string key)
        {
            using (var des = DES.Create())
            {
                var array = Convert.FromBase64String(str);

                // 设置密钥和向量
                des.Key = Encoding.ASCII.GetBytes(MD5Encrypt(key).Substring(0, 8));
                des.IV = Encoding.ASCII.GetBytes(MD5Encrypt(MD5Encrypt(key)).Substring(0, 8));
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                cStream.Write(array, 0, array.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
        }
    }
}
