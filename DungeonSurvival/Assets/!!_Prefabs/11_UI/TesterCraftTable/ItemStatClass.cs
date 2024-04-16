using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Top Down Engine/Stat Class", fileName = "New Stat Class")]
public class ItemStatClass : ScriptableObject
{
    public enum StatType
    {
        Speed,
        Damage,
        Protection,
        Evasion,
        Range,
        Spread,
        Custom
    }

    [SerializeField] StatType _type;

    public StatType type => _type;

    public bool isCustom { get => type == StatType.Custom; }

    [SerializeField, ShowIf("_type", StatType.Custom)] string _statName;
    public string statName
    {
        get
        {
            return type == StatType.Custom ? _statName : type.ToString();
        }
    }

    [SerializeField, ShowIf("_type", StatType.Custom)] Sprite _icon;
    public Sprite icon
    {
        get
        {
            return type == StatType.Custom ? icon : Resources.Load<Sprite>("Icons/" + type.ToString());
        }
    }
}
[System.Serializable]
public class Stat
{
    public ItemStatClass statClass;
    public int value;

    public Stat ( ) { }

    public Stat ( ItemStatClass statClass, int value )
    {
        this.statClass = statClass;
        this.value = value;
    }
}