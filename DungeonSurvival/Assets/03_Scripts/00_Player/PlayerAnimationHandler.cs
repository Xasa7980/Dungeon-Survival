using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    Animator anim;

    /// <summary>
    /// AnimatorOverrideController del Animator Component
    /// </summary>
    /// 
    private void Awake()
    {
        anim = GetComponent<Animator>();
        //En vez de usar el Asset del Animator Controller creo una instancia
        //para preservar la configuracion del Asset
        anim.runtimeAnimatorController = Instantiate(anim.runtimeAnimatorController);
    }
}
