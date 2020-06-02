using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMathLibrary;

public class DropController : MonoBehaviour
{
    [SerializeField] private List<DropItemType> possibleDropItems = new List<DropItemType>();
    [SerializeField] [Range(1, 10)] private int amountToDrop = 0;
    public List<DropItemType> PossibleDropItems { get { return possibleDropItems; } }
    public int AmountToDrop { get { return amountToDrop; } }
    private Vector3 randomPosition;

    /// <summary>
    /// Drop item
    /// </summary>
    public void DropItem()
    {
        if (possibleDropItems.Count == 0)
        {
            Debug.LogWarning(this + ": Posible Drop Items was not set in Drop Controller");
            return;
        }

        for (int i = 0; i < amountToDrop; i++)
        {
            randomPosition = CustomMathf.RandomPointInCirclePerpendicularToAxis(2f, Axis.Y) + transform.position;
            DropManager.DropItem(possibleDropItems[Random.Range(0, possibleDropItems.Count)], randomPosition);
        }
    }
}
