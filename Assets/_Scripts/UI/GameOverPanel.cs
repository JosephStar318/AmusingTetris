using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Animation panelAnim;
    private bool isActive;

    private void OnEnable()
    {
        GameManager.OnGameOver += GameManager_OnGameOver;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= GameManager_OnGameOver;
    }
    private void GameManager_OnGameOver()
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
