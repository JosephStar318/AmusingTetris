using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += ScoreManager_OnScoreChanged;
    }
    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= ScoreManager_OnScoreChanged;
    }
    private void ScoreManager_OnScoreChanged(int score, int highScore)
    {
        scoreText.SetText(score.ToString());
        highScoreText.SetText(highScore.ToString());
    }
}
