using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCombatBehaviour", menuName = "Dungeon Survival/Enemy/Combat/SkillData")]
public class SkillDataSO : ScriptableObject
{
    public enum SkillData
    {
        Damage,
        Buff,
        Debuff,
        Passive
    }
    

    public float durationTimerMax;
    public float castingTimerMax;
}
