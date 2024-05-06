using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerHolsterHandler : MonoBehaviour
{
    public static PlayerHolsterHandler current { get; private set; }

    public Transform[] weaponHolster;
    public Transform[] allActualHolsters => new Transform[]
    {
        primaryHolster,
        secondaryHolster,
        hipsRight,
        hipsLeft
    };
    [Title("Hands")]
    public Transform rightHand;
    public Transform leftHand;

    [Title("Holsters")]
    public Transform primaryHolster;
    public Transform secondaryHolster;

    public Transform helmetHolster;
    public Transform chestHolster;
    public Transform glovesHolsterRight;
    public Transform glovesHolsterLeft;
    public Transform leggingsHolster;
    public Transform bootsHolsterRight;
    public Transform bootsHolsterLeft;
    public Transform necklaceHolster;
    public Transform ringHolster;

    public Transform hipsRight;
    public Transform hipsLeft;

    [Title("Back")]
    public Transform backpack;

    private void Awake()
    {
        current = this;
        weaponHolster = new Transform[] { rightHand, leftHand };
    }
}
