using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Variables/Int")]
public class IntVariable : ScriptableObject
{
    public int value;
}

[Serializable]
public class IntReference
{
    public IntVariable Variable;
    public int constantValue;
    public bool useConstant = true;
    public int value
    {
        get { return useConstant ? constantValue : Variable.value; }
        set { Variable.value = value; constantValue = value; }
    }
}
