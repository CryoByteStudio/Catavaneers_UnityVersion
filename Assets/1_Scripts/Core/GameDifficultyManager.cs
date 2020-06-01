using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficultyManager : MonoBehaviour
{
    public DifficultyLevel dif; 
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

}
