using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Shell.Core.Encryption
{
    public class Encryption
    {
        public static string ComputeHash(string text, string salt)
        {
            string token = string.Concat(salt, text);
            byte[] bytes = new byte[token.Length];
            using (HashAlgorithm hashAlgorithm = SHA256.Create())
            {
                byte[] hashedBytes = hashAlgorithm.ComputeHash(bytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static string Encrypt(string clearText, string salt)
        {
            return string.Empty;
        }

        public static string Decrypt(string encryptedText)
        {
            return string.Empty;
        }
    }
}
