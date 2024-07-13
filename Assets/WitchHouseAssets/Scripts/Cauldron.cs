using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject cauldron;
    [SerializeField] private Slot[] itemSlots;
    [SerializeField] private List<PotionModel> potions;
    [SerializeField] private GameObject cauldronView;
    [SerializeField] private Image rotateImage;  // Поле для вращаемого изображения
    [SerializeField] private Slider progressBar; // Поле для прогресс-бара
    [SerializeField] private Button closeButton; // Кнопка закрытия панели
    [SerializeField] private Button brewButton;

    private float rotationProgress = 0f;
    private float progressPerRotation = 1f / 50f;
    private Lilith lilith;

    private void Start()
    {
        lilith = GameManager.Instance.GetComponent<Lilith>();
    }
    public void Interact()
    {
        cauldron.SetActive(true);
        Invoke("DisableOutline", 0.2f);
        Debug.Log("Котел: Начинаем варить зелье!");
        UIManager.Instance.ShowPanel(UIManager.PanelType.Cauldron);

        progressBar.gameObject.SetActive(false);  // Скрыть прогресс-бар по умолчанию
        cauldronView.gameObject.SetActive(false);
    }

    public void CheckSlot()
    {
        if (itemSlots[0].HasItem)
        {
            Mortar mortar = null;
            try
            {
                mortar = itemSlots[0].GetMortar().GetComponent<Mortar>();
            }
            catch
            {
                lilith.Dialog($"You can only cook blanks, crafted on a workbench.");
            }

            if (mortar != null)
            {
                Debug.Log($"Предметы в слотах: {mortar.itemName}");

                foreach (PotionModel potion in potions)
                {
                    Debug.Log($"ВАРИМ: {mortar.name}!");
                    if (MatchesRecipe(mortar.itemName, potion))
                    {
                        Debug.Log($"Сварили: {potion.name}!");
                        cauldronView.gameObject.SetActive(true);
                        progressBar.gameObject.SetActive(true);  // Показать прогресс-бар
                        rotationProgress = 0f;
                        progressBar.value = rotationProgress;

                        ToggleSlotsAndButton(false); // Отключить слоты и кнопку
                        StartCoroutine(RotateAndCraft(potion));
                        return;
                    }
                }
            }
          
            
               
            
        }
    }

    private IEnumerator RotateAndCraft(PotionModel potion)
    {
        RotateImageWithTouch rotateImageWithTouch = rotateImage.GetComponent<RotateImageWithTouch>();
        rotateImageWithTouch.OnRotate += UpdateProgress;

        while (rotationProgress < 1f)
        {
            yield return null;
        }

        rotateImageWithTouch.OnRotate -= UpdateProgress;
        CraftNewItem(potion);
        cauldronView.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        ClearItemSlots();

        ToggleSlotsAndButton(true); // Включить слоты и кнопку
    }

    private void UpdateProgress(float rotationAmount)
    {

        // Прогресс обновляется с учетом переданного угла вращения
        rotationProgress += (rotationAmount / 360f) * progressPerRotation;
        progressBar.value = rotationProgress;
    }

    void CraftNewItem(PotionModel potion)
    {
        // Создаем новый предмет

        AudioManager.Instance.PlaySound(AudioManager.Sound.BrewSound);
        AudioManager.Instance.PlaySound(AudioManager.Sound.CraftSound);
        CraftedItem<Potion> newItem = new CraftedItem<Potion>(potion.name, potion.image);

        // Добавляем новый предмет в инвентарь
        Inventory.Instance.AddPotion(newItem);
        Inventory.Instance.ShowGrid("potions");
        GameManager.Instance.AddExperience(20);


        Debug.Log("Создан новый предмет и добавлен в инвентарь: " + newItem.itemName);
    }

    bool MatchesRecipe(string name, PotionModel potion)
    {
        // Убираем "Mortar " из имени предмета в слоте
        string mortar = name.Replace("Mortar ", "");

        // Сравниваем очищенное имя с именем зелья из рецепта
        return (mortar == potion.name);
    }

    void ClearItemSlots()
    {
        foreach (Slot slot in itemSlots)
        {
            if (slot.HasItem)
            {
                Destroy(slot.GetMortar().gameObject);
            }
        }
    }

    void DisableOutline()
    {
        cauldron.SetActive(false);
    }

    private void ToggleSlotsAndButton(bool isActive)
    {
        foreach (Slot slot in itemSlots)
        {
            slot.gameObject.SetActive(isActive);
        }

        closeButton.interactable = isActive;
        brewButton.interactable = isActive;
        brewButton.gameObject.SetActive(isActive);
    }
}
