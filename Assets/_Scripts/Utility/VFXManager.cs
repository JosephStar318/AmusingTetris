using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve zoomCurve;

    private void OnEnable()
    {
        GameGrid.OnRowCleared += GameGrid_OnRowCleared;
    }
    private void OnDisable()
    {
        GameGrid.OnRowCleared -= GameGrid_OnRowCleared;
    }

    private void GameGrid_OnRowCleared(int rowCount)
    {
        switch (rowCount)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                CinemachineHelper.Instance.ZoomEffect(zoomCurve, 0.5f, () => CinemachineHelper.Instance.ShakeScreen(5));
                break;
            default:
                break;
        }
    }
}
