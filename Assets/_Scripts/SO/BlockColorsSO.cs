using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Block Colors")]
public class BlockColorsSO : ScriptableObject
{
    public List<Color> blockColors = new List<Color>();
    

    public Color GetRandomColor()
    {
        int randomColorIndex = UnityEngine.Random.Range(0, blockColors.Count);

        return blockColors[randomColorIndex];
    }
}
