using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isConsumable;
    public float staminaBoost;  // ฟิลด์สำหรับเพิ่ม Stamina
    public int healthBoost;      // ฟิลด์ใหม่สำหรับเพิ่มสุขภาพ

    // ฟิลด์เพิ่มเติมสำหรับกุญแจ
    public bool isKey;
    public string keyID; // ระบุ ID ของกุญแจที่สามารถเปิดได้

}

