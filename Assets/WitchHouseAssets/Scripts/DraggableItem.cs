using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    private Image image;
    private Camera mainCamera;

    public float smoothTime = 0.1f;  // Время сглаживания
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        mainCamera = Camera.main;
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        Vector3 touchPosition = eventData.position;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
       
        // Ограничиваем позицию объект видимыми границами камеры
        worldPosition = ClampToCameraBounds(worldPosition);
        transform.position = Vector3.SmoothDamp(transform.position, worldPosition, ref velocity, smoothTime);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    private Vector3 ClampToCameraBounds(Vector3 worldPosition)
    {
        Vector3 viewPortPosition = mainCamera.WorldToViewportPoint(worldPosition);

        viewPortPosition.x = Mathf.Clamp(viewPortPosition.x, 0.0f, 1.0f);
        viewPortPosition.y = Mathf.Clamp(viewPortPosition.y, 0.0f, 1.0f);

        Vector3 clampedWorldPosition = mainCamera.ViewportToWorldPoint(viewPortPosition);
        clampedWorldPosition.z = transform.position.z; // сохранить текущую z координату

        return clampedWorldPosition;
    }
}
