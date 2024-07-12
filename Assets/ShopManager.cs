using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public Button tradeButton; // Assign the Trade button in the Unity Editor
    public ShopSlot[] itemSlots; // Assign the item slots in the Unity Editor
    public TMP_Text itemNameText; // Assign the text UI element for displaying item name
    private ShopSlot selectedSlot; // переменна€ дл€ хранени€ выбранного слота
    private int unlockedItemCount = 0; //  оличество разблокированных предметов
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

    public void CheckSelectedSlot(ShopSlot shopSlot , bool isSelected)
    {
        selectedSlot = null;
        bool anySlotSelected = false;

        foreach (ShopSlot slot in itemSlots)
        {
            if (slot != shopSlot)
            {
                slot.Deselect();
                continue;
            }

            if (isSelected)
            {
                selectedSlot = slot; // сохран€ем выбранный слот
                anySlotSelected = true;
            }
            else
            {
                selectedSlot = null;
                anySlotSelected = false;
            }
            
        }

        tradeButton.interactable = anySlotSelected;
    }

    private void OnTradeButtonClick()
    {
        if (selectedSlot != null && GameManager.Instance.GetMoney() >= selectedSlot.price)
        {
            AudioManager.Instance.PlaySound(AudioManager.Sound.BuySound);
            GameManager.Instance.Pay(selectedSlot.price);
            Ingredient item = selectedSlot.GetIngredient();
            Inventory.Instance.AddIngredient(item);
            selectedSlot.Deselect(); // снимаем выбор с текущего слота
            selectedSlot = null; // сбрасываем выбранный слот
        }
        else if(GameManager.Instance.GetMoney() < selectedSlot.price)
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
        unlockedItemCount = PlayerPrefs.GetInt("UnlockedItemCount", 3); // «агружаем количество разблокированных предметов

        for (int i = 0; i < unlockedItemCount; i++)
        {
            if (i < itemSlots.Length)
            {
                itemSlots[i].Unlock();
            }
        }
    }
}
