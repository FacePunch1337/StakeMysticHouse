using UnityEngine;

[System.Serializable]
public class BrewSlot : MonoBehaviour
{
    public bool HasItem => transform.childCount > 0;

    public Mortar GetItem()
    {
        if (HasItem)
        {
            return transform.GetChild(0).GetComponent<Mortar>();
        }
        return null;
    }
}
