using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableZone : MonoBehaviour, IPointerClickHandler
{
    private IInteractable interactable;

    private void Start()
    {
        interactable = GetComponent<IInteractable>();
        if (interactable == null)
        {
            Debug.LogError("������ �� ����� ����������, ������������ IInteractable.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        interactable?.Interact();
    }
}
