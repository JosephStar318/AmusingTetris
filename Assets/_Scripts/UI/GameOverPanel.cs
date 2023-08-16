using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour, IPanel
{
    [SerializeField] private Animation panelAnim;
    public void Show()
    {
        panelAnim.Play("UI_PanelAppear");
    }
    public void Hide()
    {
        panelAnim.Play("UI_PanelDisappear");
    }
}
