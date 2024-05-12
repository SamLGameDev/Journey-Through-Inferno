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

    public bool StartFadeOut = false;

    public bool StartFadeIn = false;

    private void Start()
    {
        vol = GetComponent<Volume>();
        vol.profile.TryGet(out vig);

        vignetteAmount = vig.intensity.value;

        vol.enabled = true;
    }

    public IEnumerator FadeIn()
    {
        while (true)
        {
            yield return new WaitUntil(() => StartFadeIn);
            float t = 0;
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                float normalisedTime = t / fadeTime;

                vig.intensity.Override(Mathf.Lerp(0, vignetteAmount, normalisedTime));

                yield return null;
            }
            StartFadeIn = false;


        }
    }

    public IEnumerator FadeOut()
    {

        while (true)
        {
            Debug.Log("ReachedEnd");
            yield return new WaitUntil(() => StartFadeOut);
            float t = 0;
            while (vig.intensity.value > 0.001)
            {
                t += Time.deltaTime;
                float normalisedTime = t / fadeTime;

                vig.intensity.Override(Mathf.Lerp(vignetteAmount, 0, normalisedTime));
                Debug.Log("Viginetefadeout " + vig.intensity.value);

                yield return null;
            }
            

            StartFadeOut = false;
            
        }

    }

    private void OnDisable()
    {
        vig.intensity.Override(0f);
    }
}
