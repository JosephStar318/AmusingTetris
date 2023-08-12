using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    void Update()
    {
        transform.Translate(-Vector2.up * speed * Time.deltaTime);
    }
}
