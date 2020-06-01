using UnityEngine;
using System.IO;
using Learning.Data.Cryptography;
using Learning.Utils;

namespace Learning.Data
{
    public static class JsonSaver
    {
        private static string fileExtension = ".sav";

        #region PRIVATE STATIC METHODS

        private static void LoadFailed()
        {
            EditorHelper.LogWarning(null, "Invalid data! Aborting loading...");
        }

        private static bool ValidateData(string json)
        {
            SaveData tempSaveData = new SaveData();
            JsonUtility.FromJsonOverwrite(json, tempSaveData);
            string oldHash = tempSaveData.hashValue;

            tempSaveData.hashValue = string.Empty;
            string tempJson = JsonUtility.ToJson(tempSaveData);
            string newHash = Sha256Crypto.Encrypt(tempJson);

            return oldHash == newHash;
        }

        private static void WriteToFile(string content, string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            using (StreamWriter sWriter = new StreamWriter(fileStream))
            {
                sWriter.Write(content);
            }
        }

        #endregion

        #region PUBLIC STATIC METHODS

        public static string GetSaveFilePath(string fileName)
        {
            return Application.persistentDataPath + "/" + fileName + fileExtension;
        }

        public static void Save(string fileName, SaveData data, bool encrypted)
        {
            data.hashValue = string.Empty;
            string json = JsonUtility.ToJson(data);
            string filePath = GetSaveFilePath(fileName);

            data.hashValue = Sha256Crypto.Encrypt(json);
            json = JsonUtility.ToJson(data);

            if (encrypted)
            {
                string encryptedJson = AesCrypto.Encrypt(json);
                WriteToFile(encryptedJson, filePath);

                EditorHelper.Log(null, "File saved to: " + GetSaveFilePath(fileName));
            }
            else
            {
                WriteToFile(json, filePath);

                EditorHelper.Log(null, "File saved to: " + GetSaveFilePath(fileName));
            }
        }

        public static bool Load(string fileName, SaveData data, bool encrypted)
        {
            string filePath = GetSaveFilePath(fileName);

            if (File.Exists(filePath))
            {
                using (StreamReader sReader = new StreamReader(filePath))
                {
                    string content = sReader.ReadToEnd();

                    if (encrypted)
                    {
                        // base 64 encoded string always has a length that is multiple of 4
                        if (content.Length % 4 == 0)
                        {
                            string decryptedContent = AesCrypto.Decrypt(content);

                            if (ValidateData(decryptedContent))
                            {
                                JsonUtility.FromJsonOverwrite(decryptedContent, data);
                                EditorHelper.Log(null, "File loaded from: " + GetSaveFilePath(fileName));
                                return true;
                            }
                            LoadFailed();
                        }
                        LoadFailed();
                    }
                    else
                    {
                        if (ValidateData(content))
                        {
                            JsonUtility.FromJsonOverwrite(content, data);
                            EditorHelper.Log(null, "File loaded from: " + GetSaveFilePath(fileName));
                            return true;
                        }
                        LoadFailed();
                    }
                }
            }

            return false;
        }
        
        public static void Delete(string fileName)
        {
            File.Delete(GetSaveFilePath(fileName));
        }

        #endregion
    }
}