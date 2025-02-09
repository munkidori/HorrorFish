using System;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileWidth = 32;
    public const float tileHeight = 32;

    [SerializeField] int gridWidth = 10;
    [SerializeField] int gridHeight = 15;

    [SerializeField] float showPosition = 1000f;  // X position when shown
    [SerializeField] float hidePosition = 1600f;  // X position when hidden
    private bool isShowing = false;

    RectTransform rectTransform;
    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    InventoryItem[,] inventoryItemSlot;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridWidth, gridHeight);
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileWidth, height * tileHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        // om de coordinaten van mijn tiles in te stellen in mijn grid (tile 1 = 0,0 / tile 2 = 1,0 / ...)
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileHeight);

        return tileGridPosition;
    }

    // overload method met het te plaatse item reference
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
            CleanGridReference(overlapItem);

        PlaceItem(item, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem item, int posX, int posY)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        // zodat de item alle nodige tiles neemt om ermee te interacten, niet enkel de 1ste tile (links boven)
        for (int i = 0; i < item.Width; i++)
        {
            for (int j = 0; j < item.Height; j++)
            {
                inventoryItemSlot[posX + i, posY + j] = item;
            }
        }

        item.onGridPositionX = posX;
        item.onGridPositionY = posY;

        Vector2 position = new Vector2();
        position = CalculatePosition(item, posX, posY);

        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePosition(InventoryItem item, int posX, int posY)
    {
        Vector2 position;
        position.x = posX * tileWidth + tileWidth * item.Width / 2;
        position.y = -(posY * tileHeight + tileHeight * item.Height / 2);
        return position;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
            return null;

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int i = 0; i < item.Width; i++)
        {
            for (int j = 0; j < item.Height; j++)
            {
                inventoryItemSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
            }
        }
    }

    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
            return false;

        if (posX >= gridWidth || posY >= gridHeight)
            return false;

        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (PositionCheck(posX, posY) == false)
            return false;

        posX += width-1;
        posY += height-1;

        if (PositionCheck(posX, posY) == false)
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

    public InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
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

    public bool IsSlotTaken(int x, int y)
    {
        if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight)
            return true;
        return inventoryItemSlot[x, y] != null;
    }

    public void ToggleInventory()
    {
        isShowing = !isShowing;
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        anchoredPos.x = isShowing ? showPosition : hidePosition;
        rectTransform.anchoredPosition = anchoredPos;
    }

    public void ClearGrid()
    {
        // Clear all slots
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

        // Reset position to hidden
        isShowing = false;
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        anchoredPos.x = hidePosition;
        rectTransform.anchoredPosition = anchoredPos;
    }
}
