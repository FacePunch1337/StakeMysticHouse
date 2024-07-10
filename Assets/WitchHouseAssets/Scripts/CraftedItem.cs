using UnityEngine;


public class CraftedItem<T>
{
    public string itemName;
    public Sprite itemSprite;

    public CraftedItem(string name, Sprite sprite)
    {
        itemName = name;
        itemSprite = sprite;
    }
}
