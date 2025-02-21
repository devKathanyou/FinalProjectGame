using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TextBlinkEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI buttonText;
    private Tween blinkTween;

    void Start()
    {
        buttonText = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ทำให้ตัวอักษรกระพริบ (Fade In & Out)
        blinkTween = buttonText.DOFade(0.3f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // หยุดกระพริบและคืนค่าเดิม
        blinkTween.Kill();
        buttonText.DOFade(1f, 0.2f);
    }
}
