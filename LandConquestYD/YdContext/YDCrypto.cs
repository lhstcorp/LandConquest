using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LandConquestYD
{
    public static class YDCrypto
    {
        public static string Decrypt(string cipherText)
		{
			return Decryption(cipherText);
		}
		private static string Decryption(string cipherText)
        {
            byte[] array = Convert.FromBase64String(cipherText);
            byte[] salt = array.Take(32).ToArray();
            byte[] rgbIV = array.Skip(32).Take(32).ToArray();
            byte[] array2 = array.Skip(64).Take(array.Length - 64).ToArray();
            using (var rfc2898DeriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(new FileInfo(AppDomain.CurrentDomain.FriendlyName).Length.ToString(), salt, 1000))
            {
                byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
                using (var rijndaelManaged = new System.Security.Cryptography.RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 256;
                    rijndaelManaged.Mode = System.Security.Cryptography.CipherMode.CBC;
                    rijndaelManaged.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                    using (var transform = rijndaelManaged.CreateDecryptor(bytes, rgbIV))
                    {
                        using (var memoryStream = new MemoryStream(array2))
                        {
                            using (var cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, transform, System.Security.Cryptography.CryptoStreamMode.Read))
                            {
                                byte[] array3 = new byte[array2.Length];
                                int count = cryptoStream.Read(array3, 0, array3.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(array3, 0, count);
                            }
                        }
                    }
                }
            }
        }
    }
}
