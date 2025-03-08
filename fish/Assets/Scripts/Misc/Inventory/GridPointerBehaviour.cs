using UnityEngine;
using UnityEngine.EventSystems;

//assigns the inventory manager the correct grid when it is being hovered over
[RequireComponent(typeof(ItemGrid))]
public class GridPointerBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryManager inventoryManager;
    ItemGrid itemGrid;

    private void Awake()
    {
        inventoryManager = FindAnyObjectByType(typeof(InventoryManager)) as InventoryManager;
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManager.SelectedItemGrid = null;
    }
}
