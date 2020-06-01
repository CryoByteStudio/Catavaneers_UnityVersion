using System.Security.Cryptography;

namespace Learning.Data.Cryptography
{
    public static class Sha256Crypto
    {
        #region PUBLIC STATIC METHODS

        public static string Encrypt(string plainText)
        {
            SHA256Managed SHA256 = new SHA256Managed();

            return Converter.BytesToHex(SHA256.ComputeHash(Converter.StringToByteUTF8(plainText)));
        }

        #endregion
    }
}