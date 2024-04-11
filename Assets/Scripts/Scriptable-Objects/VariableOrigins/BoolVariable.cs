using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Variables/Bool")]
public class BoolVariable : ScriptableObject
{
    public bool value;
}
[Serializable]
public class BoolReference
{
    public BoolVariable Variable;
    public bool constantValue;
    public bool useConstant = true;
    public bool value
    {
        get { return useConstant ? constantValue : Variable.value; }
        set { Variable.value = value; constantValue = value; }
    }
}
