using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class Recipe
{
    public string name; // Имя рецепта
    public Ingredient ingredient1;
    public Ingredient ingredient2;
    public Ingredient ingredient3;
    public Sprite recipeImage; // Изображение рецепта

    public Recipe(string recipeName, Ingredient ing1, Ingredient ing2, Ingredient ing3, Sprite image)
    {
        name = recipeName;
        ingredient1 = ing1;
        ingredient2 = ing2;
        ingredient3 = ing3;
        recipeImage = image;
    }
}
