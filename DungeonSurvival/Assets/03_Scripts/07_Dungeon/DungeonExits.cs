using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExitType
{
    None,
    Entrance,
    Exit
}
public class DungeonExits : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 parentRotation;

    [SerializeField] private float radius;

    [SerializeField] private ExitType exitType;

    private bool isOpen;

    public Vector3 actualPos;
    public Vector3 parentActualPos;
    public void SetExitType(ExitType _exitType )
    {
        exitType = _exitType;
    }
    public bool IsEntrance ( )
    {
        return exitType == ExitType.Entrance;
    }
    public bool IsExit ( )
    {
        return exitType == ExitType.Exit;
    }
    private void OnDrawGizmos ( )
    {
        if (!IsEntrance() && !IsExit())
        {
            Gizmos.color = Color.red;
        }
        else if (IsEntrance() && !IsExit())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.blue;
        }

        Gizmos.DrawRay(transform.position, -transform.forward * 7.5f);
        Gizmos.DrawSphere(transform.position + position, radius);
    }
}
