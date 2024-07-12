using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private List<Slot> IngredientsSlots;
    public event System.Action<Ingredient> OnIngredientAdd;
    [SerializeField] private List<Slot> MortarsSlots;
    public event System.Action<Mortar> OnMortarAdd;
    [SerializeField] private List<Slot> PotionsSlots;
    public event System.Action<Potion> OnPotionAdd;
    [SerializeField] private Slot activitySlot;

    [SerializeField] private GameObject ingredientsGrid;
    [SerializeField] private GameObject mortarsGrid;
    [SerializeField] private GameObject potionsGrid;

    [SerializeField] private Button ingredientsButton;
    [SerializeField] private Button mortarsButton;
    [SerializeField] private Button potionsButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ingredientsButton.onClick.AddListener(ShowIngredientsGrid);
        mortarsButton.onClick.AddListener(ShowMortarsGrid);
        potionsButton.onClick.AddListener(ShowPotionsGrid);
    }

    private void Start()
    {
        LoadInventory();
    }

    public void ShowGrid(string name)
    {
        switch (name)
        {
            case "ingredients":
                ShowIngredientsGrid();
                break;
            case "mortars":
                ShowMortarsGrid();
                break;
            case "potions":
                ShowPotionsGrid();
                break;
        }
    }

    public void ShowIngredientsGrid()
    {
        ingredientsGrid.SetActive(true);
        mortarsGrid.SetActive(false);
        potionsGrid.SetActive(false);
        ingredientsButton.Select();
        scrollRect.content = ingredientsGrid.GetComponent<RectTransform>();
    }

    public void ShowMortarsGrid()
    {
        ingredientsGrid.SetActive(false);
        mortarsGrid.SetActive(true);
        potionsGrid.SetActive(false);
        mortarsButton.Select();
        scrollRect.content = mortarsGrid.GetComponent<RectTransform>();
    }

    public void ShowPotionsGrid()
    {
        ingredientsGrid.SetActive(false);
        mortarsGrid.SetActive(false);
        potionsGrid.SetActive(true);
        potionsButton.Select();
        scrollRect.content = potionsGrid.GetComponent<RectTransform>();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        GameObject newItemObject = new GameObject(ingredient.itemName);

        DragableItem newItem = newItemObject.AddComponent<DragableItem>();
        Image newItemImage = newItemObject.AddComponent<Image>();
        newItemImage.sprite = ingredient.itemSprite.sprite;

        Ingredient newMortar = newItemObject.AddComponent<Ingredient>();
        newMortar.itemName = ingredient.itemName;

        foreach (Slot slot in IngredientsSlots)
        {
            if (slot.transform.childCount == 0)
            {
                newItem.transform.SetParent(slot.transform);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localScale = Vector3.one;
                Debug.Log($"Предмет добавлен в инвентарь: {ingredient.itemName}");
                SaveInventory();
                OnIngredientAdd?.Invoke(newMortar);
                return;
            }
        }
        Debug.Log("Инвентарь полон, не удалось добавить предмет");
    }

    public bool HasIngredient(Ingredient targetIngredient)
    {
        foreach (Slot slot in IngredientsSlots)
        {
            if (slot.transform.childCount != 0)
            {
                if(slot.transform.GetChild(0).TryGetComponent<Ingredient>(out Ingredient ingredient))
                {
                    if (targetIngredient.itemName == ingredient.itemName)
                    {
                        Debug.Log($"Found ingredient in inventory: {targetIngredient.itemName}");
                        return true;
                    }
                }
                
            }
        }

        Debug.Log($"CANT find ingredient in inventory: {targetIngredient.itemName}");
        return false;
    }

    public void AddMortar(CraftedItem<Mortar> craftedItem)
    {
        GameObject newItemObject = new GameObject(craftedItem.itemName);

        DragableItem newItem = newItemObject.AddComponent<DragableItem>();
        Image newItemImage = newItemObject.AddComponent<Image>();
        newItemImage.sprite = craftedItem.itemSprite;

        Mortar newMortar = newItemObject.AddComponent<Mortar>();
        newMortar.itemName = craftedItem.itemName;

        foreach (Slot slot in MortarsSlots)
        {
            if (slot.transform.childCount == 0)
            {
                newItem.transform.SetParent(slot.transform);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localScale = Vector3.one;
                Debug.Log($"Предмет добавлен в инвентарь: {craftedItem.itemName}");
                SaveInventory();
                OnMortarAdd?.Invoke(newMortar);
                return;
            }
        }
        Debug.Log("Инвентарь полон, не удалось добавить предмет");
    }
    
    public bool HasMortar(Mortar targetMortar)
    {
        foreach (Slot slot in MortarsSlots)
        {
                Debug.Log("has slot");
            if (slot.transform.childCount != 0)
            {
                Debug.Log("has item");
                if(slot.transform.GetChild(0).TryGetComponent<Mortar>(out Mortar mortar))
                {
                Debug.Log("has mortar");
                    if (targetMortar.itemName == mortar.itemName)
                    {
                        Debug.Log($"Found mortar in inventory: {targetMortar.itemName}");
                        return true;
                    }
                }
                
            }
        }

        Debug.Log($"CANT find mortar in inventory: {targetMortar.itemName}");
        return false;
    }

    public void AddPotion(CraftedItem<Potion> craftedItem)
    {
        GameObject newItemObject = new GameObject(craftedItem.itemName);

        DragableItem newItem = newItemObject.AddComponent<DragableItem>();
        Image newItemImage = newItemObject.AddComponent<Image>();
        newItemImage.sprite = craftedItem.itemSprite;

        Potion newPotion = newItemObject.AddComponent<Potion>();
        newPotion.itemName = craftedItem.itemName;

        if (activitySlot != null && activitySlot.transform.childCount == 0)
        {
            newItem.transform.SetParent(activitySlot.transform);
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localScale = Vector3.one;
            Debug.Log($"Предмет добавлен в активный слот: {craftedItem.itemName}");
            SaveInventory();
            OnPotionAdd?.Invoke(newPotion); // What is active Slot ? 
            return;
        }

        foreach (Slot slot in PotionsSlots)
        {
            if (slot.transform.childCount == 0)
            {
                newItem.transform.SetParent(slot.transform);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localScale = Vector3.one;
                Debug.Log($"Предмет добавлен в инвентарь: {craftedItem.itemName}");
                if(GameManager.Instance.GetCurrentLevel() == 1 && GameManager.Instance.GetCurrentExperience() < 100)
                {
                    TutorialManager.Instance.Next();
                }
                SaveInventory();
                OnPotionAdd?.Invoke(newPotion);
                return;
            }
        }

        Debug.Log("Инвентарь полон, не удалось добавить предмет");
    }
    
    public bool HasPotion(Potion targetPotion)
    {
        foreach (Slot slot in PotionsSlots)
        {
            if (slot.transform.childCount != 0)
            {
                if(slot.transform.GetChild(0).TryGetComponent<Potion>(out Potion potion))
                {
                    if (targetPotion.itemName == potion.itemName)
                    {
                        Debug.Log($"Found potion in inventory: {targetPotion.itemName}");
                        return true;
                    }
                }
                
            }
        }

        Debug.Log($"CANT find potion in inventory: {targetPotion.itemName}");
        return false;
    }

    public void SaveInventory()
    {
        for (int i = 0; i < IngredientsSlots.Count; i++)
        {
            if (IngredientsSlots[i].transform.childCount > 0)
            {
                Ingredient ingredient = IngredientsSlots[i].GetComponentInChildren<Ingredient>();
                PlayerPrefs.SetString("Ingredient_" + i, ingredient.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Ingredient_" + i, "");
            }
        }

        for (int i = 0; i < MortarsSlots.Count; i++)
        {
            if (MortarsSlots[i].transform.childCount > 0)
            {
                Mortar mortar = MortarsSlots[i].GetComponentInChildren<Mortar>();
                PlayerPrefs.SetString("Mortar_" + i, mortar.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Mortar_" + i, "");
            }
        }

        for (int i = 0; i < PotionsSlots.Count; i++)
        {
            if (PotionsSlots[i].transform.childCount > 0)
            {
                Potion potion = PotionsSlots[i].GetComponentInChildren<Potion>();
                PlayerPrefs.SetString("Potion_" + i, potion.itemName);
            }
            else
            {
                PlayerPrefs.SetString("Potion_" + i, "");
            }
        }

        PlayerPrefs.Save();
    }

    private void LoadInventory()
    {
        Debug.Log("Начало загрузки инвентаря");

        for (int i = 0; i < IngredientsSlots.Count; i++)
        {
            string itemName = PlayerPrefs.GetString("Ingredient_" + i, "");
            if (!string.IsNullOrEmpty(itemName))
            {
                GameObject prefab = Resources.Load<GameObject>($"Prefabs/Ingredients/{itemName}");
                if (prefab != null)
                {
                    GameObject newItem = Instantiate(prefab, IngredientsSlots[i].transform);
                    newItem.transform.localPosition = Vector3.zero;
                    newItem.transform.localScale = Vector3.one;
                    Debug.Log($"Загружен ингредиент: {itemName} в слот {i}");
                }
                else
                {
                    Debug.LogError($"Префаб {itemName} не найден в Resources/Prefabs/Ingredients!");
                }
            }
        }

        for (int i = 0; i < MortarsSlots.Count; i++)
        {
            string itemName = PlayerPrefs.GetString("Mortar_" + i, "");
            if (!string.IsNullOrEmpty(itemName))
            {
                GameObject prefab = Resources.Load<GameObject>($"Prefabs/Mortars/{itemName}");
                if (prefab != null)
                {
                    GameObject newItem = Instantiate(prefab, MortarsSlots[i].transform);
                    newItem.transform.localPosition = Vector3.zero;
                    newItem.transform.localScale = Vector3.one;
                    Debug.Log($"Загружен ступка: {itemName} в слот {i}");
                }
                else
                {
                    Debug.LogError($"Префаб {itemName} не найден в Resources/Prefabs!");
                }
            }
        }

        for (int i = 0; i < PotionsSlots.Count; i++)
        {
            string itemName = PlayerPrefs.GetString("Potion_" + i, "");
            if (!string.IsNullOrEmpty(itemName))
            {
                GameObject prefab = Resources.Load<GameObject>($"Prefabs/Potions/{itemName}");
                if (prefab != null)
                {
                    GameObject newItem = Instantiate(prefab, PotionsSlots[i].transform);
                    newItem.transform.localPosition = Vector3.zero;
                    newItem.transform.localScale = Vector3.one;
                    Debug.Log($"Загружено зелье: {itemName} в слот {i}");
                }
                else
                {
                    Debug.LogError($"Префаб {itemName} не найден в Resources/Prefabs!");
                }
            }
        }

        // Убедитесь, что UI обновляется после загрузки данных
        UIManager.Instance.UpdateUI();

        Debug.Log("Загрузка инвентаря завершена");
    }


}
