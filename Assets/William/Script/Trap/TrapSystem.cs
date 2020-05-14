using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSystem : MonoBehaviour
{
    [SerializeField] TrapScriptable CurrentTrap;
    [SerializeField] TrapScriptable Trap1;
    [SerializeField] TrapScriptable Trap2;
    [SerializeField] Transform TrapSpawnLocation;

    private float DPadX;
    public string trapButton;
    public string dpadAxis;
    private void Update()
    {
        if(Trap1 != null) CurrentTrap = Trap1;
        else if (Trap1 == null && Trap2 != null) CurrentTrap = Trap2;
        else CurrentTrap = null;

        if(Input.GetButtonDown(trapButton))
        {
            Debug.Log("trapbutton");
            if (CurrentTrap == null) return;
            CurrentTrap.SpawnTrap(transform.position);
            if(CurrentTrap == Trap1) Trap1 = null;
            else if(CurrentTrap == Trap2) Trap2 = null;
        }

        float X = Input.GetAxis(dpadAxis);

        if(DPadX != X)
        {
            if(X == -1 || X == 1)
            {
                TrapScriptable Temp = Trap1;
                Trap1 = Trap2;
                Trap2 = Temp;
            }
        }

        DPadX = X;
    }

    public int CheckHasTrap()
    {
        if (Trap1&&Trap2)
        {
            return 3;
        }else if (Trap1)
        {
            return 1;
        }else if (Trap2)
        {
            return 2;
        }
        else { return 0; }
    }

    public void EquipTrap(TrapScriptable TrapInShop)
    {
        if (Trap1 == null) Trap1 = TrapInShop;
        else if (Trap2 == null) Trap2 = TrapInShop;
        else if (Trap1 != null && Trap2 != null)
        {
            Trap2 = Trap1;
            Trap1 = TrapInShop;
        }
    }
}
