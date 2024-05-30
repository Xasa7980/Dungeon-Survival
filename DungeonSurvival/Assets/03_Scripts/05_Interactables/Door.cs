using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Door : MonoBehaviour
{
    [SerializeField] Transform door;
    [SerializeField] float openSpeed = 3;
    [SerializeField, Range (0,180)] float openAngle = 90;
    [SerializeField] bool invertAngle;
    [SerializeField, HideInPlayMode] bool invertGizmo;

    private bool isOpened;
    //public override void FinishInteraction()
    //{
    //    base.FinishInteraction();
    //    StartCoroutine(Open());
    //    _canInteract = false;
    //}

    public void OpenOrClose ( )
    {
        if (isOpened)
        {
            isOpened = false;
            StartCoroutine(_Close());
        }
        else if (!isOpened)
        {
            isOpened = true;
            StartCoroutine(_Open());
        }
    }

    IEnumerator _Open()
    {
        float angle = 0;

        while (angle <= openAngle)
        {
            angle += Time.deltaTime * openSpeed;
            door.localRotation = Quaternion.Euler(Vector3.up * angle * ((invertAngle) ? -1 : 1));

            yield return null;
        }
    }
    IEnumerator _Close ( )
    {
        float angle = openAngle;

        while (angle >= 0)
        {
            angle -= Time.deltaTime * openSpeed;
            door.localRotation = Quaternion.Euler(Vector3.up * angle * ((invertAngle ? -1 : 1)));

            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireArc(door.position, Vector3.up, door.right * ((invertGizmo) ? -1 : 1), openAngle * ((invertAngle) ? -1 : 1), 2);
#endif
    }
}
