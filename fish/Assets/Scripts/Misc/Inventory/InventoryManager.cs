using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        if (Input.GetMouseButtonDown(1))
        {
            RotateItem();
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

    private void RotateItem()
    {
        if (selectedItem == false)
            return;

        selectedItem.Rotate();
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
            position.x -= (selectedItem.Width - 1) * ItemGrid.tileWidth / 2;
            position.y += (selectedItem.Height - 1) * ItemGrid.tileHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    public void PlaceItem(Vector2Int tileGridPosition)
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
                rectTransform.SetAsLastSibling();
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

    public void InsertItem(InventoryItem itemToInsert)
    {
        if (SelectedItemGrid == null)
        {
            Debug.LogError("No grid selected for item insertion!");
            return;
        }

        Vector2Int? posOnGrid = SelectedItemGrid.FindSpaceForItem(itemToInsert);

        if (posOnGrid == null)
        {
            Debug.LogWarning($"No space found for {itemToInsert.itemData.itemName} in inventory!");
            Destroy(itemToInsert.gameObject);
            return;
        }

        InventoryItem overlapItem = null;
        bool placed = SelectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y, ref overlapItem);
        
        if (placed)
        {
            Debug.Log($"Successfully placed {itemToInsert.itemData.itemName} at {posOnGrid.Value}");
        }
        else
        {
            Debug.LogWarning($"Failed to place {itemToInsert.itemData.itemName} in inventory!");
            Destroy(itemToInsert.gameObject);
        }
    }
}
