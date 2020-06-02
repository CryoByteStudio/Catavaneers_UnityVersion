using UnityEngine;

namespace Learning.LevelManagement
{
    [System.Serializable]
    public class MissionSpecs
    {
        [SerializeField] protected string name;
        [SerializeField] [Multiline] protected string description;
        [SerializeField] protected string sceneName;
        [SerializeField] protected int id;
        [SerializeField] protected Sprite image;

        public string Name => name;
        public string Description => description;
        public string SceneName => sceneName;
        public int ID => id;
        public Sprite Image => image;
    } 
}
