using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image previewImage;
    [SerializeField] private GameObject removeAdsBtn;
    private void OnEnable()
    {
        GameGrid.OnPreviewChanged += GameGrid_OnPreviewChanged;
    }
    private void OnDisable()
    {
        GameGrid.OnPreviewChanged -= GameGrid_OnPreviewChanged;
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
}
