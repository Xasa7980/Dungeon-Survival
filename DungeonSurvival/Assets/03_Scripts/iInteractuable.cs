using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface iInteractuable
{
    public bool IsInteractuable { get; set; }
    public float InteractCounter { get; set; }
    public float InteractTime { get; set; }
}

