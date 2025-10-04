using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

class Program
{
    public static class StringCipher
    {
        private const string SecretKey = "MySuperSecretKey123";

        public static string Encrypt(string plainText)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(plainText);

            using (MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider())
            {
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecretKey));
            }

            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] keyArray;
            byte[] toDecryptArray = Convert.FromBase64String(cipherText);

            using (MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider())
            {
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecretKey));
            }

            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }
    }

    static void Main()
    {
        string originalText = "Salam, bu məxfi məlumatdır.";
        Console.WriteLine($"Orijinal mətn: {originalText}");

        string encryptedText = StringCipher.Encrypt(originalText);
        Console.WriteLine($"Şifrlənmiş mətn (Base64): {encryptedText}");

        string decryptedText = StringCipher.Decrypt(encryptedText);
        Console.WriteLine($"Şifrəsi açılmış mətn: {decryptedText}");

        Console.WriteLine("\n--- Yoxlama ---");
        if (originalText == decryptedText)
        {
            Console.WriteLine("Şifrləmə və şifrə açma uğurla başa çatdı!");
        }
        else
        {
            Console.WriteLine("XƏTA: Mətnlər uyğun gəlmir!");
        }
    }
}