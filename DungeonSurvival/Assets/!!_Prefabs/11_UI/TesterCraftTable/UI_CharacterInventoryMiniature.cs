using System.Collections;
using UnityEngine;

public class UI_CharacterInventoryMiniature : MonoBehaviour
{
    public Animator anim { get; private set; }

    public void Init(RuntimeAnimatorController controller)
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = controller;
    }
}