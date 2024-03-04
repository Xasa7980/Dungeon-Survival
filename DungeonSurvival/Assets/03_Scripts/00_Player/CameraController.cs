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
