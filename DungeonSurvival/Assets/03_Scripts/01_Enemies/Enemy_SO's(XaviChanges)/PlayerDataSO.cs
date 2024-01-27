using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterCategory
{
    Adventurer,
    Soldier,
    Mercenary
}

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Dungeon Survival/Player/Data")]
public class PlayerDataSO : ScriptableObject
{
    public CharacterCategory characterCategory;

    [Header("Status")]
    public int VIT = 1;
    public int INT = 1;
    public int STR = 1;
    public int RES = 1;    
    public int DEX = 1;
    public int AGI = 1;
    public int elemRES = 1;

    [Header("Extra Stats")]
    public float extraAttackPoints;
    public float extraAttackSpeed;

    public float extraDefensePoints;

    public float extraElementalPower;

    public float extraHealthPoints;
    public float extraManaPoints;

    private float extrasValuesMultiplier;

    [PropertyOrder(0)]
    [HorizontalGroup("Values")]
    [GUIColor(0.8f, 0.5f, 0.8f, 1f)]
    [Button(ButtonSizes.Large)]
    public void GetRandomValues ( )
    {
        float minimalValue = 0;
        float maxValue = 0;
        float maxRateValue = 0;
        switch (characterCategory)
        {
            case CharacterCategory.Adventurer:

                extrasValuesMultiplier = 1f;
                minimalValue = 5f;
                maxValue = 13f;
                maxRateValue = 15f;
                break;
            case CharacterCategory.Soldier:

                extrasValuesMultiplier = 1.75f;
                minimalValue = 15f;
                maxValue = 32f;
                maxRateValue = 20f;
                break;
            case CharacterCategory.Mercenary:

                extrasValuesMultiplier = 2.35f;
                minimalValue = 35f;
                maxValue = 80f;
                maxRateValue = 25f;
                break;
            default:
                break;
        }

        //Valores dinamicos
        extraAttackPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue * 0.65f) * extrasValuesMultiplier);
        extraDefensePoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue) * extrasValuesMultiplier);
        extraElementalPower = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue / 100) * extrasValuesMultiplier);
        extraHealthPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue * 3) * extrasValuesMultiplier);
        extraManaPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue * 2) * extrasValuesMultiplier);

        //Valores estaticos
        extraAttackSpeed = MathF.Floor(UnityEngine.Random.Range(0, maxRateValue));
    }
}

