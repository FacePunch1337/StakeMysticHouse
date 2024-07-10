using UnityEngine;

[System.Serializable]
public class ItemSlot : MonoBehaviour
{
    public bool HasItem => transform.childCount > 0;

    public Ingredient GetItem()
    {
        if (HasItem)
        {
            return transform.GetChild(0).GetComponent<Ingredient>();
        }
        return null;
    }
}
