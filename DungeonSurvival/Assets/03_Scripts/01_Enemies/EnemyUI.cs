using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] Image mobDisplay;
    [SerializeField] float smoothness = 1;
    [SerializeField] private GameObject progressGameObject;


    [SerializeField] FillMethod fillMethod = FillMethod.Filled;
    [ShowIf("@fillMethod == FillMethod.Slider"), SerializeField] Image overlayHPBarImage;
    [ShowIf("@fillMethod == FillMethod.Slider"), SerializeField] Image overlayMPBarImage;
    [ShowIf("@fillMethod == FillMethod.Filled"), SerializeField] Image worldHPBarImage;
    [ShowIf("@fillMethod == FillMethod.Filled"), SerializeField] Image worldMPBarImage;
    
    private IHasProgress hasProgress;
    private enum FillMethod
    {
        Filled,
        Slider,
        None
    }
    private float currentHealthPercent;
    private float currentManaPercent;
    private float healthPercent;
    private float manaPercent;
    private void Awake ( )
    {
        hasProgress = progressGameObject.GetComponent<IHasProgress>();
        hasProgress.OnProgressChanged += EnemyUI_OnProgressChanged;

    }
    private void Start ( )
    {
    }

    private void EnemyUI_OnProgressChanged ( object sender, IHasProgress.OnProgressChangedEventArgs e )
    {
        healthPercent = e.healthProgressNormalized;
        manaPercent = e.manaProgressNormalized;
        currentHealthPercent = Mathf.MoveTowards(currentHealthPercent, healthPercent, smoothness * Time.deltaTime);
        currentManaPercent = Mathf.MoveTowards(currentManaPercent, manaPercent, smoothness * Time.deltaTime);
        //En el caso de que se decida crear barras de salud para los enemigos
    }

    private void Update ( )
    {
        UpdateUI();

    }
    protected void UpdateUI ( ) //QUITAR BARRA DEL WORLDSPACE
    {
        currentHealthPercent = Mathf.MoveTowards(currentHealthPercent, healthPercent, smoothness * Time.deltaTime);
        currentManaPercent = Mathf.MoveTowards(currentManaPercent, manaPercent, smoothness * Time.deltaTime);

        switch (fillMethod)
        {
            case FillMethod.Slider:
                overlayHPBarImage.rectTransform.localPosition = Vector3.Lerp(Vector3.left * -0.85f, Vector3.zero, currentHealthPercent);
                overlayMPBarImage.rectTransform.localPosition = Vector3.Lerp(Vector3.left * -0.85f, Vector3.zero, currentManaPercent);
                break;

            case FillMethod.Filled:
                overlayHPBarImage.fillAmount = currentHealthPercent;
                overlayMPBarImage.fillAmount = currentManaPercent;
                worldHPBarImage.fillAmount = currentHealthPercent;
                worldMPBarImage.fillAmount = currentManaPercent;
                break;
        }
    }

}
