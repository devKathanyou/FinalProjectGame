using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditBackground : MonoBehaviour
{
    public Image backgroundImage;
    public Sprite[] backgroundSprites;
    private int index = 0;

    void Start()
    {
        StartCoroutine(ChangeBackground());
    }

    IEnumerator ChangeBackground()
    {
        while (true)
        {
            backgroundImage.sprite = backgroundSprites[index];
            index = (index + 1) % backgroundSprites.Length;
            yield return new WaitForSeconds(5f);
        }
    }
}
