using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDrawer : MonoBehaviour
{
    [SerializeField] Vector3 _size = Vector3.one;
    public Vector3 size => _size;
    public Vector3 position => transform.position;
    public Quaternion rotation => transform.rotation;

    [SerializeField] Color gizmoColor = Color.green;

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(Vector3.zero, size);
    }
}
