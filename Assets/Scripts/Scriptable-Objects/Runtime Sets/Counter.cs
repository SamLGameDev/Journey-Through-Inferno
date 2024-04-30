using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Counter<T> : ScriptableObject
{
    
    protected readonly List<T> Items = new List<T>();
    public virtual void Add(T t)
    {

        if (!Items.Contains(t)) { Items.Add(t); }
    }
    public virtual void Remove(T t)
    {
        if (Items.Contains(t)) {Items.Remove(t); } 
    }
    public T GetItemAtIndex(int index)
    {
        return Items[index];
    }
    public int GetIndexOfItem(T item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Equals(item))
            { return i; }
        }
        return -1;
    }
    public int GetListSize()
    {
        return Items.Count;
    }
    public List<T> GetItems()
    {
        return Items;
    }
}
