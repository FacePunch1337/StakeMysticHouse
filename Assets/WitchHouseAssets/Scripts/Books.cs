using UnityEngine;

public class Books : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject books;
    public void Interact()
    {
        books.SetActive(true);
        Invoke("DisableOutline", 0.2f);
        Debug.Log("Книжки: Изучаем новые знания!");
        UIManager.Instance.ShowPanel(UIManager.PanelType.Bookshelf);

    }

    void DisableOutline()
    {
        books.SetActive(false);
    }
}
