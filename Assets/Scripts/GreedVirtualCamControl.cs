using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GreedVirtualCamControl : MonoBehaviour
{
    public CinemachineVirtualCamera StartCamera;
    public CinemachineVirtualCamera PlutoCamera;


    public void ChangeCam()
    {
        StartCamera.Priority = 0;
        PlutoCamera.Priority = 5;
    }
}
