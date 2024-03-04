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
    [SerializeField] private float radius;

    [SerializeField] private ExitType exitType;

    private bool isOpen;

    public Vector3 actualPos;
    public Vector3 parentActualPos;
    private void OnValidate ( )
    {
        actualPos = transform.localPosition;
        parentActualPos = transform.parent.parent.position;
        Debug.Log(transform.parent.parent.name);
    }
    private void OnDrawGizmos ( )
    {
        if(!IsEntrance() && !IsExit())
        {
            Gizmos.color = Color.red;
        }
        else if(IsEntrance() && !IsExit())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.blue;
        }

        Gizmos.DrawSphere ( transform.position + position, radius );
    }
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
    private void UpdateExitStatus()
    {
        isOpen = (exitType == ExitType.Entrance);
    }
    private void StorePosition ( )
    {
        LoadSceneManager.instance.sceneHierarchyPosition = transform.position;
    }
}
