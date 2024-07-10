using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public Button tradeButton; // Assign the Trade button in the Unity Editor
    public ShopSlot[] itemSlots; // Assign the item slots in the Unity Editor
    public TMP_Text itemNameText; // Assign the text UI element for displaying item name
    private ShopSlot selectedSlot; // ���������� ��� �������� ���������� �����
    private int unlockedItemCount = 0; // ���������� ���������������� ���������

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tradeButton.interactable = false; // Ensure the button is initially disabled
        tradeButton.onClick.AddListener(OnTradeButtonClick);
        LoadUnlockedItems();
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

    public void CheckSelectedSlot()
    {
        selectedSlot = null;
        bool anySlotSelected = false;

        foreach (ShopSlot slot in itemSlots)
        {
            if (slot.IsSelected())
            {
                selectedSlot = slot; // ��������� ��������� ����
                anySlotSelected = true;
                break;
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
            selectedSlot.Deselect(); // ������� ����� � �������� �����
            selectedSlot = null; // ���������� ��������� ����
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
        unlockedItemCount = PlayerPrefs.GetInt("UnlockedItemCount", 3); // ��������� ���������� ���������������� ���������

        for (int i = 0; i < unlockedItemCount; i++)
        {
            if (i < itemSlots.Length)
            {
                itemSlots[i].Unlock();
            }
        }
    }
}
