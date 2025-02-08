using System;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    const float tileWidth = 32;
    const float tileHeight = 32;

    [SerializeField] int gridWidth;
    [SerializeField] int gridHeight;

    RectTransform rectTransform;
    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    InventoryItem[,] inventoryItemSlot;

    // test
    [SerializeField] GameObject itemPrefab;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridWidth, gridHeight);

        // test
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 1, 1);

        inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 5, 2);

        inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 3, 10);
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

    public void PlaceItem(InventoryItem item, int posX, int posY)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        inventoryItemSlot[posX, posY] = item;

        Vector2 position = new Vector2();
        position.x = posX * tileWidth + tileWidth / 2;
        position.y = -(posY * tileHeight + tileHeight / 2);

        rectTransform.localPosition = position;
    }

    internal InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];
        inventoryItemSlot[x, y] = null;
        return toReturn;
    }
}
