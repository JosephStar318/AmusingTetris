using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CinemachineHelper : MonoBehaviour
{
    public static CinemachineHelper Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private Transform defaultTarget;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    /// <summary>
    /// Curve should be in range of (-1, 1]
    /// 
    /// </summary>
    /// <param name="curve"></param>
    /// <param name="inTime"></param>
    /// <param name="time"></param>
    public void ZoomEffect(AnimationCurve curve, float time, Action callback = null)
    {
        StartCoroutine(Zoom(curve, time, callback));
    }
    public void ShakeScreen(float force)
    {
        impulseSource.GenerateImpulse(force);
    }

    private IEnumerator Zoom(AnimationCurve curve, float time, Action callback)
    {
        float startSize = virtualCamera.m_Lens.OrthographicSize;

        float elapsedTime = 0;
        while(elapsedTime < time)
        {
            virtualCamera.m_Lens.OrthographicSize = startSize * (curve.Evaluate(elapsedTime / time) + 1);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        virtualCamera.m_Lens.OrthographicSize = startSize;
        callback?.Invoke();
    }


}
