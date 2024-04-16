using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainPanel_Controller : MonoBehaviour
{
    public GameObject[] panels;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open(GameObject panel)
    {
        //LevelManager.ChangeTimeScale(0, 5);
        this.gameObject.SetActive(true);

        UI_Panel_Toggle[] toggles = GetComponentsInChildren<UI_Panel_Toggle>();

        foreach (UI_Panel_Toggle toggle in toggles)
        {
            if (toggle.panel == panel)
            {
                toggle.GetComponent<UnityEngine.UI.Toggle>().isOn = true;
                break;
            }
        }
    }

    public void Close()
    {
        //LevelManager.ChangeTimeScale(1, 10);
        gameObject.SetActive(false);
    }
}
