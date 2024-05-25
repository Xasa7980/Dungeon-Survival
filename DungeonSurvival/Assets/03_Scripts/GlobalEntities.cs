using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlobalEntities 
{
    public enum EntityPersonality
    {
        Hostile,
        Neutral,
        Pacific
    }
    public EntityPersonality entityPersonality = EntityPersonality.Hostile;


}
