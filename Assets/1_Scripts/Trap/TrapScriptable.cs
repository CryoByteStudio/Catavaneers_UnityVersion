
using UnityEngine;

[CreateAssetMenu(fileName = "Trap", menuName = "Trap/Make New Trap", order = 2)]
public class TrapScriptable : ScriptableObject
{
    [SerializeField] GameObject TrapPrefab = null;

    public void SpawnTrap(Vector3 DropLocation)
    {
        Instantiate(TrapPrefab, DropLocation, Quaternion.identity);
    }
}
