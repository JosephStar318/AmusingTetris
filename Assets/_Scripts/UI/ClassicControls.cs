using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClassicControls : MonoBehaviour
{
    [SerializeField] private TouchButtonScript rightButton;
    [SerializeField] private TouchButtonScript leftButton;
    [SerializeField] private TouchButtonScript downButton;
    [SerializeField] private TouchButtonScript rotateButton;

    public UnityEvent OnRightMove;
    public UnityEvent OnLeftMove;
    public UnityEvent OnDownMove;
    public UnityEvent OnRotate;

    private void OnEnable()
    {
        rightButton.OnInterract += RightButton_OnInterract;
        leftButton.OnInterract += LeftButton_OnInterract;
        downButton.OnInterract += DownButton_OnInterract;
        rotateButton.OnInterract += RotateButton_OnInterract;
    }
    private void OnDisable()
    {
        rightButton.OnInterract -= RightButton_OnInterract;
        leftButton.OnInterract -= LeftButton_OnInterract;
        downButton.OnInterract -= DownButton_OnInterract;
        rotateButton.OnInterract -= RotateButton_OnInterract;
    }

    private void RotateButton_OnInterract()
    {
        OnRotate?.Invoke();
    }

    private void DownButton_OnInterract()
    {
        OnDownMove?.Invoke();
    }

    private void LeftButton_OnInterract()
    {
        OnLeftMove?.Invoke();
    }
    private void RightButton_OnInterract()
    {
        OnRightMove?.Invoke();
    }
}
