using UnityEngine;
using System.Collections.Generic;

namespace Learning.LevelManagement
{
    [CreateAssetMenu(fileName = "New Mission List", menuName = "Mission List/New Mission List")]
    public class MissionList : ScriptableObject
    {
        [SerializeField] private List<MissionSpecs> missions = new List<MissionSpecs>();

        public static int TotalMission;

        private void OnEnable()
        {
            TotalMission = missions.Count;
        }

        #region PUBLIC METHODS

        public MissionSpecs GetMission(int index)
        {
            // return null if index is not within range
            return (index >= 0 && index < missions.Count) ? missions[index] : null;
        }

        #endregion
    }
}