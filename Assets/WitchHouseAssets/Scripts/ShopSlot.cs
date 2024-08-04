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
    [SerializeField] private Color selectedColor = Color.yellow; // Цвет для выделенного слота
    [SerializeField] private Color deselectedColor = Color.white; // Цвет для невыделенного слота

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSelection);
    }

    private void Start()
    {
        gameObject.SetActive(isUnlocked);
        priceText.text = price.ToString();
        UpdateSlotColor();
    }

    public void Unlock()
    {
        isUnlocked = true;
        gameObject.SetActive(isUnlocked);
    }

    public void ToggleSelection()
    {
        if (!isSelected)
        {
            ShopManager.Instance.CheckSelectedSlot(this, true);
        }
        else
        {
            ShopManager.Instance.CheckSelectedSlot(this, false);
        }
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateSlotColor();
    }

    public void Select()
    {
        isSelected = true;
        UpdateSlotColor();
    }

    private void UpdateSlotColor()
    {
        ColorBlock colors = button.colors;
        colors.normalColor = isSelected ? selectedColor : deselectedColor;
        button.colors = colors;
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
