using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisDebug : MonoBehaviour
{
    [SerializeField] public GameGrid grid;
    [SerializeField] private AnimationCurve curve;

    public void SpawnRandomBlock()
    {
        grid.SpawnTetrisBlock();
    }

    public void ZoomEffect()
    {

        CinemachineHelper.Instance.ZoomEffect(curve, 0.5f, () => CinemachineHelper.Instance.ShakeScreen(5));
    }
}
