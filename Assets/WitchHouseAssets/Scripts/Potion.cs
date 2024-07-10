using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    public string itemName;
    private Image itemSprite { get; set; }

    public Potion(string name)
    {
        itemName = name;
    }

}