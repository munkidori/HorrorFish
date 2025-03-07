using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public float tileSize = 32;

    [SerializeField] int gridWidth = 10;
    [SerializeField] int gridHeight = 15;

    [SerializeField] float showPosition = 1000f;  // X position when shown      ////
    [SerializeField] float hidePosition = 60000f;  // X position when hidden    ////
    private bool isShowing = false;

    RectTransform gridRectTransform;
    InventoryItem[,] inventoryItemSlot;

    private void Start()
    {
        gridRectTransform = GetComponent<RectTransform>();
        Init(gridWidth, gridHeight);
    }

    private void Init(int width, int height)////
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSize, height * tileSize);
        gridRectTransform.sizeDelta = size;
    }

    //returns the targeted tile for a chosen mouse position
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        // om de coordinaten van mijn tiles in te stellen in mijn grid (tile 1 = 0,0 / tile 2 = 1,0 / ...)
        return new Vector2Int
        (
            (int)(mousePosition.x - gridRectTransform.position.x / tileSize),
            (int)(gridRectTransform.position.y - mousePosition.y / tileSize)
        );
    }

    // overload method met het te plaatse item reference (wanneer je een item wilt verplaatsen met een andere)
    public bool PlaceItem(InventoryItem item, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (posX < 0 || posY < 0 || posX + item.Width > gridWidth || posY + item.Height > gridHeight)
            return false;

        if (BoundaryCheck(posX, posY, item.Width, item.Height) == false)
            return false;

        if (OverlapCheck(posY, posX, item.Width, item.Height, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
            TakeItemOffGrid(overlapItem);

        PlaceItem(item, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem item, int posX, int posY)
    {
        RectTransform itemRectTransform = item.GetComponent<RectTransform>();
        itemRectTransform.SetParent(this.gridRectTransform);

        // zodat de item alle nodige tiles neemt om ermee te interacten, niet enkel de 1ste tile (links boven)
        for (int i = 0; i < item.Width; i++)
        {
            for (int j = 0; j < item.Height; j++)
            {
                inventoryItemSlot[posX + i, posY + j] = item;
            }
        }

        item.onGridPosition = new Vector2Int(posX, posY);

        Vector2 position = CalculateWorldPosition(item, posX, posY);

        itemRectTransform.localPosition = position;
    }

    //calculates the position for an item by converting grid coords into vector2
    public Vector2 CalculateWorldPosition(InventoryItem item, int posX, int posY)
    {
        Vector2 position;
        position.x = posX * tileSize + tileSize * item.Width / 2;
        position.y = -(posY * tileSize + tileSize * item.Height / 2);
        return position;
    }

    //returns an item for a position
    public InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    //returns an item for a position AND takes it of the grid
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
            return null;

        //remove item from the grid
        TakeItemOffGrid(toReturn);

        return toReturn;
    }

    //completely deletes an item
    public void DeleteItem(InventoryItem item)
    {
        for (int i = 0; i < item.Width; i++)
        {
            for (int j = 0; j < item.Height; j++)
            {
                Destroy(inventoryItemSlot[item.onGridPosition.x + i, item.onGridPosition.y + j]);
            }
        }
    }

    //make an item no longer part of the grid (item instance still exists)
    private void TakeItemOffGrid(InventoryItem item)
    {
        for (int i = 0; i < item.Width; i++)
        {
            for (int j = 0; j < item.Height; j++)
            {
                inventoryItemSlot[item.onGridPosition.x + i, item.onGridPosition.y + j] = null;
            }
        }
    }

    bool PositionExistCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
            return false;

        if (posX >= gridWidth || posY >= gridHeight)
            return false;

        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (PositionExistCheck(posX, posY) == false)
            return false;

        posX += width-1;
        posY += height-1;

        if (PositionExistCheck(posX, posY) == false)
            return false;

        return true;
    }

    private bool OverlapCheck(int posY, int posX, int width, int height, ref InventoryItem overlapItem)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (posX + i >= gridWidth || posY + j >= gridHeight)
                    return false;

                if (inventoryItemSlot[posX + i, posY + j] != null)
                {
                    if (overlapItem == null)
                        overlapItem = inventoryItemSlot[posX + i, posY + j];
                    else if (overlapItem != inventoryItemSlot[posX + i, posY + j])
                        return false;
                }
            }
        }

        return true;
    }

    public Vector2Int? FindSpaceForItem(InventoryItem itemToInsert)
    {
        int height = gridHeight - itemToInsert.Height + 1;
        int width = gridWidth - itemToInsert.Width + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x,y, itemToInsert.Width, itemToInsert.Height) == true)
                    return new Vector2Int(x, y);                
            }
        }

        return null;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (inventoryItemSlot[posX + i, posY + j] != null)
                    return false;
            }
        }

        return true;
    }

    public void ToggleInventory()////
    {
        isShowing = !isShowing;
        Vector2 anchoredPos = gridRectTransform.anchoredPosition;
        anchoredPos.x = isShowing ? showPosition : hidePosition;
        gridRectTransform.anchoredPosition = anchoredPos;
    }

    public void ClearGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (inventoryItemSlot[x, y] != null)
                {
                    Destroy(inventoryItemSlot[x, y].gameObject);
                    inventoryItemSlot[x, y] = null;
                }
            }
        }

        isShowing = false;
        Vector2 anchoredPos = gridRectTransform.anchoredPosition;
        anchoredPos.x = hidePosition;
        gridRectTransform.anchoredPosition = anchoredPos;
    }
}
