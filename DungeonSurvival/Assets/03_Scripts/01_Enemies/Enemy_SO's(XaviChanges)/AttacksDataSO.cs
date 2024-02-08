using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackData", menuName = "Dungeon Survival/Animations/AttacksData")]
public class AttacksDataSO : ScriptableObject
{
    public enum SkillData
    {
        Damage,
        Buff,
        Debuff,
        Passive
    }
    private AttackCategory attackCategory;

    [ShowIf("@attackCategory != AttackCategory.basic")] public float durationTimerMax;
    [ShowIf("@attackCategory != AttackCategory.basic")] public float loadingTimerMax;
    [ShowIf("@attackCategory != AttackCategory.basic")] public float coldownAttackTimerMax;

    [ShowIf("@attackCategory != AttackCategory.basic")] public bool specialAttackNeedsLoading;

    public AnimationClip release_Attack_Animation_Clip;
    [ShowIf("@attackCategory != AttackCategory.basic")] public AnimationClip loading_Attack_Animation_Clip;

    public bool hasEffectParticles;
    public bool hasEffectParticles_x2;

    [ShowIf("@hasEffectParticles == true")] public ParticleSystem animation_Particles_01;
    [ShowIf("@hasEffectParticles_x2 == true")] public ParticleSystem animation_Particles_02;
    #region EditorButtons
    [PropertyOrder(-2)]
    [HorizontalGroup("Attack Category")]
    [Button("Basic Attack")]
    private void BasicAttack ( ) { attackCategory = AttackCategory.basic;  }

    [PropertyOrder (-2)]
    [HorizontalGroup("Attack Category")]
    [Button("Charged Attack")]
    private void ChargedAttack ( ) { attackCategory = AttackCategory.charged; }

    [PropertyOrder(-1)]
    [HorizontalGroup("Attack Category2")]
    [Button("Special Attack")]
    private void SpecialAttack ( ) { attackCategory = AttackCategory.special; }

    [PropertyOrder(-1)]
    [HorizontalGroup("Attack Category2")]
    [Button("Skill Attack")]
    private void SkillAttack ( ) { attackCategory = AttackCategory.skill; }
    #endregion
}
