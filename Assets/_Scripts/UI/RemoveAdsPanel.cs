using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdsPanel : MonoBehaviour
{
    [SerializeField] private Animation panelAnim;
    private bool isActive;

    private void OnEnable()
    {
        GameManager.OnRemoveAdsBtnPressed += GameManager_OnRemoveAdsBtnPressed;
        GameManager.OnGameUnPaused += GameManager_OnGameUnPaused;
    }
    private void OnDisable()
    {
        GameManager.OnRemoveAdsBtnPressed -= GameManager_OnRemoveAdsBtnPressed;
        GameManager.OnGameUnPaused -= GameManager_OnGameUnPaused;
    }
    private void GameManager_OnGameUnPaused()
    {
        if (isActive)
        {
            Hide();
        }
    }

    private void GameManager_OnRemoveAdsBtnPressed()
    {
        Show();
    }

    public void Show()
    {
        panelAnim.Play("UI_PanelAppear");
        isActive = true;
    }
    public void Hide()
    {
        panelAnim.Play("UI_PanelDisappear");
        isActive = false;
    }
}
