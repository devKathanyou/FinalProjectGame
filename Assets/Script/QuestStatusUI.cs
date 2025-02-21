using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class QuestStatusUI : MonoBehaviour
{
    public TMP_Text questNameText;
    public TMP_Text questStatusText;
    public TMP_Text questProgressText;

    private Quest quest;

    // ตั้งค่าเควสต์และแสดงชื่อเควสต์
    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        questNameText.text = quest.questName;
    }

    // อัปเดตสถานะของเควสต์ (รองรับไอเทมหลายประเภท)
    public void UpdateStatus(Dictionary<Item, int> itemCounts, List<int> requiredCounts)
    {
        string statusText = "";
        for (int i = 0; i < quest.requiredItems.Count; i++)
        {
            Item item = quest.requiredItems[i];
            statusText += $"{item.itemName}: {itemCounts[item]} / {requiredCounts[i]}\n";
        }
        questProgressText.text = statusText;

        bool isCompleted = true;
        for (int i = 0; i < quest.requiredItems.Count; i++)
        {
            if (itemCounts[quest.requiredItems[i]] < requiredCounts[i])
            {
                isCompleted = false;
                break;
            }
        }

        if (isCompleted)
        {
            questStatusText.text = "เสร็จสิ้น";
            questStatusText.color = Color.green;
        }
        else
        {
            questStatusText.text = "ยังไม่เสร็จ";
            questStatusText.color = Color.yellow;
        }
    }

    // ดึงข้อมูลเควสต์ปัจจุบัน
    public Quest GetQuest()
    {
        return quest;
    }
}