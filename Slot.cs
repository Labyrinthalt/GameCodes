using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject currentItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check kung right click ang pinindot
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (currentItem != null)
            {
                // Tawagin ang UseItem method galing sa InventoryController
                if (InventoryController.Instance != null)
                {
                    InventoryController.Instance.UseItem(currentItem, this);
                }
            }
        }
    }
}   