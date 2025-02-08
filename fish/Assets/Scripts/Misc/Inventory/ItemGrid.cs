using System;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float tileWidth = 32;
    public const float tileHeight = 32;

    [SerializeField] int gridWidth = 10;
    [SerializeField] int gridHeight = 15;

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

    public bool PlaceItem(InventoryItem item, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (BoundaryCheck(posX, posY, item.itemData.width, item.itemData.height) == false)
            return false;

        if (OverlapCheck(posY, posX, item.itemData.width, item.itemData.height, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
            CleanGridReference(overlapItem);

        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        // zodat de item alle nodige tiles neemt om ermee te interacten, niet enkel de 1ste tile (links boven)
        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                inventoryItemSlot[posX + i, posY + j] = item;
            }
        }

        item.onGridPositionX = posX;
        item.onGridPositionY = posY;
        
        Vector2 position = new Vector2();
        position.x = posX * tileWidth + tileWidth * item.itemData.width/ 2;
        position.y = -(posY * tileHeight + tileHeight * item.itemData.height/ 2);

        rectTransform.localPosition = position;
        return true;
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
        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
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

    bool BoundaryCheck(int posX, int posY, int width, int height)
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
}
