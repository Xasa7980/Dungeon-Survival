using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    None,
    Water,
    Fire,
    Darkness,
    Light,
    Thunder,
    Earth,
    Wind,
    Ice
}
public interface GlobalEntities 
{
    public event EventHandler OnLifeTimeEnds;
    public IEnumerator LifeTimeEnds ( float lifeTimeDelay);
    public GlobalEntities GetEntity ( ) { return this; }

}
