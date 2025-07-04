using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TouchButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnInterract;

    [SerializeField] private GameObject pressIndicator;
    [SerializeField] private float touchPeriod = 0.2f;

    private float lastInterractTime;

    private bool isTouching = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouching = true;
        pressIndicator.SetActive(true);
        pressIndicator.transform.position = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
        pressIndicator.SetActive(false);
    }
    
    private void Update()
    {
        if (isTouching)
        {
            if(Time.time - lastInterractTime > touchPeriod)
            {
                lastInterractTime = Time.time;
                OnInterract?.Invoke();
            }
        }
    }
}
