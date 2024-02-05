using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Progress : MonoBehaviour
{
    public float smoothness = 1;
    public GameObject progressGameObject;
    public FillMethod fillMethod = FillMethod.Filled;
    [ShowIf("@fillMethod == FillMethod.Slider")] public Image overlayHPBarImage;
    [ShowIf("@fillMethod == FillMethod.Slider")] public Image overlayMPBarImage;

    
    public IHasProgress hasProgress;
    public enum FillMethod
    {
        Filled,
        Slider,
        None
    }
    public float currentHealthPercent;
    public float currentManaPercent;
    public float healthPercent;
    public float manaPercent;

    protected virtual void UpdateUI ( ) //QUITAR BARRA DEL WORLDSPACE
    {
    }

}
