using UnityEngine;
using UnityEngine.UI;

public class Mortar : MonoBehaviour
{
    public string itemName;
    private Image itemSprite { get; set; }

    public Mortar(string name)
    {
        itemName = name;
    }
}
