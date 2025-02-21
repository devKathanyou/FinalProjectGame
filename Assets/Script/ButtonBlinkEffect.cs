using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonBlinkEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private Tween blinkTween;

    void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ทำให้ปุ่มกระพริบ (Fade In & Out)
        blinkTween = buttonImage.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // หยุดกระพริบและคืนค่าเดิม
        blinkTween.Kill();
        buttonImage.DOFade(1f, 0.2f);
    }
}
