using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameOver;

    [SerializeField] private GameGrid grid;

    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;

    private TimeLimiter verticalMovementLimiter;
    private TimeLimiter horizontalMovementLimiter;

    private TetrisBlock activeTetrisBlock;


    private float lastInputTime;
    private float inputInterval = 0.1f;
    private void OnEnable()
    {
        PlayerInputHelper.OnRotate += PlayerInputHelper_OnRotate;
        GameGrid.OnRowsHandled += GameGrid_OnRowsHandled;
    }

    private void OnDisable()
    {
        PlayerInputHelper.OnRotate -= PlayerInputHelper_OnRotate;
        GameGrid.OnRowsHandled -= GameGrid_OnRowsHandled;
    }

    private void Start()
    {
        verticalMovementLimiter = new TimeLimiter(verticalSpeed);
        horizontalMovementLimiter = new TimeLimiter(horizontalSpeed);

        SpawnTetrisBlock();
    }

    private void SpawnTetrisBlock()
    {
        if (grid.IsSpawnAreaValid())
        {
            activeTetrisBlock = grid.SpawnTetrisBlock();
        }
        else
        {
            Debug.Log("Game Over");
            activeTetrisBlock = null;
            OnGameOver?.Invoke();
        }
    }

    private void Update()
    {
        PlayerMovementControl();

        if (verticalMovementLimiter.IsEnoughTimePassed())
        {
            VerticalMovement();
        }
    }
    private void VerticalMovement()
    {
        if (activeTetrisBlock == null) return;

        if (grid.IsDownValid(activeTetrisBlock))
        {
            activeTetrisBlock.transform.Translate(Vector2.down * grid.CellHeight, Space.World);
        }
        else
        {
            Debug.Log("HANDLE ROWS");
            TetrisBlock temp = activeTetrisBlock;
            activeTetrisBlock = null;
            grid.HandleCompletedRows(temp);
        }
    }
    private void PlayerMovementControl()
    {
        if (activeTetrisBlock == null) return;

        if(Time.time - lastInputTime > inputInterval)
        {
            Vector2 moveVector = PlayerInputHelper.Instance.GetMovementVector();

            if (moveVector.x == 1)
            {
                if (grid.IsRightValid(activeTetrisBlock))
                {
                    activeTetrisBlock.transform.Translate(Vector2.right * grid.CellWidth, Space.World);
                    lastInputTime = Time.time;
                }
            }
            else if (moveVector.x == -1)
            {
                if (grid.IsLeftValid(activeTetrisBlock))
                {
                    activeTetrisBlock.transform.Translate(-Vector2.right * grid.CellWidth, Space.World);
                    lastInputTime = Time.time;
                }
            }
            if (moveVector.y == -1)
            {
                VerticalMovement();
                lastInputTime = Time.time;
            }
        }
    }

    private void GameGrid_OnRowsHandled()
    {
        SpawnTetrisBlock();
    }

    private void PlayerInputHelper_OnRotate()
    {
        if (activeTetrisBlock == null) return;

         grid.RotateBlockInGrid(activeTetrisBlock);
    }

}
