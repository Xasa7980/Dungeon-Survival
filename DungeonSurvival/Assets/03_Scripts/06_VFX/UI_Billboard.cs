using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class UI_Billboard : MonoBehaviour
{
    Transform camTransform;

    void Start()
    {
        camTransform = CameraController.uiCamera.transform;
        if (TryGetComponent(out Canvas canvas))
        {
            canvas.worldCamera = CameraController.uiCamera;
        }
    }

    private void Update()
    {
        transform.rotation = camTransform.rotation * Quaternion.identity;
    }
}
