using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ModernControls : MonoBehaviour
{
    public UnityEvent OnRight;
    public UnityEvent OnLeft;
    public UnityEvent OnDown;
    public UnityEvent OnRotate;

    private bool fingerDown;
    private Vector2 movePos;
    [SerializeField] private float pixelDistToDetect = 100f;
    [SerializeField] private float touchPeriod = 0.05f;

    private float lastTouchTime;

    private void Update()
    {
        if(Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                movePos = Vector2.zero;
                lastTouchTime = Time.time;
            }
            else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                movePos += touch.deltaPosition;

                if (movePos.x > pixelDistToDetect)
                {
                    movePos = Vector2.zero;
                    OnRight?.Invoke();
                    Debug.Log("Swipe right");
                }
                else if (movePos.x < -pixelDistToDetect)
                {
                    movePos = Vector2.zero;
                    OnLeft?.Invoke();
                    Debug.Log("Swipe left");
                }
                else if (movePos.y < -pixelDistToDetect)
                {
                    movePos = Vector2.zero;
                    Debug.Log("Swipe Down");
                    OnDown?.Invoke();
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                if(Time.time - lastTouchTime < touchPeriod)
                {
                    OnRotate?.Invoke();
                }
            }

           
        }
    }
 
}
