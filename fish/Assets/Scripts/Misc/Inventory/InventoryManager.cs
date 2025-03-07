using UnityEngine;
using UnityEngine.UI;

//handles interaction between the player and the inventory system and its items
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
    
    [HideInInspector] private InventoryItem selectedItem;
    private InventoryItem SelectedItem
    {
        get => selectedItem;
        set
        {
            selectedItem = value;
            if(value != null)
                selectedItemRect = value.GetComponent<RectTransform>();
        }
    }
    RectTransform selectedItemRect;
    Vector2Int currentTileGridPosition;
    InventoryItem overlapItem;
    InventoryHighlight inventoryHighlight;

    Vector2Int previousHighlightPosition;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        //get tile position for current frame
        currentTileGridPosition = GetTileGridPosition();

        //tries to update the location of a selected item every frame to the mouse position
        DragItem();

        if (Input.GetKeyDown(KeyCode.R))
            RotateItem();

        // als muis buiten de grid is EN je hebt een item vast en ...
        if (selectedItemGrid == null && SelectedItem != null && Input.GetMouseButtonDown(0))
            DiscardItem();

        // als je geen item vast hebt
        if (Input.GetMouseButtonDown(1) && SelectedItem == null)
            EatItem();

        if (Input.GetMouseButtonDown(0) && selectedItemGrid != null)
            GrabOrPlaceItem();      

        HandleHighlight();
    }

    //tries to update the location of a selected item every frame to the mouse position
    private void DragItem()
    {
        if (SelectedItem != null)
            selectedItemRect.position = Input.mousePosition;
    }

    private void RotateItem()
    {
        if (SelectedItem == false)
            return;

        SelectedItem.Rotate();
    }

    public void DiscardItem()
    {
        if (SelectedItem != null)
        {
            Destroy(SelectedItem.gameObject);
            SelectedItem = null;
        }
    }

    public void EatItem()
    {
        //get current item
        InventoryItem itemAtPosition = selectedItemGrid.GetItem(currentTileGridPosition.x, currentTileGridPosition.y);

        //attempt to eat current item
        if (itemAtPosition != null)
        {
            GameManager.Instance.AddTime(itemAtPosition.itemData.healAmount);
            selectedItemGrid.RemoveItem(itemAtPosition);
            Destroy(itemAtPosition.gameObject);
        }
    }

    //checks if an item should be grabbed or place depending on the context
    private void GrabOrPlaceItem()
    {
        if (SelectedItem == null)
            GrabItem(currentTileGridPosition);
        else
            PlaceItem(currentTileGridPosition);
    }

    //attempts to update the current item
    private void GrabItem(Vector2Int tileGridPosition)
    {
        SelectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
    }

    public void PlaceItem(Vector2Int tileGridPosition)
    {
        //handles what happens inside the grid externally -> then store the result as bool & potential overlapItem to update in this part of the code.
        bool isPlacementSuccesfull = selectedItemGrid.PlaceItem(SelectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);

        if (isPlacementSuccesfull)
        {
            if (overlapItem != null)
            {
                SelectedItem = overlapItem;
                overlapItem = null;
                //adjust render order to make it first on the screen
                selectedItemRect.SetAsLastSibling();
            }
            else
            {
                SelectedItem = null;
            }
        }
    }

    //ask the grid what the current mouse position means
    private Vector2Int GetTileGridPosition()
    {
        if(selectedItemGrid)
        {
            Vector2 position = Input.mousePosition;

            //offset the grabbed item so that you hold the center of the item for placement
            if (SelectedItem != null)
            {
                position.x -= (SelectedItem.Width - 1) * ItemGrid.tileWidth / 2;
                position.y += (SelectedItem.Height - 1) * ItemGrid.tileHeight / 2;
            }

            return selectedItemGrid.GetTileGridPosition(position);
        }
        //when the grid isn't being targeted return an impossible value
        else
        {
            return new Vector2Int(-1,-1);
        }
    }

    //used by the fishing game to insert an item into the inventory
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

    private void HandleHighlight()
    {
        // toon highlighter enkel wnr je in grid hovert
        inventoryHighlight.Show(selectedItemGrid != null);

        //only apply changes when highlight has moved.
        if (previousHighlightPosition == currentTileGridPosition)
        {
            return;
        }
        else
        {
            previousHighlightPosition = currentTileGridPosition;
            //when holding no items -> attempt to highlight an inventory item.
            if (SelectedItem == null)
            {
                if(selectedItemGrid != null)
                {
                    InventoryItem itemToHighlight = selectedItemGrid.GetItem(currentTileGridPosition.x, currentTileGridPosition.y);

                    if (itemToHighlight != null)
                    {
                        inventoryHighlight.Show(true);
                        inventoryHighlight.SetSize(itemToHighlight);
                        inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
                    }
                    else
                        inventoryHighlight.Show(false);
                }
            }
            //when holding an item -> highlight that.
            else
            {
                inventoryHighlight.Show(selectedItemGrid.BoundaryCheck(currentTileGridPosition.x, currentTileGridPosition.y, SelectedItem.itemData.width, SelectedItem.itemData.height));
                inventoryHighlight.SetSize(SelectedItem);
                inventoryHighlight.SetPosition(selectedItemGrid, SelectedItem, currentTileGridPosition.x, currentTileGridPosition.y);
            }
        }
    }
}
