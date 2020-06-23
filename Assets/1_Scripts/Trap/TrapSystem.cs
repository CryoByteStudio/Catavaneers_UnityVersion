using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSystem : MonoBehaviour
{
    [SerializeField] TrapScriptable CurrentTrap;
    [SerializeField] TrapScriptable trap1;
    [SerializeField] TrapScriptable trap2;
    //[SerializeField] Transform TrapSpawnLocation;

    [SerializeField]private Material MatFlash;
    private Material MatDefault;
    [SerializeField]private SkinnedMeshRenderer sr;

    private float DPadX;
    public string trapButton;
    public string dpadAxis;

    private void Start()
    {
        MatDefault = sr.material;
        //MatFlash = Resources.Load("FlashMaterial", typeof(Material)) as Material;
    }

    private void Update()
    {
        if(trap1 != null) CurrentTrap = trap1;
        else if (trap1 == null && trap2 != null) CurrentTrap = trap2;
        else CurrentTrap = null;

        if(Input.GetButtonDown(trapButton))
        {
            Debug.Log("trapbutton");
            if (CurrentTrap == null) return;
            CurrentTrap.SpawnTrap(transform.position);
            if(CurrentTrap == trap1) trap1 = null;
            else if(CurrentTrap == trap2) trap2 = null;
        }

        float X = Input.GetAxisRaw(dpadAxis);

        if(DPadX != X)
        {
            if(X == -1 || X == 1)
            {
                TrapScriptable Temp = trap1;
                trap1 = trap2;
                trap2 = Temp;
            }
        }

        DPadX = X;
    }

    public int CheckHasTrap()
    {
        if (trap1&&trap2)
        {
            return 3;
        }else if (trap1)
        {
            return 1;
        }else if (trap2)
        {
            return 2;
        }
        else { return 0; }
    }

    public void EquipTrap(TrapScriptable TrapInShop)
    {
        if (trap1 == null) trap1 = TrapInShop;
        else if (trap2 == null) trap2 = TrapInShop;
        else if (trap1 != null && trap2 != null)
        {
            trap2 = trap1;
            trap1 = TrapInShop;
        }
    }

    public void Flash()
    {
        StartCoroutine(BlinkEffect());
    }

    private void ResetMaterial()
    {
        sr.material = MatDefault;
    }

    private void SetFlash()
    {
        sr.material = MatFlash;
    }

    IEnumerator BlinkEffect()
    {
        for(int i = 5; i >=0; i--)
        {
            yield return new WaitForSeconds(0.1f);
            SetFlash();
            yield return new WaitForSeconds(0.1f);
            ResetMaterial();
        }
    }
}
