using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSystem : MonoBehaviour
{
    //[SerializeField] TrapScriptable CurrentTrap;
    [SerializeField] TrapScriptable trap1;
    [SerializeField] TrapScriptable trap2;
    //[SerializeField] Transform TrapSpawnLocation;

    [SerializeField]private Material MatFlash;
    private Material MatDefault;
    [SerializeField]private SkinnedMeshRenderer sr;

    private float DPadX;
    public string trapButton;
    public string dpadAxis;

    public event Action<Sprite> OnTrap1Updated;
    public event Action<Sprite> OnTrap2Updated;
    public event Action OnSomethingHappen;

    private void Start()
    {
        MatDefault = sr.material;
        //MatFlash = Resources.Load("FlashMaterial", typeof(Material)) as Material;
    }

    private void Update()
    {
        //if(trap1 != null) CurrentTrap = trap1;
        //else if (trap1 == null && trap2 != null) CurrentTrap = trap2;
        //else CurrentTrap = null;

        if (Input.GetButtonDown(trapButton))
        {
            Debug.Log("trapbutton");

            //CurrentTrap = trap1;

            //if (CurrentTrap == null) return;

            //CurrentTrap.SpawnTrap(transform.position);

            //if (CurrentTrap == trap1)
            //{
            //    trap1 = null;

            //    if (trap2)
            //    {
            //        trap1 = trap2;
            //        trap2 = null;
            //        CurrentTrap = trap1;
            //        if (OnTrap1Updated != null)
            //            OnTrap1Updated.Invoke(trap1.sprite);
            //        if (OnTrap2Updated != null)
            //            OnTrap2Updated.Invoke(null);
            //    }
            //    else
            //    {
            //        CurrentTrap = trap1;
            //        if (OnTrap1Updated != null)
            //            OnTrap1Updated.Invoke(null);
            //    }
            //}
            //else if (CurrentTrap == trap2)
            //{
            //    trap2 = null;
            //    CurrentTrap = trap1;
            //    if (OnTrap2Updated != null)
            //        OnTrap2Updated.Invoke(null);
            //}

            if (!trap1) return;

            trap1.SpawnTrap(transform.position);

            if (trap2)
            {
                trap1 = trap2;
                UpdateTrap1UI(trap1.sprite);
                trap2 = null;
                UpdateTrap2UI(null);
            }
            else
            {
                trap1 = null;
                UpdateTrap1UI(null);
            }

        }

        float X = Input.GetAxisRaw(dpadAxis);

        if(DPadX != X && trap1 && trap2)
        {
            if(X == -1 || X == 1)
            {
                TrapScriptable Temp = trap1;
                trap1 = trap2;
                UpdateTrap1UI(trap1.sprite);
                trap2 = Temp;
                UpdateTrap2UI(trap2.sprite);
            }
        }

        DPadX = X;
    }

    private void UpdateTrap1UI(Sprite sprite)
    {
        if (OnTrap1Updated != null)
            OnTrap1Updated.Invoke(sprite);
    }

    private void UpdateTrap2UI(Sprite sprite)
    {
        if (OnTrap2Updated != null)
            OnTrap2Updated.Invoke(sprite);
    }

    public int CheckHasTrap()
    {
        if (trap1 && trap2)
        {
            return 3;
        }
        else if (trap1)
        {
            return 1;
        }
        else if (trap2)
        {
            return 2;
        }
        else { return 0; }
    }

    public void EquipTrap(TrapScriptable trapInShop)
    {
        if (!trap1)
        {
            trap1 = trapInShop;
            UpdateTrap1UI(trap1.sprite);
        }
        else if (!trap2)
        {
            trap2 = trapInShop;
            UpdateTrap2UI(trap2.sprite);
        }
        else
        {
            trap2 = trap1;
            UpdateTrap2UI(trap2.sprite);
            trap1 = trapInShop;
            UpdateTrap1UI(trap1.sprite);
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
