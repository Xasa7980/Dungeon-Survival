using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerComponents : MonoBehaviour
{
    public static PlayerComponents instance;
    public StandaloneInputModule standaloneInputModule_EventSystem;
    public bool lockedAll;
    public Animator anim { get; private set; }
    private void Awake ( )
    {
        instance = this;
        anim = GetComponent<Animator>();
    }
}
