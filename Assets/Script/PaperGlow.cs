using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PaperGlow : MonoBehaviour
{
    private Light2D paperLight;
    private float baseIntensity;
    public float pulseSpeed = 2f; // ความเร็วในการกระพริบ
    public float intensityRange = 0.5f; // ช่วงการเปลี่ยนความสว่าง

    void Start()
    {
        paperLight = GetComponent<Light2D>();
        if (paperLight != null)
        {
            baseIntensity = paperLight.intensity;
        }
    }

    void Update()
    {
        if (paperLight != null)
        {
            paperLight.intensity = baseIntensity + Mathf.Sin(Time.time * pulseSpeed) * intensityRange;
        }
    }
}
