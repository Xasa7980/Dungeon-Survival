using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    public static PlayerComponents instance;
    public bool lockedAll;
    public Animator anim { get; private set; }
    private void Awake ( )
    {
        instance = this;
        anim = GetComponent<Animator>();
    }
}
