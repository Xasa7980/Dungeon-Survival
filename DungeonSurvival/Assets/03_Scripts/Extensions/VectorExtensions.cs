using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.x, v.z);

    public static Vector3 ToVector3(this Vector2 v) => new Vector3(v.x, v.y, 0);

    public static Vector3 ToPlanarVector3(this Vector2 v) => new Vector3(v.x, 0, v.y);

    public static Vector3 ToPlanarVector3(this Vector3 v) => new Vector3(v.x, 0, v.y);
}
