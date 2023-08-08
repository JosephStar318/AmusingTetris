using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Image levelSliderBar;

    [SerializeField] private TextMeshProUGUI levelText;
    private void OnEnable()
    {
        ScoreManager.OnLevelUp += ScoreManager_OnLevelUp;
        ScoreManager.OnLevelStatusUpdate += ScoreManager_OnLevelStatusUpdate;
    }
    private void OnDisable()
    {
        ScoreManager.OnLevelUp -= ScoreManager_OnLevelUp;
        ScoreManager.OnLevelStatusUpdate -= ScoreManager_OnLevelStatusUpdate;
    }
    private void ScoreManager_OnLevelStatusUpdate(int status)
    {
        StartCoroutine(FillSlider(Mathf.InverseLerp(0, 10, status), 0.5f));
    }

    private void ScoreManager_OnLevelUp(int level)
    {
        levelText.SetText(level.ToString());
    }
    private IEnumerator FillSlider(float target, float time)
    {
        float elapsedTime = 0;
        float startAmount = Mathf.InverseLerp(0, 1, levelSliderBar.fillAmount);

        while (elapsedTime < time)
        {
            levelSliderBar.fillAmount = Mathf.Lerp(startAmount, target, elapsedTime / time);
            elapsedTime += 0.02f;
            yield return new WaitForSeconds(0.02f);
        }
        levelSliderBar.fillAmount = target;

    }
}
