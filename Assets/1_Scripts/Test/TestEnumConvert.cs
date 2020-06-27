using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catavaneer.Extensions;
using Catavaneer.MenuSystem;

public class TestEnumConvert : MonoBehaviour
{
    [SerializeField] private int integerToConvert;
    [SerializeField] private string stringToConvert;
    [SerializeField] private IntendedLevelImage levelImage;

    public void ConvertIntToEnum()
    {
        levelImage = integerToConvert.ToEnum<IntendedLevelImage>();
    }

    public void ConvertStringToEnum()
    {
        levelImage = stringToConvert.ToEnum<IntendedLevelImage>();
    }
}