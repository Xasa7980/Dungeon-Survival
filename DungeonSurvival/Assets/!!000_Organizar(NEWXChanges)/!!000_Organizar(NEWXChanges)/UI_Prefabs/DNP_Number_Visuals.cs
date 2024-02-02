using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNP_Number_Visuals : MonoBehaviour
{
    DamageNumber damageNumber;

    private void Awake ( )
    {
        damageNumber = GetComponent<DamageNumber>();
    }
    private void OnEnable ( )
    {
        damageNumber.position = Vector3.zero + transform.up * 40;
        damageNumber.UpdateText();
    }
}
