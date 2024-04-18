using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeScaleFlowChart : MonoBehaviour
{
    [SerializeField]
    private GameEvent _cutsceneStarted;
    [SerializeField]
    private GameEvent _cutsceneFinished;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((bool)GetComponent<Flowchart>().Variables[0].GetValue())
        {
            _cutsceneStarted.Raise();
        }
        else
        {
            _cutsceneFinished.Raise();
        }
    }
}
