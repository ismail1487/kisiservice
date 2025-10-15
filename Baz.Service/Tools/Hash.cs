using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Baz.Service.Tools
{
    /// <summary>
    /// Şifreleri encode ve decode edilebilecek class
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Şifrelemeyi benzersiz kılacak değerini bizim belirlediğimiz string
        /// </summary>
        private static readonly string encryptionKey = "orsa!maya";

        private static readonly byte[] _salt = Encoding.ASCII.GetBytes(encryptionKey);
        /// <summary>
        /// Verilen string i şifreler
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns>Şifrelenmiş string</returns>
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, _salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        /// <summary>
        /// Şifrelenmiş stringin şifresini çözer.
        /// </summary>
        /// <param name="cipherText">Şifrelenmiş string</param>
        /// <returns>Şifresi çözülen string</returns>
        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, _salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
    /// <summary>
    /// Şifreleri salt ile birlikte encode ve decode edilebilecek class
    /// </summary>
    public class HashSalt
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
        /// <summary>
        /// Hash ve salt işlemini oluşturan method.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static HashSalt GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }
        /// <summary>
        /// Salt şeklinde oluşturulan şifreyi doğrulayan method.
        /// </summary>
        /// <param name="enteredPassword">The entered password.</param>
        /// <param name="storedHash">The stored hash.</param>
        /// <param name="storedSalt">The stored salt.</param>
        /// <returns></returns>
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }
    }
}
