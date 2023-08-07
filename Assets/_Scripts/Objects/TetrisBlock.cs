using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    [SerializeField] private BlockColorsSO blockColorsSO;
    [SerializeField] private Sprite previewImage;

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        SetupSpriteRendererColors();
    }

    private void SetupSpriteRendererColors()
    {
        Color randomColor = blockColorsSO.GetRandomColor();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.color = randomColor;
                spriteRenderers.Add(spriteRenderer);
            }
        }
    }
    public void SetColor(Color color)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = color;
        }
    }
    public Color GetColor()
    {
        return spriteRenderers[0].color;
    }
    public Sprite GetPreviewImage()
    {
        return previewImage;
    }
}
