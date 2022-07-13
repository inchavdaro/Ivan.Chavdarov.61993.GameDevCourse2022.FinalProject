using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class ScreenShakeController : MonoBehaviour
{
    [SerializeField] private AnimationCurve shakeCurve = null;

    [SerializeField]
    [Range(0, 10)]
    private float lightIntensity = 0.5f;

    [SerializeField]
    [Range(0, 10)]
    private float highIntensity = 2f;

    [SerializeField]
    [Range(0, 1)]
    private float duration = 0.3f;

    private static ScreenShakeController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static void ShakeScreenLight()
    {
        ShakeScreen(instance.lightIntensity);
    }

    public static void ShakeScreenHeavy()
    {
        ShakeScreen(instance.highIntensity);
    }

    private static void ShakeScreen(float intensity)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.ShakeScreenCoroutine(intensity));
    }

    private IEnumerator ShakeScreenCoroutine(float intensity)
    {
        float shakeStart = Time.time;
        float shakeEnd = shakeStart + duration;

        float noiseSeed = Random.value * 1000;
        float cameraJiggle = intensity * 10;

        while (Time.time < shakeEnd)
        {
            float normalizedTime = (Time.time - shakeStart) / duration;
            float offsetX = PerlinNoise(noiseSeed + Time.time * cameraJiggle, 0);
            float offsetY = PerlinNoise(0, noiseSeed + Time.time * cameraJiggle);

            Vector3 offset = new Vector2(offsetX, offsetY) * shakeCurve.Evaluate(normalizedTime) * intensity;

            transform.position = transform.position + offset;
            yield return null;
        }
    }
}