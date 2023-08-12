using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteRain : MonoBehaviour
{
    [SerializeField] private GameObject meteoritePrefab;

    private Pool<GameObject> meteorPool = new Pool<GameObject>(5);


    private void Start()
    {
        InvokeRepeating(nameof(MeteoriteSpawner), 2, 8f);
    }
    private void MeteoriteSpawner()
    {
        if (meteorPool.Count < 5)
        {
            SpawnMeteorite();
        }
        else
        {
           meteorPool.GetFromPool().transform.position = GetRandomPos();
        }
    }
    private Vector3 GetRandomPos()
    {
        Vector3 randomPos = transform.position + UnityEngine.Random.insideUnitSphere * 5f;
        return randomPos;
    }

    private void SpawnMeteorite()
    {
        Vector3 randomSpawnPos = GetRandomPos();
        GameObject obj = Instantiate(meteoritePrefab, randomSpawnPos, meteoritePrefab.transform.rotation);
        meteorPool.AddToPool(obj);
    }
}
