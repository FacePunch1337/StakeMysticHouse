using UnityEngine;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour
{
    public string itemName;
    public Image itemSprite;

    public Ingredient(string name)
    {
        itemName = name;
    }
}
