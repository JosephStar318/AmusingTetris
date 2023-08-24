using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> OnLevelUp;
    public static event Action<int,int> OnScoreChanged;
    public static event Action<int> OnLevelStatusUpdate;

    [SerializeField] private GameObject scoreVfx;
    private int Level { get; set; } = 1;
    private int Score { get; set; }
    private int HighScore { get; set; }

    private int clearedRowCount;
    private int calculatedScore;
    private void OnEnable()
    {
        GameGrid.OnBeforeRowsCleared += GameGrid_OnBeforeRowsCleared; ;
    }
    private void OnDisable()
    {
        GameGrid.OnBeforeRowsCleared -= GameGrid_OnBeforeRowsCleared; ;
    }

    private void Start()
    {
        HighScore = PlayerPrefsHelper.GetHighScore();
        OnScoreChanged?.Invoke(Score, HighScore);
    }
    private void GameGrid_OnBeforeRowsCleared(int rowCount, List<List<Transform>> rowList, Action callback)
    {
        UpdateScoreStatus(rowCount);
        UpdateLevelStatus(rowCount);

        List<Transform> randomRow = rowList[UnityEngine.Random.Range(0, rowList.Count)];
        GameObject obj = Instantiate(scoreVfx, randomRow[randomRow.Count - 1].position - Vector3.forward, Quaternion.identity);
        obj.GetComponentInChildren<TextMeshPro>().SetText($"+{calculatedScore}");
    }
    private void UpdateScoreStatus(int rowCount)
    {
        calculatedScore = GetScore(rowCount);
        Score += calculatedScore;
        HighScore = Score > HighScore ? Score : HighScore;
        PlayerPrefsHelper.SetHighScore(HighScore);
        OnScoreChanged?.Invoke(Score, HighScore);
    }

    private void UpdateLevelStatus(int rowCount)
    {
        clearedRowCount += rowCount;
        int calculatedLevel = (int)(clearedRowCount / 10f) + 1;
        if (Level != calculatedLevel)
        {
            Level = calculatedLevel;
            OnLevelUp?.Invoke(Level);
        }
        OnLevelStatusUpdate?.Invoke(clearedRowCount % 10);
    }

    public int GetScore(int rowCount)
    {
        int rowMultiplier = 0;
        switch (rowCount)
        {
            case 1:
                rowMultiplier = 40;
                break;
            case 2:
                rowMultiplier = 100;
                break;
            case 3:
                rowMultiplier = 300;
                break;
            case 4:
                rowMultiplier = 1200;
                break;
            default:
                break;
        }
        return rowMultiplier * (Level + 1);
    }
}
