using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class VignetteFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeTime;

    private Volume vol;
    private UnityEngine.Rendering.Universal.Vignette vig;

    private float vignetteAmount;

    private void Start()
    {
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out vig);

        vignetteAmount = vig.intensity.value;

        vol.enabled = true;
        gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        gameObject.SetActive(true);

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            float normalisedTime = t / fadeTime;

            vig.intensity.Override(Mathf.Lerp(0, vignetteAmount, normalisedTime));

            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            float normalisedTime = t / fadeTime;

            vig.intensity.Override(Mathf.Lerp(vignetteAmount, 0, normalisedTime));

            if (vig.intensity.value < 0)
            {
                gameObject.SetActive(false);
            }

            yield return null;
        }
    }

    private void OnDisable()
    {
        vig.intensity.Override(0f);
    }
}
