using System.Security.Cryptography;
using System;
using System.IO;

namespace Learning.Data.Cryptography
{
    public static class AesCrypto
    {
        private static string KEY_STRING = "dJewJqzm93oIsaeK4PsKFIRhdoe3oPas";
        private static byte[] KEY = Converter.StringToBytesBase64(KEY_STRING);
        private static byte[] IV = new byte[16];

        #region PUBLIC STATIC METHODS

        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netcore-3.1
        // need to store a key and iv somewhere, otherwise return bad PKCS7 padding error (mismatch key/iv)
        public static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            byte[] encrypted;

            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = key;
                aesAlgorithm.IV = iv;

                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netcore-3.1
        // need to store a key and iv somewhere, otherwise return bad PKCS7 padding error (mismatch key/iv)
        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            string plainText;

            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = key;
                aesAlgorithm.IV = iv;

                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor(aesAlgorithm.Key, aesAlgorithm.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }

        // https://answers.unity.com/questions/1286146/systemsecuritycryptographycryptographicexception-b.html
        // using hard-coded key and iv is stored in backing storage memory using memory stream
        public static string Encrypt(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            byte[] encrypted;

            using (Aes aesAlgorithm = Aes.Create())
            {
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(KEY, aesAlgorithm.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlgorithm.IV, 0, aesAlgorithm.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Converter.BytesToStringBase64(encrypted);
        }

        // https://answers.unity.com/questions/1286146/systemsecuritycryptographycryptographicexception-b.html
        // using hard-coded key and iv is stored in backing storage memory using memory stream
        public static string Decrypt(string cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            string plainText;

            using (Aes aesAlgorithm = Aes.Create())
            {
                byte[] cipherBytes = Converter.StringToBytesBase64(cipherText);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    msDecrypt.Read(IV, 0, aesAlgorithm.IV.Length);

                    ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor(KEY, IV);

                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = swDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }

        #endregion
    }
}