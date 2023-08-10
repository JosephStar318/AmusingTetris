using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> OnLevelUp;
    public static event Action<int,int> OnScoreChanged;
    public static event Action<int> OnLevelStatusUpdate;

    private int Level { get; set; }
    private int Score { get; set; }
    private int HighScore { get; set; }

    private int clearedRowCount;

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
        UpdateScoreStatus(rowCount);
        UpdateLevelStatus(rowCount);
    }

    private void UpdateScoreStatus(int rowCount)
    {
        Score += GetScore(rowCount);
        HighScore = Score > HighScore ? Score : HighScore;
        OnScoreChanged?.Invoke(Score, HighScore);
    }

    private void UpdateLevelStatus(int rowCount)
    {
        clearedRowCount += rowCount;
        int calculatedLevel = (int)(clearedRowCount / 10f);
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
