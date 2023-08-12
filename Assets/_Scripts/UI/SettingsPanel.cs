using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour, IPanel
{
    public static event Action<bool> OnModeChanged;
    [SerializeField] private ToggleBar soundToggleBar;
    [SerializeField] private ToggleBar classicModeToggleBar;
    [SerializeField] private Animation panelAnim;

    private void OnEnable()
    {
        soundToggleBar.OnToggle.AddListener(OnSoundToggle);
        classicModeToggleBar.OnToggle.AddListener(OnClassicModeToggle);

    }
    private void OnDisable()
    {
        soundToggleBar.OnToggle.RemoveListener(OnSoundToggle);
        classicModeToggleBar.OnToggle.RemoveListener(OnClassicModeToggle);
    }
    private void Start()
    {
        LoadPrefs();
    }

    private void LoadPrefs()
    {
        bool soundState = PlayerPrefsHelper.GetSoundState();
        bool classicMode = PlayerPrefsHelper.GetClassicModeState();

        if (soundState)
            AudioUtility.SetMasterVolume(100);
        else
            AudioUtility.SetMasterVolume(0);

        soundToggleBar.UpdateStateWithoutNotify(soundState);
        classicModeToggleBar.UpdateStateWithoutNotify(classicMode);
        
        OnModeChanged?.Invoke(classicMode);
    }

    public void Show()
    {
        panelAnim.Play("UI_PanelAppear");
    }
    public void Hide()
    {
        panelAnim.Play("UI_PanelDisappear");
    }

    private void OnSoundToggle(bool isOn)
    {
        if(isOn)
            AudioUtility.SetMasterVolume(100);
        else
            AudioUtility.SetMasterVolume(0);

        PlayerPrefsHelper.SetSoundState(isOn);
    }
    private void OnClassicModeToggle(bool isOn)
    {
        PlayerPrefsHelper.SetClassicMode(isOn);

        OnModeChanged?.Invoke(isOn);
    }

}
