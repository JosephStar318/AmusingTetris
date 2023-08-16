using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve zoomCurve;
    [SerializeField] private ParticleSystem groundImpactVfx;
    [SerializeField] private ParticleSystem destroyVfx;

    [SerializeField] private ParticleSystem doubleRowVfx;
    [SerializeField] private ParticleSystem tripleRowVfx;
    [SerializeField] private ParticleSystem tetrisVfx;
    [SerializeField] private ParticleSystem levelUpVfx;

    private void OnEnable()
    {
        GameGrid.OnBeforeRowsCleared += GameGrid_OnRowCleared;
        GameManager.OnBlockGrounded += GameManager_OnBlockGrounded;
        ScoreManager.OnLevelUp += ScoreManager_OnLevelUp;
    }
    private void OnDisable()
    {
        GameGrid.OnBeforeRowsCleared -= GameGrid_OnRowCleared;
        GameManager.OnBlockGrounded -= GameManager_OnBlockGrounded;
        ScoreManager.OnLevelUp -= ScoreManager_OnLevelUp;
    }

    private void ScoreManager_OnLevelUp(int obj)
    {
        Instantiate(levelUpVfx, transform);
    }

    private void GameManager_OnBlockGrounded(TetrisBlock block)
    {
        foreach (Transform child in block.transform)
        {
            ParticleSystem particle = Instantiate(groundImpactVfx, child.position, Quaternion.identity);
            ParticleSystem.MainModule mainModule = particle.main;
            mainModule.startColor = block.GetColor();
        }
    }
    private void GameGrid_OnRowCleared(int rowCount, List<List<Transform>> rowList, Action callback)
    {
        switch (rowCount)
        {
            case 1:
                break;
            case 2:
                Instantiate(doubleRowVfx, transform.position, transform.rotation).Play();
                CinemachineHelper.Instance.ShakeScreen(1f);
                break;
            case 3:
                Instantiate(tripleRowVfx, transform.position, transform.rotation).Play();
                CinemachineHelper.Instance.ShakeScreen(2f);
                break;
            case 4:
                Instantiate(tetrisVfx, transform.position, transform.rotation).Play();
                CinemachineHelper.Instance.ZoomEffect(zoomCurve, 0.5f, () => CinemachineHelper.Instance.ShakeScreen(5));
                break;
            default:
                break;
        }

        foreach (List<Transform> row in rowList)
        {
            foreach (Transform singleBlock in row)
            {
                ParticleSystem particle = Instantiate(destroyVfx, singleBlock.position, Quaternion.identity);
                ParticleSystem.MainModule mainModule = particle.main;
                mainModule.startColor = singleBlock.GetComponent<SpriteRenderer>().color;
            }
        }
        callback?.Invoke();
    }
}
