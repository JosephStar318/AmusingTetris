using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private ToggleBar soundToggleBar;
    [SerializeField] private Animation panelAnim;
    private bool isActive;

    private void OnEnable()
    {
        soundToggleBar.OnToggle.AddListener(OnToggle);

        GameManager.OnSettingsBtnPressed += GameManager_OnSettingsBtnPressed;
        GameManager.OnGameUnPaused += GameManager_OnGameUnPaused;
    }
    private void OnDisable()
    {
        soundToggleBar.OnToggle.RemoveListener(OnToggle);

        GameManager.OnSettingsBtnPressed -= GameManager_OnSettingsBtnPressed;
        GameManager.OnGameUnPaused -= GameManager_OnGameUnPaused;
    }
   
    private void GameManager_OnSettingsBtnPressed()
    {
        Show();
    }
    private void GameManager_OnGameUnPaused()
    {
        if (isActive)
        {
            Hide();
        }
    }
    public void Show()
    {
        panelAnim.Play("UI_PanelAppear");
        isActive = true;
    }
    public void Hide()
    {
        panelAnim.Play("UI_PanelDisappear");
        isActive = true;
    }

    private void OnToggle(bool isOn)
    {
        if(isOn)
            AudioUtility.SetMasterVolume(100);
        else
            AudioUtility.SetMasterVolume(0);
    }
}
