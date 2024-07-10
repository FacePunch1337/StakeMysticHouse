using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    private bool isSelected = false;
    private Button button;
    private bool isUnlocked = false;
    public int price = 0;
    [SerializeField] private TMP_Text priceText;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSelection);
    }

    private void Start()
    {
        gameObject.SetActive(isUnlocked); // Убедитесь, что слот изначально скрыт
        priceText.text = price.ToString();
    }

    public void Unlock()
    {
        isUnlocked = true;
        gameObject.SetActive(isUnlocked);
    }


    public void ToggleSelection()
    {
        isSelected = !isSelected;
        ShopManager.Instance.CheckSelectedSlot();
        if (isSelected)
        {
            string itemName = GetItemName();
            ShopManager.Instance.DisplayItemName(itemName);
        }
        else
        {
            ShopManager.Instance.DisplayItemName("");
        }
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void Deselect()
    {
        isSelected = false;
    }

    public string GetItemName()
    {
        // Assuming the item is a direct child of the slot and we want the name of that child GameObject
        Transform itemTransform = transform.GetChild(0); // Assuming the item is the first child
        return itemTransform != null ? itemTransform.gameObject.name : "";
    }

    public Ingredient GetIngredient()
    {
        Transform itemTransform = transform.GetChild(0); // Assuming the item is the first child
        Ingredient ingredient = itemTransform.GetComponent<Ingredient>();
        return ingredient;
    }
}
