using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static event Action OnGridReady;

    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;
    [SerializeField] private int cellSizeX;
    [SerializeField] private int cellSizeY;

    [SerializeField] private Vector2 startPos = Vector2.zero;
    [SerializeField] private Vector2 localSpawnPoint = Vector2.zero;

    private Vector2[,] gridCoords;
    private Vector2 BlockTransformPos => transform.TransformPoint(localSpawnPoint);

    private void Start()
    {
        SetupGrid();
    }

    private void SetupGrid()
    {
        gridCoords = new Vector2[cellSizeX, cellSizeY];
        Vector2 localStarPos = transform.TransformPoint(startPos);

        for (int i = 0; i < cellSizeX; i++)
        {
            for (int j = 0; j < cellSizeY; j++)
            {
                gridCoords[i, j] = new Vector2(localStarPos.x + i * cellWidth, localStarPos.y + j * cellHeight);
            }
        }

        OnGridReady?.Invoke();
    }


    private void OnDrawGizmos()
    {
        if (gridCoords == null) return;

        foreach (Vector2 point in gridCoords)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}
