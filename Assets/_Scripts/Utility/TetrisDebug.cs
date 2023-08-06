using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisDebug : MonoBehaviour
{
    [SerializeField] public GameGrid grid;
    public void SpawnRandomBlock()
    {
        grid.SpawnTetrisBlock();
    }
}
