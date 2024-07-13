using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public Button tradeButton; // Assign the Trade button in the Unity Editor
    public ShopSlot[] itemSlots; // Assign the item slots in the Unity Editor
    public TMP_Text itemNameText; // Assign the text UI element for displaying item name
    private ShopSlot selectedSlot; // переменная для хранения выбранного слота
    private int unlockedItemCount = 0; // Количество разблокированных предметов
    private Lilith lilith;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tradeButton.interactable = false; // Ensure the button is initially disabled
        tradeButton.onClick.AddListener(OnTradeButtonClick);
        LoadUnlockedItems();

        lilith = GameManager.Instance.GetComponent<Lilith>();
    }

    public void UnlockItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (unlockedItemCount < itemSlots.Length)
            {
                itemSlots[unlockedItemCount].Unlock();
                unlockedItemCount++;
            }
        }
        SaveUnlockedItems();
    }

    public void CheckSelectedSlot(ShopSlot shopSlot, bool isSelected)
    {
        if (isSelected)
        {
            if (selectedSlot != null && selectedSlot != shopSlot)
            {
                selectedSlot.Deselect();
            }

            selectedSlot = shopSlot;
            selectedSlot.Select();
            itemNameText.text = selectedSlot.transform.GetChild(0).name;
        }
        else
        {
            shopSlot.Deselect();
            selectedSlot = null;
        }

        tradeButton.interactable = isSelected;
    }

    private void OnTradeButtonClick()
    {
        if (selectedSlot != null && GameManager.Instance.GetMoney() >= selectedSlot.price)
        {
            AudioManager.Instance.PlaySound(AudioManager.Sound.BuySound);
            GameManager.Instance.Pay(selectedSlot.price);
            Ingredient item = selectedSlot.GetIngredient();
            Inventory.Instance.AddIngredient(item);
            // Не сбрасываем выделение слота
        }
        else if (GameManager.Instance.GetMoney() < selectedSlot.price)
        {
            lilith.Dialog("Looks like you don't have enough money");
        }
    }

    public void DisplayItemName(string itemName)
    {
        itemNameText.text = itemName;
    }

    public void DeselectAll()
    {
        foreach (ShopSlot slot in itemSlots)
        {
            slot.Deselect();
        }
        tradeButton.interactable = false;
        itemNameText.text = "";
    }

    private void SaveUnlockedItems()
    {
        PlayerPrefs.SetInt("UnlockedItemCount", unlockedItemCount);
        PlayerPrefs.Save();
    }

    private void LoadUnlockedItems()
    {
        unlockedItemCount = PlayerPrefs.GetInt("UnlockedItemCount", 3); // Загружаем количество разблокированных предметов

        for (int i = 0; i < unlockedItemCount; i++)
        {
            if (i < itemSlots.Length)
            {
                itemSlots[i].Unlock();
            }
        }
    }
}
