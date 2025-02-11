using UnityEngine;
using UnityEngine.UI;

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

    public Slider timer;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        DragItem();

        if (Input.GetKeyDown(KeyCode.R))
            RotateItem();

        if (Input.GetMouseButtonDown(1) && selectedItem == null)
            EatItem();
        

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
            ItemInteraction();
        
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
            PlaceItem(tileGridPosition);
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
            return;
        
        Vector2Int? posOnGrid = SelectedItemGrid.FindSpaceForItem(itemToInsert);

        if (posOnGrid == null)
        {
            Debug.LogWarning($"No space found for {itemToInsert.itemData.itemName} in inventory!");
            // ipv Destroy, moet ik hier een ReplaceItem() method moeten roepen om een 2de grid te openen met de item
            Destroy(itemToInsert.gameObject);
            return;
        }

        InventoryItem overlapItem = null;
        bool placed = SelectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y, ref overlapItem);
    }

    public void DiscardItem()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        InventoryItem itemAtPosition = selectedItemGrid.GetItem(tileGridPosition.x, tileGridPosition.y);

        // check de tile waarover ik hover
        if (itemAtPosition != null)
        {
            selectedItemGrid.RemoveItem(itemAtPosition);
            Destroy(itemAtPosition.gameObject);
        }
    }

    public void EatItem()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();
        InventoryItem itemAtPosition = selectedItemGrid.GetItem(tileGridPosition.x, tileGridPosition.y);

        // check de tile waarover ik hover
        if (itemAtPosition != null)
        {
            timer.value += itemAtPosition.itemData.healAmount;
            selectedItemGrid.RemoveItem(itemAtPosition);
            Destroy(itemAtPosition.gameObject);
        }
    }
}
