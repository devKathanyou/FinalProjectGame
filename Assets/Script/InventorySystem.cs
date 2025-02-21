using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    public Item[] inventory = new Item[8];
    public Image[] inventorySlots;
    public float uiTransitionDuration = 0.3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Uncomment the following line if you want the InventorySystem to persist across scenes
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseItem(i);
            }
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                StartCoroutine(SmoothUpdateSlot(i, inventory[i].icon, true));
            }
            else
            {
                StartCoroutine(SmoothUpdateSlot(i, null, false));
            }
        }
    }

    IEnumerator SmoothUpdateSlot(int slotIndex, Sprite newSprite, bool showIcon)
    {
        Image slotImage = inventorySlots[slotIndex];
        float elapsedTime = 0f;

        Color initialColor = slotImage.color;
        Color targetColor = showIcon ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);

        while (elapsedTime < uiTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / uiTransitionDuration;
            slotImage.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }

        if (showIcon)
        {
            slotImage.sprite = newSprite;
            slotImage.enabled = true;
        }
        else
        {
            slotImage.sprite = null;
            slotImage.enabled = false;
        }
    }

    public bool RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == itemToRemove)
            {
                inventory[i] = null;
                UpdateUI();
                return true;
            }
        }
        Debug.Log("ไม่พบไอเท็มที่ต้องการลบในอินเวนทอรี!");
        return false;
    }

    public bool AddItem(Item newItem)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = newItem;
                UpdateUI();
                // แจ้ง QuestManager ว่าผู้เล่นเก็บไอเทม
                QuestManager.Instance.CollectItem(newItem);
                return true;
            }
        }
        Debug.Log("Inventory เต็ม!");
        return false;
    }

    public void UseItem(int slot)
    {
        if (slot < 0 || slot >= inventory.Length || inventory[slot] == null)
        {
            Debug.Log("Invalid item slot!");
            return;
        }

        Item item = inventory[slot];

        if (item.isConsumable)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            StaminaBar staminaBar = FindObjectOfType<StaminaBar>();

            bool canUseItem = false;

            // จัดการกับการเพิ่มสุขภาพ
            if (item.healthBoost > 0)
            {
                if (playerHealth.currentHealth < playerHealth.maxHealth)
                {
                    playerHealth.Heal(item.healthBoost);
                    canUseItem = true;
                }
                else
                {
                    Debug.Log("หัวใจเต็มแล้ว ไม่สามารถใช้ไอเทมนี้ได้");
                }
            }

            // จัดการกับการเพิ่ม Stamina
            if (item.staminaBoost > 0)
            {
                if (staminaBar.currentStamina < staminaBar.maxStamina)
                {
                    staminaBar.currentStamina += item.staminaBoost;
                    staminaBar.currentStamina = Mathf.Clamp(staminaBar.currentStamina, 0, staminaBar.maxStamina);
                    canUseItem = true;

                    // คงที่สตามินาเป็นเวลา 3 วินาที
                    staminaBar.FreezeStamina(5f);
                }
                else
                {
                    Debug.Log("Stamina เต็มแล้ว ไม่สามารถใช้ไอเทมนี้ได้");
                }
            }

            if (canUseItem)
            {
                StartCoroutine(SmoothRemoveItem(slot));
            }
            else
            {
                Debug.Log("ไม่สามารถใช้ไอเทมนี้ได้ในขณะนี้");
            }
        }
    }

    IEnumerator SmoothRemoveItem(int slot)
    {
        yield return SmoothUpdateSlot(slot, null, false);
        inventory[slot] = null;
        UpdateUI();
    }
}
