using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellDirections
{
    N,
    E,
    S,
    W
}

public static class CellDirectionsExtensions
{
    public static Vector3 AsVector3(this CellDirections direction)
    {
        switch (direction)
        {
            case CellDirections.N:
                return Vector3.forward;

            case CellDirections.E:
                return Vector3.right;

            case CellDirections.S:
                return Vector3.back;

            case CellDirections.W:
                return Vector3.left;

            default:
                return Vector3.zero;
        }
    }
}
