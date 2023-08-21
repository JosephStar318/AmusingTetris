using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> groundedSfx;
    [SerializeField] private List<AudioClip> breakSfx;
    [SerializeField] private AudioClip levelUpSfx;
    [SerializeField] private AudioClip tetrisSfx;

    private void OnEnable()
    {
        GameManager.OnBlockGrounded += GameManager_OnBlockGrounded;
        GameGrid.OnBeforeRowsCleared += GameGrid_OnBeforeRowsCleared;
        ScoreManager.OnLevelUp += ScoreManager_OnLevelUp;
    }
    private void OnDisable()
    {
        GameManager.OnBlockGrounded -= GameManager_OnBlockGrounded;
        GameGrid.OnBeforeRowsCleared -= GameGrid_OnBeforeRowsCleared;
        ScoreManager.OnLevelUp -= ScoreManager_OnLevelUp;
    }
    private void ScoreManager_OnLevelUp(int obj)
    {
        AudioUtility.CreateSFX(levelUpSfx, transform.position, AudioUtility.AudioGroups.SFX, 1);
    }
    private void GameGrid_OnBeforeRowsCleared(int rowCount, List<List<Transform>> rowList, System.Action callback)
    {
        for (int i = 0; i < rowCount; i++)
        {
            AudioUtility.CreateSFXRandom(breakSfx, transform.position, AudioUtility.AudioGroups.SFX, 1f);
        }
        if (rowCount == 4) AudioUtility.CreateSFX(tetrisSfx, transform.position, AudioUtility.AudioGroups.SFX, 1f);
    }
    private void GameManager_OnBlockGrounded(TetrisBlock block)
    {
        AudioUtility.CreateSFXRandom(groundedSfx, block.transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
    }
}
