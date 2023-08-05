using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController current { get; private set; }
    public static Camera uiCamera { get; private set; }

    [SerializeField] Transform target;
    [SerializeField] float followSpeed = 5;

    private void Awake()
    {
        current = this;
        uiCamera = GameObject.FindGameObjectWithTag("UI_Cam").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
    }
}
