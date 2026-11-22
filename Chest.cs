using System;
using UnityEngine;

[Serializable]
public struct ChestLootEntry
{
    public GameObject itemPrefab;
    [Min(1)] public int amount;
}

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }

    [Header("Loot Settings")]
    public ChestLootEntry[] lootTable;
    public float dropSpreadX = 0.6f;
    public Vector3 dropOffset = Vector3.down;

    [Header("Visuals")]
    public Sprite openedSprite;

    void Start()
    {
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    private void OpenChest()
    {
        SetOpened(true);

        if (lootTable == null || lootTable.Length == 0) return;

        foreach (ChestLootEntry entry in lootTable)
        {
            if (entry.itemPrefab == null) continue;

            int countToDrop = Mathf.Max(1, entry.amount);

            for (int i = 0; i < countToDrop; i++)
            {
                float xOffset = UnityEngine.Random.Range(-dropSpreadX, dropSpreadX);
                Vector3 spawnPos = transform.position + dropOffset + new Vector3(xOffset, 0f, 0f);
                Instantiate(entry.itemPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    public void SetOpened(bool opened)
    {
        IsOpened = opened;
        if (IsOpened && openedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}