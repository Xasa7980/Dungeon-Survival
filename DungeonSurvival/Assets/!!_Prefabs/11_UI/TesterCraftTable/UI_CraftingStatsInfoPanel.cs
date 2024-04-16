using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_CraftingStatsInfoPanel : MonoBehaviour
{
    Dictionary<ItemStatClass, UI_Stat_Layout> uiStats = new Dictionary<ItemStatClass, UI_Stat_Layout>();
    Dictionary<ItemStatClass, int> statsValues = new Dictionary<ItemStatClass, int>();

    public Stat[] stats
    {
        get
        {
            List<Stat> list = new List<Stat>();
            foreach (ItemStatClass stat in statsValues.Keys)
            {
                list.Add(new Stat(stat, statsValues[stat]));
            }

            return list.ToArray();
        }
    }

    [SerializeField] UI_Stat_Layout statLayoutPrefab;

    public void AddStat ( Stat stat )
    {
        ItemStatClass existing = uiStats.Keys.ToList().FirstOrDefault(s => s == stat.statClass);
        if (existing != null)
        {
            statsValues[existing] += stat.value;
            uiStats[existing].value.text = statsValues[existing].ToString();
        }
        else
        {
            uiStats.Add(stat.statClass, UI_Stat_Layout.CreateInstance(statLayoutPrefab, stat, transform));
            statsValues.Add(stat.statClass, 0);
        }
    }

    public void RemoveStat ( Stat stat )
    {
        ItemStatClass existing = uiStats.Keys.ToList().First(s => s == stat.statClass);
        statsValues[existing] -= stat.value;

        uiStats[existing].value.text = statsValues[existing].ToString();

        if (statsValues[existing] <= 0)
        {
            Destroy(uiStats[existing].gameObject);
            uiStats.Remove(existing);
            statsValues.Remove(existing);
        }
    }
}