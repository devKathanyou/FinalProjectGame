using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Image pickupPromptIcon; // Reference to the Image component

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Uncomment the following line if you want the UIManager to persist across scenes
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPickupPrompt()
    {
        if (pickupPromptIcon != null)
            pickupPromptIcon.enabled = true; // Show the icon
    }

    public void HidePickupPrompt()
    {
        if (pickupPromptIcon != null)
            pickupPromptIcon.enabled = false; // Hide the icon
    }
}
