using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    // รายการเควสต์ที่กำลังทำอยู่พร้อมกับ NPC ที่ให้เควสต์
    private Dictionary<Quest, NPC> activeQuests = new Dictionary<Quest, NPC>();
    private List<Quest> completedQuests = new List<Quest>(); // รายการเควสต์ที่เสร็จสิ้น
    private Dictionary<Quest, Dictionary<Item, int>> questItemCounts = new Dictionary<Quest, Dictionary<Item, int>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // ทำให้ QuestManager ไม่ถูกทำลายเมื่อโหลด Scene ใหม่
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ตรวจสอบว่าเควสต์นี้ถูกทำเสร็จแล้วในทุกที่หรือไม่
    public bool IsQuestCompletedGlobally(Quest quest)
    {
        return completedQuests.Contains(quest);
    }

    // รับเควสต์จาก NPC
    public void AcceptQuest(Quest quest, NPC originNPC)
    {
        // ตรวจสอบว่าเควสต์นี้ถูกทำเสร็จแล้วหรือไม่
        if (completedQuests.Contains(quest))
        {
            Debug.Log("คุณได้ทำเควสนี้เสร็จสิ้นแล้ว: " + quest.questName);
            QuestUI.Instance.ShowQuest("เควสถูกทำเสร็จแล้ว", "คุณไม่สามารถรับเควสนี้ได้อีก!");
            return;
        }

        // ตรวจสอบว่าเควสต์ก่อนหน้าเสร็จสิ้นหรือไม่
        if (quest.requiredQuest != null && !completedQuests.Contains(quest.requiredQuest))
        {
            Debug.Log("คุณต้องทำเควสต์ก่อนหน้าให้เสร็จสิ้นก่อน: " + quest.requiredQuest.questName);
            QuestUI.Instance.ShowQuest("ยังไม่สามารถรับเควสต์ได้", "คุณต้องทำเควสต์ '" + quest.requiredQuest.questName + "' ให้เสร็จสิ้นก่อน!");
            return;
        }

        // ตรวจสอบว่าเควสต์นี้กำลังทำอยู่หรือไม่
        if (!activeQuests.ContainsKey(quest))
        {
            activeQuests.Add(quest, originNPC);
            questItemCounts[quest] = new Dictionary<Item, int>();

            // เริ่มต้นจำนวนไอเทมที่เก็บได้สำหรับแต่ละไอเทมที่ต้องการ
            for (int i = 0; i < quest.requiredItems.Count; i++)
            {
                questItemCounts[quest][quest.requiredItems[i]] = 0;
            }

            QuestUI.Instance.ShowQuest(quest);
            QuestUI.Instance.ShowQuestStatus(quest, questItemCounts[quest], quest.requiredItemCounts);
            Debug.Log("รับเควสต์: " + quest.questName);
        }
        else
        {
            Debug.Log("ไม่สามารถรับเควสต์ได้: " + quest.questName);
            QuestUI.Instance.ShowQuest("ไม่สามารถรับเควสต์ได้", "คุณไม่สามารถรับเควสนี้ได้!");
        }
    }

    // ตรวจสอบว่าเควสต์นี้กำลังทำอยู่หรือไม่
    public bool HasActiveQuest(Quest quest)
    {
        return activeQuests.ContainsKey(quest);
    }

    // เพิ่มไอเท็มที่เก็บได้สำหรับเควสต์
    public void AddQuestItem(Quest quest, Item item, int count)
    {
        if (activeQuests.ContainsKey(quest) && quest.requiredItems.Contains(item))
        {
            questItemCounts[quest][item] += count;
            QuestUI.Instance.UpdateQuestStatus(quest, questItemCounts[quest], quest.requiredItemCounts);

            // ตรวจสอบว่าเควสต์นี้เสร็จสิ้นแล้วหรือไม่
            if (IsQuestCompleted(quest))
            {
                Debug.Log("เควสต์เสร็จสิ้น: " + quest.questName);
            }
        }
    }

    // ทำเควสต์เสร็จสิ้น
    public void CompleteQuest(Quest quest, NPC originNPC)
    {
        if (!activeQuests.ContainsKey(quest))
        {
            Debug.LogWarning("เควสต์ยังไม่ถูกทำให้เป็นกิจกรรม: " + quest.questName);
            return;
        }

        // ตรวจสอบว่า NPC ที่ให้เควสต์ตรงกัน
        if (activeQuests[quest] != originNPC)
        {
            Debug.LogWarning("ไม่สามารถทำเควสต์นี้กับ NPC นี้ได้: " + originNPC.name);
            return;
        }

        if (IsQuestCompleted(quest))
        {
            // ลบไอเท็มที่ต้องการจาก Inventory
            InventorySystem inventory = FindObjectOfType<InventorySystem>();
            if (inventory != null)
            {
                // ลบไอเท็มที่ต้องการจาก Inventory
                for (int i = 0; i < quest.requiredItems.Count; i++)
                {
                    Item item = quest.requiredItems[i];
                    int itemsToRemove = quest.requiredItemCounts[i];

                    for (int j = 0; j < inventory.inventory.Length; j++)
                    {
                        if (inventory.inventory[j] == item)
                        {
                            inventory.inventory[j] = null;
                            itemsToRemove--;
                            if (itemsToRemove <= 0)
                                break;
                        }
                    }
                }
                inventory.UpdateUI();

                // ให้รางวัลไอเท็ม
                foreach (Item reward in quest.rewardItems)
                {
                    bool rewardAdded = inventory.AddItem(reward);
                    if (rewardAdded)
                    {
                        Debug.Log("ได้รับไอเท็ม: " + reward.itemName);
                    }
                    else
                    {
                        Debug.LogWarning("Inventory เต็ม! ไม่สามารถเพิ่มไอเท็ม: " + reward.itemName);
                    }
                }
            }
            else
            {
                Debug.LogError("ไม่พบ InventorySystem ใน Scene!");
            }

            // ย้ายเควสต์จาก activeQuests ไปยัง completedQuests
            activeQuests.Remove(quest);
            questItemCounts.Remove(quest);
            completedQuests.Add(quest); // เพิ่มเควสต์ไปยัง completedQuests

            // ลบสถานะเควสต์จาก UI
            QuestUI.Instance.RemoveQuestStatus(quest); // ลบสถานะเควสต์จาก UI

            // ถ้าไม่มีเควสต์ที่กำลังทำ ให้ซ่อนแผงสถานะเควสต์
            if (activeQuests.Count == 0)
            {
                QuestUI.Instance.HideQuestStatusPanel();
            }

            Debug.Log("ทำเควสต์เสร็จสิ้น: " + quest.questName);
        }
        else
        {
            Debug.Log("เควสต์ยังไม่เสร็จสิ้น: " + quest.questName);
            QuestUI.Instance.ShowQuest("เควสต์ยังไม่เสร็จสิ้น", "คุณยังไม่สามารถทำเควสต์นี้ได้!");
        }
    }

    // ดึงรายการเควสต์ที่กำลังทำอยู่
    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(activeQuests.Keys);
    }

    // ตรวจสอบว่าเควสต์นี้เสร็จสิ้นหรือไม่
    public bool IsQuestCompleted(Quest quest)
    {
        if (!questItemCounts.ContainsKey(quest))
            return false;

        for (int i = 0; i < quest.requiredItems.Count; i++)
        {
            Item item = quest.requiredItems[i];
            if (questItemCounts[quest][item] < quest.requiredItemCounts[i])
            {
                return false;
            }
        }
        return true;
    }

    // เก็บไอเท็มสำหรับเควสต์
    public void CollectItem(Item item)
    {
        foreach (var quest in activeQuests.Keys)
        {
            if (quest.requiredItems.Contains(item))
            {
                AddQuestItem(quest, item, 1);
            }
        }
    }

    // ดึงจำนวนไอเท็มที่เก็บได้สำหรับเควสต์
    public Dictionary<Item, int> GetQuestItemCounts(Quest quest)
    {
        if (questItemCounts.ContainsKey(quest))
        {
            return questItemCounts[quest];
        }
        return new Dictionary<Item, int>();
    }

    // ดึง NPC ที่ให้เควสต์
    public NPC GetOriginNPC(Quest quest)
    {
        if (activeQuests.ContainsKey(quest))
        {
            return activeQuests[quest];
        }
        return null;
    }
}