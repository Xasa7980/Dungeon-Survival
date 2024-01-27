using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState
{
    Basic,
    Recharged,
    Special,
    Skill
}
[CreateAssetMenu(fileName = "NewCombatBehaviour", menuName = "Dungeon Survival/Enemy/Combat/Combat Behaviour")]
public class CombatBehaviourSO : ScriptableObject
{
    public SpecialAttacksSO specialAtkData;
    public SpecialAttacksSO skillAtkData;
}
