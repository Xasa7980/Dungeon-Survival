using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class PlayerProgress_UI : UI_Progress
{
    [SerializeField] Image characterDisplay;

    private void Awake ( )
    {
        hasProgress = progressGameObject.GetComponent<IHasProgress>();
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

    }

    private void HasProgress_OnProgressChanged ( object sender, IHasProgress.OnProgressChangedEventArgs e )
    {
        healthPercent = e.healthProgressNormalized;
        manaPercent = e.manaProgressNormalized;

        //Para Canvas Overlay
        currentHealthPercent = Mathf.MoveTowards(currentHealthPercent, healthPercent, smoothness * Time.deltaTime);
        currentManaPercent = Mathf.MoveTowards(currentManaPercent, manaPercent, smoothness * Time.deltaTime);
    }
    private void Update ( )
    {
        UpdateUI();
    }
    protected override void UpdateUI ( )
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
                break;
        }

    }

}
