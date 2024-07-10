using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Workbench : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject table;
    [SerializeField] private Slot[] itemSlots; // ������ ������ ��� ��������� �� ��������
    [SerializeField] private List<Recipe> recipes; // ������ ��������
    [SerializeField] private Image recipeImage; // ��� ����������� ����������� �������

  
    public void Interact()
    {
        table.SetActive(true);
        Invoke("DisableOutline", 0.2f);
        Debug.Log("����: �������������� �����������!");
        UIManager.Instance.ShowPanel(UIManager.PanelType.Workbench);
        recipeImage.gameObject.SetActive(false);
    }

    void DisableOutline()
    {
        table.SetActive(false);
    }

    void CheckCrafting()
    {
        // �������� ������� ��������� � ������
        if (itemSlots[0].HasItem && itemSlots[1].HasItem && itemSlots[2].HasItem)
        {
            Ingredient ing1 = itemSlots[0].GetIngredient().GetComponent<Ingredient>();
            Ingredient ing2 = itemSlots[1].GetIngredient().GetComponent<Ingredient>();
            Ingredient ing3 = itemSlots[2].GetIngredient().GetComponent<Ingredient>();

            if (ing1 != null && ing2 != null && ing3 != null)
            {
                Debug.Log($"�������� � ������: {ing1.itemName}, {ing2.itemName}, {ing3.itemName}");
                
                // ��������� ������ ������
                foreach (Recipe recipe in recipes)
                {
                    if (MatchesRecipe(recipe, ing1, ing2, ing3))
                    {
                        Debug.Log($"�������� ����� �������: {recipe.name}!");
                        recipeImage.gameObject.SetActive(true);
                        if (recipe.recipeImage != null)
                        {
                            recipeImage.sprite = recipe.recipeImage;
                        }
                        CraftNewItem(recipe);
                        ClearItemSlots();
                        return;
                    }
                }

                Debug.Log("�������� ���������� ������������");
            }
            else
            {
                Debug.Log("���� ��� ��������� ��������� �� ������� � ������");
            }
        }
        else
        {
            Debug.Log("������������ ������������");
        }
    }

    bool MatchesRecipe(Recipe recipe, Ingredient ing1, Ingredient ing2, Ingredient ing3)
    {
        // ���������, ��������� �� ���������� ����� ������������ � ��������
        return (ing1.itemName == recipe.ingredient1.itemName &&
                ing2.itemName == recipe.ingredient2.itemName &&
                ing3.itemName == recipe.ingredient3.itemName);
    }

    void CraftNewItem(Recipe recipe)
    {
        // ������� ����� �������
        AudioManager.Instance.PlaySound(AudioManager.Sound.CraftSound);
        CraftedItem<Mortar> newItem = new CraftedItem<Mortar>(recipe.name, recipe.recipeImage);

        // ��������� ����� ������� � ���������
        Inventory.Instance.AddMortar(newItem);
        Inventory.Instance.ShowGrid("mortars");
        GameManager.Instance.AddExperience(10);
        Debug.Log("������ ����� ������� � �������� � ���������: " + newItem.itemName);
    }
    void ClearItemSlots()
    {
        foreach (Slot slot in itemSlots)
        {
            if (slot.HasItem)
            {
                Destroy(slot.GetIngredient().gameObject);
            }
        }
    }
}
