using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        inventoryManager.selectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManager.selectedItemGrid = null;
    }
}
