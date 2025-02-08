using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] public ItemGrid selectedItemGrid;
    InventoryItem selectedItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    private void Update()
    {
        DragItem();

        // test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (selectedItemGrid == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ItemInteraction();
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int selectedItemID = Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
            
    }

    private void ItemInteraction()
    {
        Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

        if (selectedItem == null)
        {
            selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

            if (selectedItem != null)
                rectTransform = selectedItem.GetComponent<RectTransform>();
        }
        else
        {
            selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
            selectedItem = null;
        }
    }

    private void DragItem()
    {
        if (selectedItem != null)
            rectTransform.position = Input.mousePosition;
    }
}
