using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    public static HotbarController Instance { get; private set; }

    public GameObject hotbarPanel;
    public GameObject hotbarSlotPrefab;
    public int slotCount = 5;

    private ItemDictionary itemDictionary;
    private Key[] hotbarKeys;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        itemDictionary = FindAnyObjectByType<ItemDictionary>();

        hotbarKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    private void Start()
    {
        CreateHotbarSlots();
    }

    private void CreateHotbarSlots()
    {
        if (hotbarPanel == null || hotbarSlotPrefab == null) return;

        foreach (Transform child in hotbarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(hotbarSlotPrefab, hotbarPanel.transform, false);

            RectTransform rect = slotObj.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localScale = Vector3.one;
                rect.anchoredPosition = Vector2.zero;
            }
        }
    }

    public bool AddItemToHotbar(GameObject itemPrefab)
    {
        if (hotbarPanel == null || itemPrefab == null) return false;

        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null) return false;

        // First try stacking if the same weapon/item already exists in a hotbar slot
        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.ID == itemToAdd.ID && slotItem.quantity < slotItem.maxStack)
                {
                    slotItem.AddToStack(itemToAdd.quantity);
                    return true;
                }
            }
        }

        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform, false);
                RectTransform rect = newItem.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.localScale = Vector3.one;
                    rect.anchoredPosition = Vector2.zero;
                }
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.Log("Hotbar is full!");
        return false;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index)
    {
        if (index < 0 || index >= hotbarPanel.transform.childCount) return;

        Slot slot = hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if (slot != null && slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            if (item != null)
            {
                item.UseItem();
            }
        }
    }
}