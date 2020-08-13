using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class PlayerInfo : MonoBehaviour, IEntityInfo
    {
        #region PUBLIC
        public float Health => health;
        public Vector3 Position => transform.position;
        
        public void SetHealth(float health)
        {
            this.health = health;
        }
        #endregion
        #region PROTECTED
        #endregion
        #region PRIVATE
        [SerializeField] private float health;
        #endregion
    }
}