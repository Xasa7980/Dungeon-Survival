using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterCategory
{
    Damnned,
    Beast,
    Humanoid,
    Spirit
}
public enum MonsterRank
{
    Minion,
    Soldier,
    Guardian,
    General,
    Boss,
    EliteBoss
}

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Dungeon Survival/Enemy/Data")]
public class MonsterDataSO : ScriptableObject
{
    public MonsterCategory monsterCategory;
    public MonsterRank monsterRank;

    [Title("$monsterName"), GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    public string monsterName;
    [Header("Status")]
    public int VIT = 1;
    public int INT = 1;
    public int STR = 1;
    public int RES = 1;
    public int DEX = 1;
    public int AGI = 1;
    public int elemRES = 1;
    public float weaponAttackRange;

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
        switch (monsterRank)
        {
            case MonsterRank.Minion:

                extrasValuesMultiplier = 1f;
                minimalValue = 5f;
                maxValue = 13f;
                maxRateValue = 15f;
                break;
            case MonsterRank.Soldier:

                extrasValuesMultiplier = 1.75f;
                minimalValue = 15f;
                maxValue = 32f;
                maxRateValue = 20f;
                break;
            case MonsterRank.Guardian:

                extrasValuesMultiplier = 2.35f;
                minimalValue = 35f;
                maxValue = 80f;
                maxRateValue = 25f;
                break;
            case MonsterRank.General:

                extrasValuesMultiplier = 3f;
                minimalValue = 120f;
                maxValue = 275f;
                maxRateValue = 30f;
                break;
            case MonsterRank.Boss:

                extrasValuesMultiplier = 4f;
                minimalValue = 500f;
                maxValue = 700f;
                maxRateValue = 35f;
                break;
            case MonsterRank.EliteBoss:

                extrasValuesMultiplier = 5f;
                minimalValue = 1200f;
                maxValue = 2000f;
                maxRateValue = 50f;
                break;
            default:
                break;
        }

        //Valores dinamicos
        extraAttackPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue * 0.65f) * extrasValuesMultiplier);
        extraDefensePoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue) * extrasValuesMultiplier);
        extraElementalPower = MathF.Floor(UnityEngine.Random.Range((minimalValue / maxValue) * 100, maxValue / 100) * extrasValuesMultiplier);
        extraHealthPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue * 2) * extrasValuesMultiplier);
        extraManaPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, maxValue * 2) * extrasValuesMultiplier);

        //Valores estaticos
        extraAttackSpeed = MathF.Floor(UnityEngine.Random.Range(0, maxRateValue));

        switch (monsterCategory)
        {
            case MonsterCategory.Damnned:

                extraAttackPoints *= 1.5f;
                extraAttackSpeed *= 1.5f;
                break;

            case MonsterCategory.Beast:

                extraDefensePoints *= 1.5f;
                extraAttackSpeed *= 1.5f;
                break;

            case MonsterCategory.Humanoid:

                extraAttackPoints *= 1.5f;
                extraElementalPower *= 1.5f;
                break;

            case MonsterCategory.Spirit:

                extraHealthPoints *= 1.5f;
                extraManaPoints *= 1.5f;
                extraDefensePoints *= 1.5f;
                break;

            default:
                break;
        }
    }
}
