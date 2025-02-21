using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string npcName; // ชื่อของ NPC ที่พูด
    [TextArea(3, 10)]
    public string[] dialogueLines; // ข้อความที่ NPC และผู้เล่นจะพูดกัน

    private int currentDialogueIndex = 0; // ใช้ในการติดตามตำแหน่งของบทสนทนา

    public string GetNextDialogue()
    {
        if (currentDialogueIndex < dialogueLines.Length)
        {
            return dialogueLines[currentDialogueIndex++];
        }
        return null; // หมายถึงบทสนทนาหมดแล้ว
    }

    public void ResetDialogue()
    {
        currentDialogueIndex = 0; // รีเซ็ตบทสนทนาเพื่อเริ่มใหม่
    }
}
