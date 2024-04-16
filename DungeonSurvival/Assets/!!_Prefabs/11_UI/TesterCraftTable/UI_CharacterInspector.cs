using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CharacterInspector : MonoBehaviour, IDragHandler
{
    [SerializeField] float sensivityX = 10;
    Transform target;

    public void Init(Transform target)
    {
        this.target = target;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            target.Rotate(Vector3.up, -eventData.delta.x * sensivityX * Time.deltaTime);
        }
    }
}