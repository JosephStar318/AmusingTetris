using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSway : MonoBehaviour
{
    [SerializeField] private Vector2 speed = Vector2.right;
    [SerializeField] private Vector2 offset;

    List<Transform> bgList = new List<Transform>();
    private void Start()
    {
        foreach (Transform child in transform)
        {
            bgList.Add(child);
        }
    }
    void Update()
    {
        MoveRight();
        BorderCheck();
    }

    private void Shake()
    {
        StartCoroutine(transform.Shake(0.5f, 0.5f));
    }

    private void BorderCheck()
    {
        foreach (Transform bg in bgList)
        {
           if(Vector2.Distance(transform.position, bg.localPosition) >= offset.magnitude)
            {
                Transform otherBg = bgList.Find((item) => item != bg);
                bg.localPosition = otherBg.localPosition + (Vector3)offset;
            }
        }
    }

    private void MoveRight()
    {
        foreach (Transform bg in bgList)
        {
            bg.Translate(speed * Time.deltaTime);
        }
    }
}
