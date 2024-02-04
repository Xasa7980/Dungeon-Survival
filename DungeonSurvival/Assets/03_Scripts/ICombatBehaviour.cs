using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatBehaviour
{
    public void OnAnimationEvent_AttackCallback ( );
    public void OnAnimationEvent_AttackEffectCallback ( );

}
