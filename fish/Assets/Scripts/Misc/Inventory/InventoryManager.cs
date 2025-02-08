using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid 
    { 
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;
    InventoryItem itemToHighlight;
    Vector2Int oldPosition;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        DragItem();


        // test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        // test
        if (Input.GetKeyDown(KeyCode.W))
        {
            InsertRandomItem();
        }

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            ItemInteraction();
        }
    }

    private void ItemInteraction()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
            GrabItem(tileGridPosition);
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.itemData.width - 1) * ItemGrid.tileWidth / 2;
            position.y += (selectedItem.itemData.height - 1) * ItemGrid.tileHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        if (complete)
        {
            selectedItem = null;

            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
    }

    private void GrabItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem != null)
            rectTransform = selectedItem.GetComponent<RectTransform>();
    }

    private void DragItem()
    {
        if (selectedItem != null)
            rectTransform.position = Input.mousePosition;
    }

    private void HandleHighlight()
    {
        Vector2Int positionGrid = GetTileGridPosition();

        if (oldPosition == positionGrid)
            return;

        oldPosition = positionGrid;

        if (selectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionGrid.x, positionGrid.y);

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
                inventoryHighlight.Show(false);
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(positionGrid.x, positionGrid.y, selectedItem.itemData.width, selectedItem.itemData.height));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionGrid.x, positionGrid.y);
        }
    }

    // test
    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int selectedItemID = Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);

    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForItem(itemToInsert);

        if (posOnGrid == null)
            return;

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    // test
    private void InsertRandomItem()
    {
        if (selectedItemGrid == null)
            return;
        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }
}
