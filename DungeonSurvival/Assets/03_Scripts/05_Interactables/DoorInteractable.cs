using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] Transform door;
    [SerializeField] float openSpeed = 3;
    [SerializeField, Range (0,180)] float openAngle = 90;
    [SerializeField] bool invert;

    public override void FinishInteraction()
    {
        base.FinishInteraction();
        StartCoroutine(Open());
        _canInteract = false;
    }

    IEnumerator Open()
    {
        float angle = 0;

        while (angle < openAngle)
        {
            angle += Time.deltaTime * openSpeed;
            door.localRotation = Quaternion.Euler(Vector3.up * angle * ((invert) ? -1 : 1));

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireArc(door.position, Vector3.up, -door.right, openAngle * ((invert) ? -1 : 1), 2);
#endif
    }
}
