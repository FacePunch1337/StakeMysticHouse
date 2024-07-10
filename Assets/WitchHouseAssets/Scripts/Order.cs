using UnityEngine;

public class Order
{
    public int recipeIndex;
    public string orderName;
    public Sprite sprite;

    public Order(int recipeIndex, string orderName, Sprite sprite)
    {
        this.recipeIndex = recipeIndex;
        this.orderName = orderName;
        this.sprite = sprite;
    }
}
