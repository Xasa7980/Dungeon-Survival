using Sirenix.OdinInspector;
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

    private void OnDrawGizmosSelected ( )
    {
        switch (drawMode)
        {
            case DrawMode.Box:
                DrawBox();
                break;
            case DrawMode.Sphere:
                DrawSphere();
                break;
            case DrawMode.Circle:
                // Implementa la lógica para Circle si es necesario
                break;
            default:
                break;
        }
    }

    void DrawBox ( )
    {
        Vector3 position = objectPositionMode == ObjectPositionMode.ObjectPosition ? transform.position : transform.TransformPoint(freePosition);
        Quaternion rotation = transform.rotation;
        Vector3 scale = transform.lossyScale;

        Matrix4x4 gizmoMatrix = Matrix4x4.TRS(position, rotation, scale);

        Gizmos.color = gizmoBoxColor;
        Gizmos.matrix = gizmoMatrix;
        Gizmos.DrawCube(Vector3.zero, size); // Usar size directamente, escalado por la matriz
    }

    void DrawSphere ( )
    {
        Vector3 position = objectPositionMode == ObjectPositionMode.ObjectPosition ? transform.position : transform.TransformPoint(freePosition);
        Quaternion rotation = transform.rotation;
        Vector3 scale = transform.lossyScale; // Considera cómo la escala afectará la esfera

        Matrix4x4 gizmoMatrix = Matrix4x4.TRS(position, rotation, scale);

        Gizmos.color = gizmoSphereColor;
        Gizmos.matrix = gizmoMatrix;
        Gizmos.DrawSphere(Vector3.zero, radius); // El radio puede necesitar ajuste basado en la escala
    }
}