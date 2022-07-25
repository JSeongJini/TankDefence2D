using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] protected int poolSize = 0;
    [SerializeField] protected GameObject prefab = null;
    [SerializeField] protected string objectName = "";

    protected Queue<GameObject> pool = new Queue<GameObject>();

    protected delegate void OnInstantiate(GameObject _go);
    protected OnInstantiate onInstantiate;

    protected virtual void Awake()
    {
        BuildPool();
    }

    protected void BuildPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = InstantiateObject();
            go.name = objectName + i;

            pool.Enqueue(go);
        }
    }


    public GameObject GetObject()
    {
        if (pool.Count == 0)
        {
            return InstantiateObject();
        }
        else
        {
            GameObject go = pool.Dequeue();
            go.SetActive(true);
            return go;
        }
    }

    private GameObject InstantiateObject()
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform);
        go.SetActive(false);

        onInstantiate?.Invoke(go);

        return go;
    }

    public void ReturnObject(GameObject _go)
    {
        _go.SetActive(false);
        pool.Enqueue(_go);
    }
}
