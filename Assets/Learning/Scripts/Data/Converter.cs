using UnityEngine;
using System.Collections;
using System;
using System.Text;

namespace Learning.Data
{
    public static class Converter
    {
        #region PUBLIC STATIC METHODS

        public static string BytesToHex(byte[] input)
        {
            string output = string.Empty;

            foreach (byte b in input)
            {
                output += b.ToString("x2");
            }

            return output;
        }

        // https://stackoverflow.com/questions/53515566/how-to-convert-aes-byte-into-string-and-vice-versa
        // check if 2 bytes arrays is equal with byte[].SequenceEquals(byte[])
        public static string BytesToStringBase64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static byte[] StringToBytesBase64(string input)
        {
            return Convert.FromBase64String(input);
        }

        public static string BytesToStringUTF8(byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }

        public static byte[] StringToByteUTF8(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        #endregion
    }
}