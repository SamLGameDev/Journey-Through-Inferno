using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class DamageIndicator : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] Color warningColor;
    [SerializeField] Color detonateColor;

    [Header("Timing")]
    [Tooltip("Time before fade starts after trigger.")]
    [SerializeField] float holdTime;
    [Tooltip("Fade Time")]
    [SerializeField] float fadeTime;

    private SpriteRenderer sr;

    private bool fadeOut, holdColor;
    private float heldTime;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = warningColor;

        heldTime = 0;
        holdColor = true;
    }

    private void Update()
    {
        // Check to see if the color should fade.
        if (fadeOut)
        {
            // Check to see if the color should be held first.
            if (holdColor)
            {
                // Count down the time left to hold before fading.
                if (heldTime < holdTime)
                {
                    heldTime += Time.deltaTime;
                }
                else
                {
                    holdColor = false;
                }

                return;
            }

            StartCoroutine(FadeOutOverTime());

            fadeOut = false;
        }
    }

    /// <summary>
    /// 'Detonates' the indicator.
    /// </summary>
    public void TriggerDetonate()
    {
        sr.color = detonateColor;

        fadeOut = true;
    }

    /// <summary>
    /// 'Detonates' the indicator, changes the indicator to an impact mark.
    /// </summary>
    /// <param name="impactMark">Sprite of the impact mark.</param>
    public void TriggerDetonate(Sprite impactMark)
    {
        sr.color = Color.white;
        sr.sprite = impactMark;

        fadeOut = true;
    }

    private IEnumerator FadeOutOverTime()
    {
        // Loop through lerping the color from its current value towards invisible.
        // The value is checked to see if its within a certain tolerance to save time on end.
        for (float t = 0; Mathf.Abs(sr.color.a - Color.clear.a) > 0.01; t += Time.deltaTime)
        {
            float fadeAmount = t / fadeTime;

            sr.color = Color.Lerp(sr.color, Color.clear, fadeAmount);
            yield return null;
        }

        Destroy(gameObject);
    }
}
