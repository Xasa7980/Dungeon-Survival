using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public enum BillBoardType
{
    LookAt,
    LookAtInverted,
    Forward,
    ForwardInverted
}
public class CameraController : MonoBehaviour
{
    public static bool locked;
    public static CameraController current { get; private set; }
    public static Camera mainCamera { get; private set; }
    public static Camera uiCamera { get; private set; }

    [SerializeField] Transform target;

    Vector3 defaultCameraPosition;
    Quaternion defaultCameraRotation;

    [SerializeField] float followSpeed = 5;

    private void Awake()
    {
        current = this;
        mainCamera = Camera.main;
        uiCamera = GameObject.FindGameObjectWithTag("UI_Cam").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
    }
    public void GrabCamera ( Transform targetPoint, float speed = 1, bool lockPlayer = true )
    {
        locked = true;
        PlayerComponents.instance.lockedAll = true;

        defaultCameraPosition = mainCamera.transform.position;
        defaultCameraRotation = mainCamera.transform.rotation;

        StartCoroutine(MoveCamera(targetPoint.position, targetPoint.rotation, speed));
    }

    IEnumerator MoveCamera ( Vector3 targetPos, Quaternion targetRot, float speed )
    {
        Debug.Log("moviendo camara");
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        float percent = 0;

        while (true)
        {
            percent += Time.deltaTime * speed;

            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, percent);
            Quaternion nextRot = Quaternion.Lerp(startRot, targetRot, percent);

            mainCamera.transform.position = nextPos;
            mainCamera.transform.rotation = nextRot;

            if (percent >= 1)
                break;

            yield return null;
        }
    }

    public void ReleaseCamera ( Transform startPoint, float speed = 1, bool unlockPlayer = true )
    {
        StartCoroutine(ReleaseCameraRoutine(defaultCameraPosition, defaultCameraRotation, speed, unlockPlayer));
    }

    IEnumerator ReleaseCameraRoutine ( Vector3 startPos, Quaternion startRot, float speed, bool unlockPlayer )
    {
        float percent = 0;

        while (true)
        {
            percent += Time.deltaTime * speed;

            Vector3 nextPos = Vector3.Lerp(startPos, defaultCameraPosition, percent);
            Quaternion nextRot = Quaternion.Lerp(startRot, defaultCameraRotation, percent);

            mainCamera.transform.position = nextPos;
            mainCamera.transform.rotation = nextRot;

            if (percent >= 1)
                break;

            yield return null;
        }

        locked = false;
    }

    public IEnumerator Shake ( float duration, float intensity, float frequency )
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float z = Random.Range(-1f, 1f) * intensity;
            transform.localPosition = originalPosition + new Vector3(0, 0, z);

            yield return new WaitForSeconds(1f / frequency);
        }

        transform.localPosition = originalPosition;
    }
}
