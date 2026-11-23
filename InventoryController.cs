using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    [Header("Ammo Settings (One Ammo Fits All)")]
    public int ammoItemID = 5; // Matches Ammo Variant.prefab ID = 5

    [Header("Stat Bars (kusang hahanapin kung walang laman)")]
    public StatBar hungerBar;
    public StatBar thirstBar;

    public static InventoryController Instance { get; private set; }

    private PlayerHealth playerHealth;
    private WeaponManager weaponManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        weaponManager = FindAnyObjectByType<WeaponManager>();

        if (hungerBar == null || thirstBar == null)
        {
            StatBar[] bars = FindObjectsByType<StatBar>();
            foreach (StatBar bar in bars)
            {
                string n = bar.gameObject.name.ToLower();
                if (hungerBar == null && n.Contains("hunger")) hungerBar = bar;
                if (thirstBar == null && (n.Contains("thirst") || n.Contains("water"))) thirstBar = bar;
            }
        }

        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;

                Item startingItem = item.GetComponent<Item>();
                if (startingItem != null && startingItem.isWeapon)
                {
                    UnlockWeaponFromItem(startingItem, false);
                }
            }
        }
    }

    public int GetTotalAmmo()
    {
        if (inventoryPanel == null) return 0;

        int total = 0;
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.ID == ammoItemID)
                {
                    total += slotItem.quantity;
                }
            }
        }
        return total;
    }

    public bool ConsumeAmmo(int amount = 1)
    {
        if (amount <= 0) return true;
        if (GetTotalAmmo() < amount) return false;

        int remainingToRemove = amount;

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.ID == ammoItemID && slotItem.quantity > 0)
                {
                    int take = Mathf.Min(slotItem.quantity, remainingToRemove);
                    slotItem.RemoveFromStack(take);
                    remainingToRemove -= take;

                    if (slotItem.quantity <= 0)
                    {
                        GameObject itemObj = slot.currentItem;
                        slot.currentItem = null;
                        Destroy(itemObj);
                    }

                    if (remainingToRemove <= 0)
                    {
                        return true;
                    }
                }
            }
        }

        return remainingToRemove <= 0;
    }

    public bool AddItem(GameObject itemPrefab)
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null) return false;

        // Only weapons go directly to the hotbar
        if (itemToAdd.isWeapon)
        {
            HotbarController hotbar = HotbarController.Instance != null
                ? HotbarController.Instance
                : FindAnyObjectByType<HotbarController>();

            if (hotbar != null)
            {
                bool addedToHotbar = hotbar.AddItemToHotbar(itemPrefab);
                if (addedToHotbar)
                {
                    UnlockWeaponFromItem(itemToAdd, true);
                    return true;
                }
                return false;
            }
        }

        // Non-weapons go to the regular inventory panel
        foreach (Transform slotTransform in inventoryPanel.transform)
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

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public void UseItem(GameObject itemObject, Slot slot = null)
    {
        if (itemObject == null) return;

        Item item = itemObject.GetComponent<Item>();
        if (item == null) return;

        if (item.isWeapon)
        {
            UnlockWeaponFromItem(item, true);
            return;
        }

        if (item.ID == ammoItemID)
        {
            return;
        }

        bool canHunger = item.hungerRestore > 0f && hungerBar != null
                         && hungerBar.currentValue < hungerBar.maxValue;

        bool canWater = item.waterRestore > 0f && thirstBar != null
                        && thirstBar.currentValue < thirstBar.maxValue;

        bool canHeal = item.healthRestore > 0f && playerHealth != null
                       && playerHealth.currentHealth < playerHealth.MaxHealth;

        if (!canHunger && !canWater && !canHeal)
        {
            Debug.Log("Hindi magagamit: wala talagang madadagdag.");
            return;
        }

        if (canHunger) hungerBar.ModifyValue(item.hungerRestore);
        if (canWater) thirstBar.ModifyValue(item.waterRestore);
        if (canHeal) playerHealth.Heal(item.healthRestore);

        item.RemoveFromStack(1);

        if (item.quantity <= 0)
        {
            if (slot == null) slot = itemObject.GetComponentInParent<Slot>();
            if (slot != null) slot.currentItem = null;
            Destroy(itemObject);
        }
    }

    private void UnlockWeaponFromItem(Item item, bool autoEquip)
    {
        if (weaponManager == null)
        {
            weaponManager = FindAnyObjectByType<WeaponManager>();
        }

        if (weaponManager != null)
        {
            string targetName = string.IsNullOrEmpty(item.weaponName) ? item.itemName : item.weaponName;
            weaponManager.UnlockWeapon(targetName, autoEquip);
        }
    }
}