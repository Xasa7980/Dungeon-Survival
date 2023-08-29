using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerHolsterHandler : MonoBehaviour
{
    public static PlayerHolsterHandler current { get; private set; }

    [Title("Hands")]
    public Transform rightHand;
    public Transform leftHand;

    [Title("Holsters")]
    public Transform primaryHolster;
    public Transform secondaryHolster;
    public Transform hipsRight;
    public Transform hipsLeft;

    [Title("Back")]
    public Transform backpack;

    private void Awake()
    {
        current = this;
    }
}
