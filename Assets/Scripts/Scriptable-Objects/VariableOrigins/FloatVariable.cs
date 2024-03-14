using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    public float value;
}

[Serializable]
public class FloatReference 
{
    public FloatVariable Variable;
    public float constantValue;
    public bool useConstant = true;
    public float value
    {
        get { return useConstant ? constantValue : Variable.value; }
        set { Variable.value = value; constantValue = value; }
    }
}
