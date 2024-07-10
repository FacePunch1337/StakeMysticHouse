using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{

    public bool HasItem => transform.childCount > 0;

    public Ingredient GetIngredient()
    {
        if (HasItem)
        {
            return transform.GetChild(0).GetComponent<Ingredient>();
        }
        return null;
    }

    public Mortar GetMortar()
    {
        if (HasItem)
        {
            return transform.GetChild(0).GetComponent<Mortar>();
        }
        return null;
    }

    public Potion GetPotion()
    {
        if (HasItem)
        {
            return transform.GetChild(0).GetComponent<Potion>();
        }
        return null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped != null)
            {
                DragableItem dragableItem = dropped.GetComponent<DragableItem>();
                if (dragableItem != null)
                {
                    dragableItem.parentAfterDrag = transform;
                }
                else
                {
                    Debug.Log("Dropped object does not have a DragableItem component, which is expected for some objects.");
                }
            }
            else
            {
                Debug.Log("PointerDrag object is null");
            }
        }
    }
}
