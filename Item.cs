using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    [Header("Item Info")]
    public int ID;
    public string itemName;
    public int quantity = 1;
    public int maxStack = 64;

    [Header("Consumable Stats")]
    public float hungerRestore = 25f;
    public float healthRestore = 10f;
    public float waterRestore = 0f;

    [Header("Weapon Settings")]
    public bool isWeapon = false;
    public string weaponName; // Must match exact name under GunHolder (e.g., Machete, Crowbar, M16)

    private TMP_Text tmpText;
    private Text legacyText;

    private void Awake()
    {
        tmpText = GetComponentInChildren<TMP_Text>(true);
        legacyText = GetComponentInChildren<Text>(true);
        RefreshQuantityUI();
    }

    public void RefreshQuantityUI()
    {
        string s = quantity.ToString();
        if (tmpText != null) tmpText.text = s;
        if (legacyText != null) legacyText.text = s;
    }

    public void AddToStack(int amount = 1)
    {
        quantity = Mathf.Min(quantity + amount, maxStack);
        RefreshQuantityUI();
    }

    public int RemoveFromStack(int amount = 1)
    {
        quantity -= amount;
        RefreshQuantityUI();
        return quantity;
    }

    public void UseItem()
    {
        if (InventoryController.Instance != null)
        {
            Slot slot = GetComponentInParent<Slot>();
            InventoryController.Instance.UseItem(gameObject, slot);
        }
    }

    public GameObject CloneItem(int amount = 1)
    {
        GameObject cloneObj = Instantiate(gameObject);
        Item clonedItem = cloneObj.GetComponent<Item>();
        clonedItem.ID = ID;
        clonedItem.itemName = itemName;
        clonedItem.quantity = amount;
        clonedItem.maxStack = maxStack;
        clonedItem.hungerRestore = hungerRestore;
        clonedItem.healthRestore = healthRestore;
        clonedItem.waterRestore = waterRestore;
        clonedItem.isWeapon = isWeapon;
        clonedItem.weaponName = weaponName;
        clonedItem.RefreshQuantityUI();
        return cloneObj;
    }
}