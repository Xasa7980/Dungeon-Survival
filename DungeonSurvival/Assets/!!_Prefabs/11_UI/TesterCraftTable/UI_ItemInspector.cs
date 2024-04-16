using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemInspector : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Camera _camera;

    GameObject item;
    [SerializeField] Transform h_Pivot, v_Pivot;
    Item itemInspected;
    Transform objectInspected;
    Vector3 geometricalCenter;
    [SerializeField] Transform viewPoint;
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityY;

    bool pointerIn = false;
    [SerializeField] float minZoom, maxZoom;
    float currentZoom;
    float zoom;
    [SerializeField] float zoomSensitivity = 1;
    [SerializeField] float zoomSpeed = 1;

    public void Configure (Item item)
    {
        if(objectInspected != null)
            Destroy(objectInspected.gameObject);

        objectInspected = item.InstantiateInWorld(transform.position).transform;

        MeshFilter[] filters = objectInspected.GetComponentsInChildren<MeshFilter>();
        Vector3 center = GeometryTool.GetCenterOf(filters, objectInspected, GeometryTool.CenterType.Geometrical);
        h_Pivot.position = center;
        h_Pivot.rotation = Quaternion.identity;
        v_Pivot.rotation = Quaternion.identity;
        objectInspected.parent = v_Pivot;

        MeshRenderer[] renderers = objectInspected.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.receiveShadows = false;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        float radius = GeometryTool.GetMinRotationRadius(filters);
        h_Pivot.position = viewPoint.position + viewPoint.forward * (radius + 1);

        zoom = maxZoom;
        currentZoom = minZoom;
    }

    void Start()
    {
        currentZoom = _camera.fieldOfView;
        zoom = _camera.fieldOfView;
    }

    void Update()
    {
        currentZoom = Mathf.Lerp(currentZoom, zoom, zoomSpeed * Time.unscaledDeltaTime);
        _camera.fieldOfView = currentZoom;

        if (!pointerIn) return;

        zoom += -Input.GetAxisRaw("Mouse ScrollWheel") * zoomSensitivity;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
    }

    public void OnDrag(PointerEventData eventData)
    {
        h_Pivot.Rotate(viewPoint.up, -eventData.delta.x * sensitivityX * Time.unscaledDeltaTime, Space.World);
        v_Pivot.Rotate(viewPoint.right, eventData.delta.y * sensitivityY * Time.unscaledDeltaTime, Space.World);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerIn = false;
    }
}
