using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image previewImage;
    [SerializeField] private GameObject removeAdsBtn;

    [SerializeField] private GameObject classicControls;
    [SerializeField] private GameObject modernControls;
    private void OnEnable()
    {
        GameGrid.OnPreviewChanged += GameGrid_OnPreviewChanged;
        SettingsPanel.OnModeChanged += SettingsPanel_OnModeChanged;
        IAPManager.OnPremiumPurchase += IAPManager_OnPremiumPurchase;
    }

    private void OnDisable()
    {
        GameGrid.OnPreviewChanged -= GameGrid_OnPreviewChanged;
        SettingsPanel.OnModeChanged -= SettingsPanel_OnModeChanged;
        IAPManager.OnPremiumPurchase -= IAPManager_OnPremiumPurchase;
    }
    private void Start()
    {
        removeAdsBtn.SetActive(PlayerPrefsHelper.GetPremiumState() == false);
    }
    private void GameGrid_OnPreviewChanged(TetrisBlock block)
    {
        previewImage.sprite = block.GetPreviewImage();
        previewImage.color = block.GetColor();
    }
    private void SettingsPanel_OnModeChanged(bool isClassicMode)
    {
        if (isClassicMode)
        {
            classicControls.SetActive(true);
            modernControls.SetActive(false);
        }
        else
        {
            classicControls.SetActive(false);
            modernControls.SetActive(true);
        }
    }

    private void IAPManager_OnPremiumPurchase()
    {
        removeAdsBtn.SetActive(false);
    }
}
