using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDrawer : MonoBehaviour
{
    public enum DrawMode
    {
        Box,
        Sphere,
        Circle
    }
    private enum ObjectPositionMode
    {
        ObjectPosition,
        FreePosition
    }
    public DrawMode drawMode => _drawMode;
    [SerializeField] DrawMode _drawMode = DrawMode.Box;

    [SerializeField] ObjectPositionMode objectPositionMode = ObjectPositionMode.ObjectPosition;

    [SerializeField, ShowIf("@_drawMode == DrawMode.Box")] Color gizmoBoxColor = Color.green;
    [SerializeField, ShowIf("@_drawMode == DrawMode.Box")] Vector3 _size = Vector3.one;

    [SerializeField, ShowIf("@_drawMode == DrawMode.Sphere")] Color gizmoSphereColor = Color.red;
    [SerializeField, ShowIf("@_drawMode == DrawMode.Sphere")] float _radius = 1;

    public float radius => _radius;
    public Vector3 size => _size;
    public Vector3 objectPosition => transform.position;
    public Vector3 freePosition;
    public Quaternion rotation => transform.rotation;


    private void OnDrawGizmosSelected()
    {
        switch (drawMode)
        {
            case DrawMode.Box:
                switch (objectPositionMode)
                {
                    case ObjectPositionMode.ObjectPosition:

                        Gizmos.matrix = transform.localToWorldMatrix;
                        Gizmos.color = gizmoBoxColor;
                        Gizmos.DrawCube(Vector3.zero, size);
                        break;

                    case ObjectPositionMode.FreePosition:

                        Vector3 localPositionFromWorld = transform.TransformPoint(freePosition);
                        Quaternion localRotation = transform.rotation;

                        Gizmos.color = gizmoBoxColor;
                        Gizmos.matrix = Matrix4x4.TRS(localPositionFromWorld, localRotation, Vector3.one);
                        Gizmos.DrawCube(Vector3.zero, size);
                        break;

                    default:
                        break;
                }
                break;

            case DrawMode.Sphere:

                switch (objectPositionMode)
                {
                    case ObjectPositionMode.ObjectPosition:

                        Gizmos.color = gizmoSphereColor;
                        Gizmos.DrawSphere(Vector3.zero, radius);
                        break;
                    case ObjectPositionMode.FreePosition:

                        Vector3 localPositionFromWorld = transform.TransformPoint(freePosition);
                        Quaternion localRotation = transform.rotation;

                        Gizmos.matrix = Matrix4x4.TRS(localPositionFromWorld, localRotation, Vector3.one);
                        Gizmos.color = gizmoSphereColor;
                        Vector3 realPosition = localPositionFromWorld;
                        Gizmos.DrawSphere(Vector3.zero, radius);
                        break;
                    default:
                        break;
                }
                break;
            case DrawMode.Circle:
                break;
            default:
                break;
        }
    }
}
