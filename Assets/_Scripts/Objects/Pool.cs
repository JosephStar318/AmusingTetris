using System.Collections.Generic;
using UnityEngine;

public class Pool<T>
{
    private int _size;
    private int _currentIndex;

    private List<T> poolList;
    public int Count { get => poolList.Count; }
    public Pool(int size)
    {
        _size = size;
        poolList = new List<T>(size);
    }

    public void AddToPool(T obj)
    {
        if(poolList.Count <= _size)
        {
            poolList.Add(obj);
            Debug.Log("Added");
        }
        else
        {
            poolList.Insert(0, obj);
            poolList.RemoveAt(poolList.Count - 1);
            Debug.Log("Inserted");
        }
    }

    public T GetFromPool()
    {
        T obj = poolList[_currentIndex];
        _currentIndex = (_currentIndex + 1) % _size;
        return obj;
    }

}
