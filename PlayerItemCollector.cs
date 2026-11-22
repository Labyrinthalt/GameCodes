using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    void Start()
    {
        inventoryController = FindAnyObjectByType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    // Itala na napulot na ito para hindi na bumalik pagkabalik sa scene
                    WorldObjectId worldId = collision.GetComponent<WorldObjectId>();
                    if (worldId != null) worldId.MarkRemoved();

                    Destroy(collision.gameObject);
                }
            }
        }
    }
}