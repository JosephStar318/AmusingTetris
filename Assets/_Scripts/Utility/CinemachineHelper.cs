using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineHelper : MonoBehaviour
{
    public static CinemachineHelper Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;

    [SerializeField] private Transform defaultTarget;

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

    public void FollowTarget(Transform target)
    {
        foreach (var camera in virtualCameras)
        {
            camera.m_Follow = target;
            camera.m_LookAt = target;
        }
      
    }
    public void FollowDefault()
    {
        foreach (var camera in virtualCameras)
        {
            camera.m_Follow = defaultTarget;
            camera.m_LookAt = defaultTarget;
        }
    }

}
