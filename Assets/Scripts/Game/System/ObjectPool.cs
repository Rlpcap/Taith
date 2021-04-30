using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T>
{
    public delegate T FactoryMethod();

    private FactoryMethod _factoryMethod;
    private List<T> _currentStock;
    private bool _isDynamic;


    private Action<T> _turnOnCallback;
    private Action<T> _turnOffCallback;

    public ObjectPool(FactoryMethod factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialStock = 0, bool isDynamic = true)
    {
        _factoryMethod = factoryMethod;
        _turnOnCallback = turnOnCallback;
        _turnOffCallback = turnOffCallback;

        _isDynamic = isDynamic;

        _currentStock = new List<T>();

        for (int i = 0; i < initialStock; i++)
        {
            var obj = _factoryMethod();
            _turnOffCallback(obj);
            _currentStock.Add(obj);
        }
    }

    public T GetObject()
    {
        Debug.Log("GETTING OBJECT");
        Debug.Log(_currentStock.Count);
        var result = default(T);

        if (_currentStock.Count > 0)
        {
            Debug.Log("GETTING FIRST ELEMENT");
            result = _currentStock[0];
            _currentStock.RemoveAt(0);
        }
        else if (_isDynamic)
        {
            Debug.Log("Create New Element");
            result = _factoryMethod();
        }

        _turnOnCallback(result);

        Debug.Log(_currentStock.Count);


        return result;
    }


    public void ReturnObject(T obj)
    {
        _turnOffCallback(obj);
        _currentStock.Add(obj);
    }
}

