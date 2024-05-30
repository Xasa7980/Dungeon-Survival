using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WallStickTrap : ActivableTrap
{
    //[SerializeField] float spacing = 0.5f;
    //[SerializeField] GameObject stickPrefab;
    [SerializeField] float actionArea;
    [SerializeField] Transform movingPart;
    Vector3 startPos;
    Vector3 endPos;
    [SerializeField] float speed = 1;

    [Space(10)]

    [SerializeField] float deactivateTime;
    [SerializeField] float returnSpeed = 0.3f;

    [SerializeField] int damage = 25;
    [SerializeField] GameObject flashHit;

    private void Start()
    {
        startPos = movingPart.position;
        endPos = startPos + movingPart.forward * actionArea;
    }

    //private void OnDrawGizmos()
    //{
    //Gizmos.matrix = transform.localToWorldMatrix;
    //for (int x = 0; x < this.size.x; x++)
    //{
    //    for (int y = 0; y < this.size.y; y++)
    //    {
    //        Vector3 pos = new Vector3(x * spacing, y * spacing, 0);
    //        Gizmos.DrawSphere(pos, 0.2f);
    //    }
    //}

    //Color color = Color.green;
    //color.a = 0.5f;
    //Gizmos.color = color;

    //Vector3 center = new Vector3((this.size.x - 1) / 2f * spacing, (this.size.y - 1) / 2f * spacing, actionArea / 2);
    //Vector3 size = new Vector3((this.size.x - 1) * spacing, (this.size.y - 1) * spacing, actionArea);

    //Gizmos.DrawCube(center, size);
    //}
    private void OnTriggerEnter ( Collider other )
    {
        if(other.TryGetComponent<iDamageable>(out iDamageable iDamageable))
        {
            iDamageable.ApplyDamage(damage);
        }
    }
    protected override IEnumerator _activate()
    {
        yield return base._activate();

        //Do something
        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * speed;

            movingPart.position = Vector3.Lerp(startPos, endPos, percent);

            yield return null;
        }

        yield return new WaitForSeconds(deactivateTime);

        percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * returnSpeed;

            movingPart.position = Vector3.Lerp(endPos, startPos, percent);

            yield return null;
        }

        active = false;
    }

//#if UNITY_EDITOR
//    [Button("Create", ButtonSizes.Medium)]
//    private void Create()
//    {
//        if (!stickPrefab) return;

//        if (movingPart.childCount != 0)
//        {
//            Transform[] childs = movingPart.GetComponentsInChildren<Transform>();
//            foreach (Transform child in childs)
//            {
//                if (child == movingPart) continue;

//                if (child == null) continue;

//                DestroyImmediate(child.gameObject);
//            }
//        }

//        for (int x = 0; x < size.x; x++)
//        {
//            for (int y = 0; y < size.y; y++)
//            {
//                GameObject stick = UnityEditor.PrefabUtility.InstantiatePrefab(stickPrefab) as GameObject;
//                stick.transform.parent = movingPart;
//                Vector3 pos = new Vector3(x * spacing, y * spacing, 0);
//                stick.transform.localPosition = pos;
//            }
//        }
//    }
//#endif

    private void OnValidate()
    {
        //BoxCollider collider = GetComponent<BoxCollider>();

        //Vector3 center = new Vector3((this.size.x - 1) / 2f * spacing, (this.size.y - 1) / 2f * spacing, collider.center.z);
        //Vector3 size = new Vector3((this.size.x - 1) * spacing, (this.size.y - 1) * spacing, collider.size.z);

        //collider.center = center;
        //collider.size = size;
    }
}
