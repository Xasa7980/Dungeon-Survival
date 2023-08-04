using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationContainer : SingletonPersistent<AnimationContainer>
{
    [SerializeField] AnimatorOverrideController aoc;
    [SerializeField] List<AnimationClip> animationClips = new List<AnimationClip>();
    List<string> animNames;
    public override void Awake()
    {
        base.Awake();
        InitAnimNames();
    }
    void InitAnimNames()
    {
        for (int i = 0; i < aoc.animationClips.Length; i++)
        {
            animNames[i] = aoc.animationClips[i].name;
            animationClips.Add(aoc.animationClips[i]);
        }
    }
    void SearchForAnim(string animName)
    {
        for (int i = 0; i < animNames.Count; i++)
        {
            if (animNames[i].Contains(animName))
            {
                continue;
            }
            else return;
        }
    }
    public void SetInteractuableAnimation(AnimationClip clip)
    {
        for (int i = 0; i < aoc.animationClips.Length; i++)
        {
            if (aoc.animationClips[i] != clip)
            {
                if (aoc.animationClips[i].name == "InteractableAnimation") aoc.animationClips[i] = clip;
            }
        }
    }
    public void SetPlayFloat(Animator anim, string animName, float f)
    {
        SearchForAnim(animName);
        anim.speed = 1;
        anim.SetFloat(animName, f);
    }
    public void SetPlayBool(Animator anim, string animName, bool b)
    {
        SearchForAnim(animName);
        anim.speed = 1;
        anim.SetBool(animName, b);
    }
    public void SetPlayTrigger(Animator anim, string animName)
    {
        SearchForAnim(animName);
        anim.speed = 1;
        anim.SetTrigger(animName);
    }
    public void SetRetardedPlayTrigger(Animator anim, string animName, float time)
    {
        SearchForAnim(animName);
        anim.speed = 1;
        float f = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (f > time)
        {
            anim.SetTrigger(animName);
        }
    }
}
