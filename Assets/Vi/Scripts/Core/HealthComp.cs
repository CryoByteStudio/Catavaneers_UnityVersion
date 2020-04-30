﻿using UnityEngine;
using UnityEditor;
using ObjectPooling;
using UnityEngine.UI;


public enum CharacterClass { Player, Enemy, Caravan, Obj };
//public enum TrapType { None, Freeze, Damage, Slow, Reverse, Smoke };
public class HealthComp : MonoBehaviour
{
    public CharacterClass myClass;
    public float startHealth = 100;
    public bool debug;
    public float damageTakenPerSecond;
    
    private float currentHealth = 0;
    private float nextDamageTime = 0;
    private float timeElapsed = 0;
    private bool is_Dead = false;

    public Slider health_slider = null;

    private static ObjectPooler objectPooler;

    //public TrapType trapEffect = TrapType.None;
    //public float trap_timer;


    private void Start()
    {
        if(myClass == CharacterClass.Enemy)
        objectPooler = FindObjectOfType<ObjectPooler>();
        currentHealth = startHealth;
        DisplayHealth();
    }

    private void Update()
    {
        if (debug)
            TestTakeDamage();

        timeElapsed += Time.deltaTime;
    }

    /// <summary>
    /// Subtracting health by a preset amount every second for debugging purpose
    /// </summary>
    private void TestTakeDamage()
    {
        if (timeElapsed > nextDamageTime)
        {
            nextDamageTime = timeElapsed + 1f;
            TakeDamage(damageTakenPerSecond);
        }
    }

    /// <summary>
    /// Subtract health by some amount
    /// </summary>
    /// <param name="amount"> The amount that will be subtracted from health </param>
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        DisplayHealth();

        if (currentHealth == 0)
        {
            if (myClass == CharacterClass.Enemy)
            {
                SpawnManager.EnemiesAlive--;
                print(gameObject.name + " has died");
                objectPooler.SetInactive(gameObject);
            }
            is_Dead = true;
        }
    }

    /// <summary>
    /// Add to health by some amount
    /// </summary>
    /// <param name="amount"> The amount that will be added to health </param>
    public void AddHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, startHealth);
        DisplayHealth();
    }

    /// <summary>
    /// returns currentHealth amount
    /// </summary>
    public float GetCurHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// returns if character is dead
    /// </summary>
    public bool IsDead()
    {
        return is_Dead;
    }

    /// <summary>
    /// Displays health on the health slider
    /// </summary>
    private void DisplayHealth()
    {
        if(health_slider)
        health_slider.value = currentHealth;
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(HealthComp))]
//public class MyScriptEditor : Editor
//{
//    override public void OnInspectorGUI()
//    {
//        var myScript = target as HealthComp;

//        myScript.startHealth = EditorGUILayout.FloatField("Start Health", myScript.startHealth);
//        myScript.debug = GUILayout.Toggle(myScript.debug, "Debug");

//        if (myScript.debug)
//            myScript.damageTakenPerSecond = EditorGUILayout.FloatField("Damage Taken Per Second", myScript.damageTakenPerSecond);

//        myScript.myClass = (CharacterClass)EditorGUILayout.EnumFlagsField(myScript.myClass);

//    }
//}
//#endif