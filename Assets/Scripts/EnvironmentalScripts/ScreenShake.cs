using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform;
    public float shakeDuration; // Duration of the screenshake
    public float shakeMagnitude; // Intensity of the screenshake

    private float timeElapsed = 0f;
    private Vector3 originalPos;


    private void Start()
    {
        // Stores the original position of the camera
        originalPos = cameraTransform.localPosition; 
    }

    public void Shake()
    {
        originalPos = cameraTransform.localPosition;
        timeElapsed = 0f; // Reset the elapsed time for the shake effect
    }

    private void Update()
    {
        if (timeElapsed < shakeDuration)
        {
            // Generate random shake offset every frame
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;

            // Apply the shake offset to the camera's position
            cameraTransform.localPosition = originalPos + shakeOffset;
            
            // Increments the elapsed time
            timeElapsed += Time.deltaTime;
        }
        else
        {
            // Resets the camera's position 
            cameraTransform.localPosition = originalPos;
        }
    }
}
