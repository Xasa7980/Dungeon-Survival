using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemDropper : Interactable
{
    enum DropMode
    {
        All,
        Amount
    }

    [SerializeField] DropMode dropMode = DropMode.All;
    [SerializeField, ShowIf("dropMode", DropMode.Amount)] int amount = 3;

    [SerializeField] float dropRange;
    [SerializeField, Range(0, 1)] float dropMinRange = 0.2f;

    [SerializeField] iDrop[] drops;

    public override void FinishInteraction()
    {
        base.FinishInteraction();
        Drop();
    }

    void Drop()
    {
        List<GameObject> drop = new List<GameObject>();

        switch (dropMode)
        {
            case DropMode.All:
                drop = drops.Select(d => d.item).ToList();
                break;

            case DropMode.Amount:
                float total = 0;
                foreach (iDrop d in drops)
                {
                    total += d.chance;
                }

                for (int c = 0; c < amount; c++)
                {
                    float value = Random.Range(0, total);
                    float frac = 0;
                    for (int i = 0; i < drops.Length; i++)
                    {
                        frac += drops[i].chance;
                        if (value <= frac)
                        {
                            drop.Add(drops[i].item);
                            break;
                        }
                    }
                }
                break;
        }

        foreach(GameObject d in drop)
        {
            Vector2 point = Random.insideUnitCircle;
            point = point * (dropRange * dropMinRange) + point * (dropRange - (dropRange * dropMinRange));
            //point.x = (dropRange * dropMinRange) + point.x * (dropRange - (dropRange * dropMinRange));
            //point.y = (dropRange * dropMinRange) + point.y * (dropRange - (dropRange * dropMinRange));

            Vector3 position = transform.position + new Vector3(point.x, 0, point.y);
            Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));

            Instantiate(d, position, rotation);
        }
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, dropRange * dropMinRange);

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, dropRange);
#endif
    }

    [System.Serializable]
    class iDrop
    {
        string name => (_item == null) ? "Empty" : _item.name;

        [FoldoutGroup("@name"), SerializeField]
        GameObject _item;
        [FoldoutGroup("@name"), SerializeField, Range(0, 1)]
        float _chance = 0.5f;

        public GameObject item => _item;
        public float chance => _chance;
    }
}
