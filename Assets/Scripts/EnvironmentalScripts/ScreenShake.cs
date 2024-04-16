using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{   
    public static ScreenShake Instance {  get; private set; } // Static instance property allows other scripts to access this one
    
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    private CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin;
    
    private float shakeTimer; // Timer for the duration of the screen shake


    private void Awake()
    {
        Instance = this; // Assign this script instance to the static Instance property

        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity , float time)
    {     
        CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity; // Sets the amplitude gain of the CinemachineBasicMultiChannelPerlin to the specified intensity
        
        shakeTimer = time; // Sets the shakeTimer to the specified duration
    }

    private void Update()
    {
        if (shakeTimer > 0) // If the shakeTimer is greater than 0, decrease the value each frame 
        {
            shakeTimer -= Time.deltaTime;
            
            if(shakeTimer <= 0f) // If the shakeTimer hits 0 or less, it resets the amplitude to 0
            
            { CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f; }
        }

    }
}
