using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class PotionModel
{
    public string name;
    public Sprite image;

  
    public PotionModel(string itemName, Sprite potionImage)
    {
        name = itemName;
        image = potionImage;
    }

}