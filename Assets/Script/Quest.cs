using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    [TextArea]
    public string description; // คำอธิบายเควส

    // ไอเทมที่ต้องการเพื่อเสร็จสิ้นเควส (สามารถมีหลายไอเทม)
    public List<Item> requiredItems;
    public List<int> requiredItemCounts; // จำนวนไอเทมที่ต้องการสำหรับแต่ละไอเทม

    // ไอเทมที่จะให้เป็นรางวัลเมื่อเสร็จสิ้น
    public List<Item> rewardItems;

    // เควสต์ที่ต้องทำก่อนจึงจะปลดล็อกเควสต์นี้
    public Quest requiredQuest;

    private void OnEnable()
    {
        // ตรวจสอบและเริ่มต้น requiredItemCounts หากยังไม่มีค่า
        if (requiredItems != null && requiredItemCounts == null)
        {
            requiredItemCounts = new List<int>();
            for (int i = 0; i < requiredItems.Count; i++)
            {
                requiredItemCounts.Add(1); // ตั้งค่าจำนวนเริ่มต้นเป็น 1 หรือตามที่ต้องการ
            }
        }
    }
}