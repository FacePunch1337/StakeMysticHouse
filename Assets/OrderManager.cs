using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class OrderManager : MonoBehaviour
{
    public float orderInterval = 30f; // Интервал между заказами в секундах
    public Order currentOrder;
    private bool orderCompleted = true;

    // Индекс предыдущего заказа
    private int previousRecipeIndex = -1;

    // UI элементы для отображения текущего заказа
    public TMP_Text recipeNameText;
    public Image recipeImage;

    [SerializeField] private Slot itemSlot;

    private Lilith lilith;
    private TutorialManager tutorialManager;

    public event System.Action OnOrderComplete;
    private void Start()
    {
        lilith = GameManager.Instance.GetComponent<Lilith>();
        tutorialManager = GameManager.Instance.GetComponent<TutorialManager>();

        StartCoroutine(OrderCoroutine());

        if (GameManager.Instance.GetCurrentLevel() == 1 && GameManager.Instance.GetCurrentExperience() == 0)
        {
            tutorialManager.StartTutorial();
        }
    }

    public void CheckSlot()
    {
        if (itemSlot.HasItem)
        {
            Potion potion = itemSlot.GetPotion().GetComponent<Potion>();

            if (potion != null)
            {
                Debug.Log($"Предметы в слотах: Potion of {potion.name}");
                Debug.Log(currentOrder.orderName);
                // Проверка, соответствует ли зелье в слоте текущему заказу
                if (potion.itemName == currentOrder.orderName)
                {
                    Debug.Log("Заказ выполнен!");
                    CompleteOrder(potion);
                }
                else
                {
                    Debug.Log("Зелье не соответствует заказу.");
                }
            }
        }
    }

    private IEnumerator OrderCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(orderInterval);
            if (orderCompleted && !lilith.IsDialogActive)
            {
                GenerateOrder();
            }
        }
    }

    private void GenerateOrder()
    {
        if (orderCompleted)
        {
            List<int> availableRecipes = GameManager.Instance.GetLearnedRecipes();
            if (availableRecipes.Count > 0)
            {
                int randomIndex;
                int recipeIndex;

                // Генерация нового заказа, учитывая текущий опыт и уровень
                if (GameManager.Instance.GetCurrentExperience() == 0)
                {
                    recipeIndex = GameManager.Instance.GetLearnedRecipes().Count - 1; // Последний изученный рецепт
                }
                else
                {
                    do
                    {
                        randomIndex = Random.Range(0, availableRecipes.Count);
                        recipeIndex = availableRecipes[randomIndex];
                    } while (recipeIndex == previousRecipeIndex);
                }

                previousRecipeIndex = recipeIndex;

                string recipeName = GameManager.Instance.GetRecipeName(recipeIndex);
                Sprite recipeSprite = GameManager.Instance.GetRecipeSprite(recipeIndex);
                currentOrder = new Order(recipeIndex, recipeName, recipeSprite);
                orderCompleted = false; // Устанавливаем состояние заказа как невыполненный
                Debug.Log("New order received for recipe " + recipeIndex + ": " + recipeName);
                UpdateOrderUI(recipeName, recipeSprite);
                if(GameManager.Instance.GetCurrentLevel() > 1 && !TutorialManager.Instance.GetTutorialActive())
                {
                    // Вызов метода Dialog у Лилит
                    lilith.Dialog($"Can you make '{recipeName}'? Thanks!");
                }
                
            }
        }
    }

    private void UpdateOrderUI(string recipeName, Sprite recipeSprite)
    {
        recipeNameText.text = recipeName;
        recipeImage.sprite = recipeSprite;
    }

    private void CompleteOrder(Potion potion)
    {
        if (!orderCompleted)
        {
            orderCompleted = true;
            GetPaid(potion);
            Destroy(potion.gameObject);
            if (GameManager.Instance.GetCurrentLevel() >= 2 && GameManager.Instance.GetCurrentExperience() > 0)
            {
                UIManager.Instance.HideAllPanels();
                lilith.Dialog("Is it ready yet? Nice! Thanks for your help!");
            }
            GameManager.Instance.AddExperience(70);                 
            Inventory.Instance.SaveInventory();

            OnOrderComplete?.Invoke();
            
        }
    }

    private void GetPaid(Potion potion)
    {
        switch (potion.itemName)
        {
            case "Healing Elixir":
                GameManager.Instance.AddMoney(600);
                break;
            case "Mushroom Tincture":
                GameManager.Instance.AddMoney(800);
                break;
            case "Eldritch Potion":
                GameManager.Instance.AddMoney(1400);
                break;
            case "Twilight Witch's Potion":
                GameManager.Instance.AddMoney(1800);
                break;
            case "Grave Water":
                GameManager.Instance.AddMoney(2200);
                break;
            case "Witch's Curse":
                GameManager.Instance.AddMoney(2600);
                break;
            case "Glimmering Fairy Potion":
                GameManager.Instance.AddMoney(3000);
                break;
            case "Shining Starse":
                GameManager.Instance.AddMoney(3400);
                break;
            case "Blazing Moonlight":
                GameManager.Instance.AddMoney(3800);
                break;
            case "Sirens' Cry":
                GameManager.Instance.AddMoney(4200);
                break;
        }
    }

    /*private void SaveCurrentOrder()
    {
        if (currentOrder != null)
        {
            string orderData = JsonConvert.SerializeObject(currentOrder);
            PlayerPrefs.SetString("CurrentOrder", orderData);
            PlayerPrefs.Save();
        }
    }

    private void LoadCurrentOrder()
    {
        string orderData = PlayerPrefs.GetString("CurrentOrder", null);
        if (!string.IsNullOrEmpty(orderData))
        {
            currentOrder = JsonConvert.DeserializeObject<Order>(orderData);
            UpdateOrderUI(currentOrder.orderName, currentOrder.sprite);
        }
    }*/
}
