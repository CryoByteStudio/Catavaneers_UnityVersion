using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public interface IEntityInfo
    {
        float Health { get; }
        void SetHealth(float health);
        Vector3 Position { get; }
    }
}