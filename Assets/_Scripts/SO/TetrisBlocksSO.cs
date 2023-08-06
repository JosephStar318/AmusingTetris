using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Tetris Blocks")]
public class TetrisBlocksSO : ScriptableObject
{
    public List<TetrisBlock> tetrisBlocks = new List<TetrisBlock>();
    public TetrisBlock GetRandomBlock()
    {
        int randomIndex = UnityEngine.Random.Range(0, tetrisBlocks.Count);

        return tetrisBlocks[randomIndex];
    }
}
