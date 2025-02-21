using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadePanel;  // Drag FadePanel here in the Inspector
    public float fadeDuration = 1f;

    private void Start()
    {
        // àÃÔèÁµé¹·ÕèË¹éÒ¨ÍÊÇèÒ§
        fadePanel.alpha = 0f;
    }

    // ·ÓãËéË¹éÒ¨ÍÁ×´Å§
    public IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadePanel.alpha = 1f; // Ë¹éÒ¨ÍÁ×´Ê¹Ô·
    }

    // ·ÓãËéË¹éÒ¨ÍÊÇèÒ§¢Öé¹
    public IEnumerator FadeToClear()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadePanel.alpha = 0f; // Ë¹éÒ¨ÍÊÇèÒ§Ê¹Ô·
    }
}
