using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Prefabs/Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public float pullForce, pushForce;
    public float jumpCD;
    public float healAmount;
    public int width = 1, height = 1; // size in grid
    public Sprite image;
}
