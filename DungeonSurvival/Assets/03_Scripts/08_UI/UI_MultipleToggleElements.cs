using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MultipleToggleElements : MonoBehaviour
{
    public List<Image> imagesToControl;
    private Toggle toggle;

    private void Start ( )
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }
    private void OnToggleValueChanged ( bool isOn )
    {
        foreach (Image img in imagesToControl)
        {
            img.enabled = isOn;
        }
    }
}
