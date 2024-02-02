using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Dungeon Survival/Enemy/SpecialAttacks")]
public class SpecialAttacksSO : ScriptableObject
{
    //public const string E_CHARGING_ATTACK_BASIC_1 = "ChargingAttack_01";
    //public const string E_CHARGING_ATTACK_BASIC_2 = "ChargingAttack_02";
    //public const string E_CHARGING_ATTACK_BASIC_3 = "ChargingAttack_03";
    //public const string E_CHARGING_ATTACK_BASIC_4 = "ChargingAttack_04";

    //public const string E_CHARGED_ATTACK_BASIC_1 = "ChargedAttack_01";
    //public const string E_CHARGED_ATTACK_BASIC_2 = "ChargedAttack_02";
    //public const string E_CHARGED_ATTACK_BASIC_3 = "ChargedAttack_03";
    //public const string E_CHARGED_ATTACK_BASIC_4 = "ChargedAttack_04";

    //public const string E_SKILL_CASTING_ATTACK_BASIC_1 = "SkillCastingAttack_01";
    //public const string E_SKILL_CASTING_ATTACK_BASIC_2 = "SkillCastingAttack_02";
    //public const string E_SKILL_CASTING_ATTACK_BASIC_3 = "SkillCastingAttack_03";
    //public const string E_SKILL_CASTING_ATTACK_BASIC_4 = "SkillCastingAttack_04";

    //public const string E_SKILL_ATTACK_BASIC_1 = "SkillAttack_01";
    //public const string E_SKILL_ATTACK_BASIC_2 = "SkillAttack_02";
    //public const string E_SKILL_ATTACK_BASIC_3 = "SkillAttack_03";
    //public const string E_SKILL_ATTACK_BASIC_4 = "SkillAttack_04";

    public enum SkillData
    {
        Damage,
        Buff,
        Debuff,
        Passive
    }

    public float durationTimerMax;
    public float castingTimerMax;

    public AnimationClip loading_Attack_Animation_Clip;
    public AnimationClip release_Attack_Animation_Clip;

    public bool specialAttackNeedsLoading;
    public bool hasEffectParticles;
    public bool hasEffectParticles_x2;

    [ShowIf("@hasEffectParticles == true")] public ParticleSystem animation_Particles_01;
    [ShowIf("@hasEffectParticles_x2 == true")] public ParticleSystem animation_Particles_02;
}
