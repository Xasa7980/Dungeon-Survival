using System.Collections;
using UnityEngine;

public class UI_Panel_Toggle : MonoBehaviour
{
    public UI_Panel panel;

    public void UpdatePanel(bool value)
    {
        if (panel != null)
        {
            if (value)
                panel.Open();
            else
                panel.Close();

            panel.gameObject.SetActive(value);
        }
    }
}