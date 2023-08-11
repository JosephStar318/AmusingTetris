using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameOver;
    public static event Action OnGamePaused;
    public static event Action OnGameUnPaused;
    public static event Action OnRemoveAdsBtnPressed;
    public static event Action OnSettingsBtnPressed;

    [SerializeField] private GameGrid grid;

    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;

    private TimeLimiter verticalMovementLimiter;
    private TimeLimiter horizontalMovementLimiter;

    private TetrisBlock activeTetrisBlock;

    private bool isGamePaused;
    
    private float lastInputTime;
    private float inputInterval = 0.1f;
    private void OnEnable()
    {
        PlayerInputHelper.OnRotate += PlayerInputHelper_OnRotate;
        GameGrid.OnRowsHandled += GameGrid_OnRowsHandled;
        ScoreManager.OnLevelUp += ScoreManager_OnLevelUp;
    }

    private void OnDisable()
    {
        PlayerInputHelper.OnRotate -= PlayerInputHelper_OnRotate;
        GameGrid.OnRowsHandled -= GameGrid_OnRowsHandled;
        ScoreManager.OnLevelUp -= ScoreManager_OnLevelUp;
    }

    private void Start()
    {
        verticalMovementLimiter = new TimeLimiter(verticalSpeed);
        horizontalMovementLimiter = new TimeLimiter(horizontalSpeed);

        SpawnTetrisBlock();
    }

    private void ScoreManager_OnLevelUp(int level)
    {
        GameSpeedUp(level);
    }

    private void GameSpeedUp(int level)
    {

    }
    public void OpenSettings()
    {
        PauseGame();
        OnSettingsBtnPressed?.Invoke();
    }
    public void OpenRemoveAdsPanel()
    {
        PauseGame();
        OnRemoveAdsBtnPressed?.Invoke();
    }
    public void PauseGame()
    {
        isGamePaused = true;
        OnGamePaused?.Invoke();
    }
    public void UnpauseGame()
    {
        isGamePaused = false;
        OnGameUnPaused?.Invoke();
    }
    public void RestartLevel()
    {
        SceneLoader.StartLoadScene(SceneManager.GetActiveScene().name);
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
        if (isGamePaused) return;

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
