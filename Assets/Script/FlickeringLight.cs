using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLight : MonoBehaviour
{
    public Light2D pointLight;  // อ้างอิงไปยัง Point Light 2D
    public float minIntensity = 0.2f;  // ค่าความสว่างต่ำสุด
    public float maxIntensity = 1.0f;  // ค่าความสว่างสูงสุด
    public float flickerSpeed = 0.1f;  // ความเร็วในการกระพริบ (วินาที)

    public AudioSource flickerSound;  // เพิ่มตัวแปรสำหรับเสียง

    void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light2D>();
        }
        if (flickerSound == null)
        {
            flickerSound = GetComponent<AudioSource>();
        }
        InvokeRepeating("Flicker", 0f, flickerSpeed);
    }

    IEnumerator TurnOffLight()
    {
        pointLight.intensity = 0;  // ปิดไฟ
        yield return new WaitForSeconds(Random.Range(1f, 3f));  // รอ 1-3 วินาที
        pointLight.intensity = maxIntensity;  // เปิดไฟใหม่
    }

    void Flicker()
    {
        if (Random.value > 0.8f)  // มีโอกาส 20% ที่ไฟจะดับสนิท
        {
            StartCoroutine(TurnOffLight());
        }
        else
        {
            pointLight.intensity = Random.Range(minIntensity, maxIntensity);
        }

        if (Random.value > 0.5f)
        {
            flickerSound.Play();
        }
    }
}
