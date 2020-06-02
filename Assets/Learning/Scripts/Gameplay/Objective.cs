using UnityEngine;

namespace Learning.Gameplay
{
    public class Objective : MonoBehaviour
    {
        [SerializeField] private GameTag playerTag = GameTag.Player;
        private bool isComplete = false;
        public bool IsComplete { get { return isComplete; } }

        #region UNITY ENGINE FUNCTION

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == playerTag.ToString())
            {
                ObjectiveCompleted();
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void ObjectiveCompleted()
        {
            isComplete = true;
        }

        #endregion
    }
}