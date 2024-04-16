using System.Collections;
using UnityEngine;

public class SupportTools
{
    public static bool RectContains (Rect rect, Vector2 point, bool invertX = false, bool invertY = false)
    {
        float hDelta = (!invertX) ? point.x - rect.x : rect.x - point.x;
        float vDelta = (!invertY) ? point.y - rect.y : rect.y - point.y;

        Debug.Log(hDelta + " / " + rect.width);
        Debug.Log(vDelta + " / " + rect.height);

        bool hInside = hDelta >= 0 && hDelta < rect.width;
        bool vInside = vDelta >= 0 && vDelta < rect.height;

        return hInside && vInside;
    }
}