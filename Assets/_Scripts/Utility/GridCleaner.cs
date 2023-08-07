using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCleaner : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating(nameof(CleanGrid), 10f, 10f);
    }

    private void CleanGrid()
    {
        TetrisBlock[] children = GetComponentsInChildren<TetrisBlock>();

        foreach (TetrisBlock child in children)
        {
            if(child.transform.childCount == 0)
                Destroy(child.gameObject);
        }
    }
}
