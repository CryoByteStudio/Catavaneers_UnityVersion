
using UnityEngine;

[CreateAssetMenu(fileName = "Trap", menuName = "Trap/Make New Trap", order = 2)]
public class TrapScriptable : ScriptableObject
{
    [SerializeField] GameObject trapPrefab = null;
    public Sprite sprite = null;

    public void SpawnTrap(Vector3 DropLocation)
    {
        Instantiate(trapPrefab, DropLocation, Quaternion.identity);
    }
}
