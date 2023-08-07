using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    [SerializeField] string _displayName;
    [SerializeField] Sprite _icon;

    public string displayName => _displayName;
    public Sprite icon => _icon;
}
