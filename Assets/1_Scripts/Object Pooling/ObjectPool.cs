using UnityEngine;

namespace ObjectPooling
{
    [System.Serializable]
    public class ObjectPool
    {
        public string name;
        public GameObject prefab;
        public int amount;
        public float activeTime;
        public Transform parent;
    }
}