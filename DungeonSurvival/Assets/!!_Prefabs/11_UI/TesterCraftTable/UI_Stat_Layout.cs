using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat_Layout : MonoBehaviour
{
    [SerializeField] Text statName;
    public Text value;
    [SerializeField] Image icon;

    public static UI_Stat_Layout CreateInstance ( UI_Stat_Layout reference, Stat stat, Transform parent = null )
    {
        UI_Stat_Layout instance = Instantiate(reference, parent);

        instance.statName.text = stat.statClass.name;
        instance.value.text = stat.value.ToString();
        instance.icon.sprite = stat.statClass.icon;

        return instance;
    }
}