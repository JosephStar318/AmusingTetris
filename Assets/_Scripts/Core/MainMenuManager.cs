using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private SettingsPanel settingsPanel;
    [SerializeField] private RemoveAdsPanel removeAdsPanel;
    [SerializeField] private GameObject removeAdsBtn;

    private void Start()
    {
        highScoreText.SetText(PlayerPrefsHelper.GetHighScore().ToString());
        removeAdsBtn.SetActive(PlayerPrefsHelper.GetPremiumState() == false);
    }
    public void LoadGame()
    {
        SceneLoader.StartLoadScene("Gameplay");
    }

    public void OpenSettings()
    {
        settingsPanel.Show();
    }
    public void OpenRemoveAdsPanel()
    {
        removeAdsPanel.Show();
    }
    public void Quit()
    {
        Application.Quit();
    }

}
