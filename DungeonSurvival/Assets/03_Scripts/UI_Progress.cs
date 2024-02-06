using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Progress : MonoBehaviour
{
    public float smoothness = 1;
    [SerializeField] internal FillMethod fillMethod = FillMethod.Filled;


    
    internal IHasProgress hasProgress;
    internal enum FillMethod
    {
        Filled,
        Slider,
        None
    }
    public float currentHealthPercent;
    public float currentManaPercent;
    public float healthPercent;
    public float manaPercent;

    public GameObject progressGameObject;
    public Image overlayHPBarImage;
    public Image overlayMPBarImage;

    protected virtual void UpdateUI ( ) //QUITAR BARRA DEL WORLDSPACE
    {
    }

}
