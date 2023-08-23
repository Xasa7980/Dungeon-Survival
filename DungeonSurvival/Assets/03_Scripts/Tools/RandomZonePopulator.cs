using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RandomZonePopulator : MonoBehaviour
{
    enum Shape
    {
        Circle,
        Square
    }

    enum PlacingMethod
    {
        Noise,
        RandomPick
    }

    [SerializeField] int seed = 12345;
    [SerializeField] Shape shape;
    [SerializeField] PlacingMethod method;
    [SerializeField, ShowIf("method", PlacingMethod.RandomPick)] int pointAmount = 30;

    [SerializeField, Range(0, 1)] float overallProbability = 0.5f;
    [SerializeField] AnimationCurve probabilityFalloff;
    [SerializeField] bool invert = false;

    [SerializeField, ShowIf("shape", Shape.Circle)] float radius = 5;
    [SerializeField, ShowIf("shape", Shape.Square)] Vector2 size = new Vector2(5, 5);

    [SerializeField] GameObject[] objectsPool;

    public void Populate()
    {

    }

    #region Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        switch (shape)
        {
            case Shape.Circle:

                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radius);

                break;

            case Shape.Square:
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, 0, size.y));
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color color = Color.yellow;

        switch (shape)
        {
            case Shape.Circle:

                for (int i = 0; i <= 400; i++)
                {
                    float percent = i / 400f;
                    float value = probabilityFalloff.Evaluate(invert ? (1 - percent) : percent);
                    value = Mathf.Clamp01(value);
                    color.a = value * overallProbability;
                    UnityEditor.Handles.color = color;
                    UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, (1 - percent) * radius);
                    //UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, Vector3.forward, 360, radius);
                }

                break;

            case Shape.Square:
                Gizmos.matrix = transform.localToWorldMatrix;

                for (int i = 0; i <= 400; i++)
                {
                    float percent = i / 400f;
                    float value = probabilityFalloff.Evaluate(invert ? (1 - percent) : percent);
                    value = Mathf.Clamp01(value);
                    color.a = value * overallProbability;

                    float x = (1 - percent) * size.x;
                    float y = (1 - percent) * size.y;

                    Gizmos.color = color;
                    Gizmos.DrawWireCube(Vector3.zero, new Vector3(x, 0, y));
                }
                break;
        }
    }
#endif
    #endregion
}
