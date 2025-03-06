using UnityEngine;
using UnityEngine.UI;

//handles interaction between the player and the inventory system and its items
public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;////
    [SerializeField] Transform canvasTransform;////

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
    RectTransform selectedItemRect;
    GameManager gameManager;
    InventoryItem overlapItem;////
    InventoryHighlight inventoryHighlight;////
    InventoryItem itemToHighlight;////
    Vector2Int oldPosition;////

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        //tries to update the location of a selected item every frame to the mouse position
        DragItem();

        if (Input.GetKeyDown(KeyCode.R))
            RotateItem();

        // als muis buiten de grid is EN je hebt een item vast en ...
        if (selectedItemGrid == null && selectedItem != null && Input.GetMouseButtonDown(0))
            DiscardItem();

        // als je geen item vast hebt
        if (Input.GetMouseButtonDown(1) && selectedItem == null)
            EatItem();

        // toon highlighter enkel wnr je in grid hovert
        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);////
            return;////
        }

        HandleHighlight();////

        if (Input.GetMouseButtonDown(0))
            GrabOrPlaceItem();      
    }

    //tries to update the location of a selected item every frame to the mouse position
    private void DragItem()
    {
        if (selectedItem != null)
            selectedItemRect.position = Input.mousePosition;
    }

    private void RotateItem()
    {
        if (selectedItem == false)
            return;

        selectedItem.Rotate();
    }

    public void DiscardItem()
    {
        if (selectedItem != null)
        {
            Destroy(selectedItem.gameObject);
            selectedItem = null;
        }
    }

    public void EatItem()
    {
        //get current item
        Vector2Int tileGridPosition = GetTileGridPosition();
        InventoryItem itemAtPosition = selectedItemGrid.GetItem(tileGridPosition.x, tileGridPosition.y);

        //attempt to eat current item
        if (itemAtPosition != null)
        {
            gameManager.timer.value += itemAtPosition.itemData.healAmount;
            selectedItemGrid.RemoveItem(itemAtPosition);
            Destroy(itemAtPosition.gameObject);
        }
    }

    //checks if an item should be grabbed or place depending on the context
    private void GrabOrPlaceItem()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
            GrabItem(tileGridPosition);
        else
            PlaceItem(tileGridPosition);
        
    }

    private void GrabItem(Vector2Int tileGridPosition)////
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem != null)
            selectedItemRect = selectedItem.GetComponent<RectTransform>();
    }

    public void PlaceItem(Vector2Int tileGridPosition)////
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        if (complete)
        {
            selectedItem = null;

            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                selectedItemRect = selectedItem.GetComponent<RectTransform>();
                selectedItemRect.SetAsLastSibling();
            }
        }
    }

    //ask the grid what the current mouse position means
    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        //offset the grabbed item so that you hold the center of the item for placement
        if (selectedItem != null)
        {
            position.x -= (selectedItem.Width - 1) * ItemGrid.tileWidth / 2;
            position.y += (selectedItem.Height - 1) * ItemGrid.tileHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    public void InsertItem(InventoryItem itemToInsert)////
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

    private void HandleHighlight()////
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
}
