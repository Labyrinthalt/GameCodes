using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    bool isDragging;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        isDragging = true;
        originalParent = transform.parent;
        transform.SetParent(transform.root);

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();

        // Hanapin ang slot na binitawan
        Slot dropSlot = null;
        if (eventData.pointerEnter != null)
        {
            dropSlot = eventData.pointerEnter.GetComponentInParent<Slot>();
        }

        // Walang slot, o sarili niyang slot: ibalik lang
        if (dropSlot == null || dropSlot == originalSlot)
        {
            transform.SetParent(originalParent, false);
            ResetRect(gameObject);
            return;
        }

        if (dropSlot.currentItem != null)
        {
            Item draggedItem = GetComponent<Item>();
            Item targetItem = dropSlot.currentItem.GetComponent<Item>();

            if (draggedItem != null && targetItem != null && draggedItem.ID == targetItem.ID)
            {
                // Pagsamahin ang stack
                targetItem.AddToStack(draggedItem.quantity);
                if (originalSlot != null) originalSlot.currentItem = null;
                Destroy(gameObject);
            }
            else
            {
                // Magpalit ng pwesto
                GameObject targetObj = dropSlot.currentItem;

                targetObj.transform.SetParent(originalParent, false);
                ResetRect(targetObj);
                if (originalSlot != null) originalSlot.currentItem = targetObj;

                transform.SetParent(dropSlot.transform, false);
                ResetRect(gameObject);
                dropSlot.currentItem = gameObject;
            }
        }
        else
        {
            // Walang laman ang slot: ilipat
            if (originalSlot != null) originalSlot.currentItem = null;

            transform.SetParent(dropSlot.transform, false);
            ResetRect(gameObject);
            dropSlot.currentItem = gameObject;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Right-click = gamitin ang item (kain, inom, bandage)
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Item item = GetComponent<Item>();
            if (item != null) item.UseItem();
        }
    }

    private void ResetRect(GameObject obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
        }
    }
}