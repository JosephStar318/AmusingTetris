using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour, IPanel
{
    [SerializeField] private Animation panelAnim;
    [SerializeField] private GameObject watchAdBtn;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        AdsManager.OnOptionalLimitReached += AdsManager_OnOptionalLimitReached;
    }
    private void OnDisable()
    {
        AdsManager.OnOptionalLimitReached -= AdsManager_OnOptionalLimitReached;
    }
    private void AdsManager_OnOptionalLimitReached()
    {
        text.SetText("Better luck next time.");
        watchAdBtn.SetActive(false);
    }

    public void Show()
    {
        panelAnim.Play("UI_PanelAppear");
    }
    public void Hide()
    {
        panelAnim.Play("UI_PanelDisappear");
    }

}
