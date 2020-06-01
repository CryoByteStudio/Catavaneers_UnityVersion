namespace Learning.Data
{
    [System.Serializable]
    public class SaveData
    {
        public string profileName = "New Profile";
        public string timeStamp;
        public float masterVolume;
        public float sfxVolume;
        public float musicVolume;
        public int levelUnlocked;
        public string hashValue = string.Empty;

        #region CONSTRUCTORS

        public SaveData()
        {
            timeStamp = string.Empty;
            masterVolume = 0f;
            sfxVolume = 0f;
            musicVolume = 0f;
            levelUnlocked = 1;
        }

        public SaveData(string profileName)
        {
            this.profileName = profileName;
            timeStamp = string.Empty;
            masterVolume = 0f;
            sfxVolume = 0f;
            musicVolume = 0f;
            levelUnlocked = 1;
        }

        #endregion
    }
}