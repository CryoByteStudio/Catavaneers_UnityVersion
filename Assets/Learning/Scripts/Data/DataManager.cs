using UnityEngine;
using Learning.Singleton;
using Learning.Utils;
using System.Collections.Generic;

namespace Learning.Data
{
    public class DataManager : SingletonEntity<DataManager>
    {
        [SerializeField] private string fileName;
        [SerializeField] private bool encryption = true;
        [SerializeField] private int currentSaveSlot = 0;
        private const int maxSaveProfiles = 3;
        private const string emptyText = "Empty";
        private List<SaveData> saveData = new List<SaveData>(maxSaveProfiles);

        public static string EmptyText { get {return emptyText; } }
        public int CurrentSaveSlot { get { return currentSaveSlot; } set { currentSaveSlot = value; } }
        public string SaveTimeStamp { get { return saveData[CurrentSaveSlot].timeStamp; } set { saveData[CurrentSaveSlot].timeStamp = value; } }
        public float MasterVolume { get { return saveData[CurrentSaveSlot].masterVolume; } set { saveData[CurrentSaveSlot].masterVolume = value; } }
        public float SFXVolume { get { return saveData[CurrentSaveSlot].sfxVolume; } set { saveData[CurrentSaveSlot].sfxVolume = value; } }
        public float MusicVolume { get { return saveData[CurrentSaveSlot].musicVolume; } set { saveData[CurrentSaveSlot].musicVolume = value; } }
        public int LevelUnlocked { get { return saveData[CurrentSaveSlot].levelUnlocked; } set { saveData[CurrentSaveSlot].levelUnlocked = value; } }

        #region UNITY FUNCTIONS

        override protected void Awake()
        {
            base.Awake();
            CreateNewSaves();
        }

        #endregion

        #region PRIVATE METHODS

        private void CreateNewSaves()
        {
            for (int i = 0; i < maxSaveProfiles; i++)
            {
                saveData.Add(new SaveData());
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void Save()
        {
            fileName = "Slot " + (currentSaveSlot + 1);
            JsonSaver.Save(fileName, saveData[CurrentSaveSlot], encryption);
        }

        public void Save(int slotIndex)
        {
            fileName = "Slot " + (slotIndex + 1);
            currentSaveSlot = slotIndex;
            JsonSaver.Save(fileName, saveData[CurrentSaveSlot], encryption);
        }

        public bool Load()
        {
            string tempFileName = "Slot " + (currentSaveSlot + 1);
            if (!JsonSaver.Load(tempFileName, saveData[CurrentSaveSlot], encryption))
            {
                fileName = tempFileName;
                EditorHelper.LogWarning(this, "Load failed! File path not exist: " + JsonSaver.GetSaveFilePath(fileName));
                return false;
            }

            return true;
        }

        public bool Load(int slotIndex)
        {
            string tempFileName = "Slot " + (slotIndex + 1);
            if (JsonSaver.Load(tempFileName, saveData[slotIndex], encryption))
            {
                fileName = tempFileName;
                currentSaveSlot = slotIndex;
                return true;
            }
            else
            {
                EditorHelper.LogWarning(this, "Load failed! File path not exist: " + JsonSaver.GetSaveFilePath(fileName));
                return false;
            }
        }

        public void Delete()
        {
            JsonSaver.Delete(fileName);
        }

        public void Delete(string fileName)
        {
            JsonSaver.Delete(fileName);
        }

        public string GetSaveDetails(int slotIndex)
        {
            string timeStamp = saveData[slotIndex].timeStamp;
            string text = timeStamp != string.Empty ? "Slot " + (slotIndex + 1) + " - " + saveData[slotIndex].timeStamp : emptyText;
            return text == string.Empty ? emptyText : text;
        }

        #endregion
    }
}