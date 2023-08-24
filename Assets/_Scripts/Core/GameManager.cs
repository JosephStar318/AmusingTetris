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
    public static event Action<TetrisBlock> OnBlockGrounded;

    [SerializeField] private GameGrid grid;
    [SerializeField] private SettingsPanel settingsPanel;
    [SerializeField] private RemoveAdsPanel removeAdsPanel;
    [SerializeField] private GameOverPanel gameOverPanel;

    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;

    [SerializeField] private AudioClip moveSound;

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
        IAPManager.OnPremiumPurchase += IAPManager_OnPremiumPurchase;
    }

    private void OnDisable()
    {
        PlayerInputHelper.OnRotate -= PlayerInputHelper_OnRotate;
        GameGrid.OnRowsHandled -= GameGrid_OnRowsHandled;
        ScoreManager.OnLevelUp -= ScoreManager_OnLevelUp;
        IAPManager.OnPremiumPurchase -= IAPManager_OnPremiumPurchase;
    }
    private void Start()
    {
        verticalMovementLimiter = new TimeLimiter(1 / verticalSpeed);
        horizontalMovementLimiter = new TimeLimiter(1 / horizontalSpeed);
        isGamePaused = true;
        SpawnTetrisBlock();

        StartCoroutine(Delay.Seconds(2f, () => isGamePaused = false));
    }
    private void FixedUpdate()
    {
        if (isGamePaused) return;

        PlayerMovementControl();

        if (verticalMovementLimiter.IsEnoughTimePassed())
        {
            VerticalMovement();
        }
    }
    private void ScoreManager_OnLevelUp(int level)
    {
        GameSpeedUp(level);
    }

    private void GameSpeedUp(int level)
    {
        verticalSpeed += 0.5f;
        verticalMovementLimiter = new TimeLimiter(1 / verticalSpeed);
    }
    public void OpenSettings()
    {
        PauseGame();
        settingsPanel.Show();
    }
    public void CloseSettings()
    {
        UnpauseGame();
        settingsPanel.Hide();
    }
    private void IAPManager_OnPremiumPurchase()
    {
        CloseRemoveAdsPanel();
    }
    public void OpenRemoveAdsPanel()
    {
        PauseGame();
        removeAdsPanel.Show();
    }
    public void CloseRemoveAdsPanel()
    {
        UnpauseGame();
        removeAdsPanel.Hide();
    }
    public void ResumeFromHalf()
    {
        AdsManager.Instance.ShowRewardedAd(
            onSuccess: () =>
            {
                CloseGameOverPanel();
                grid.ClearHalf();
                StartCoroutine(Delay.Seconds(0.2f, () => SpawnTetrisBlock()));
            },
            onFail: () => Debug.Log("Failed")
            );
    }
    private void CloseGameOverPanel()
    {
        gameOverPanel.Hide();
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
        AdsManager.Instance.ShowInterstitialAd(() => SceneLoader.StartLoadScene(SceneManager.GetActiveScene().name));
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
            gameOverPanel.Show();
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
            TetrisBlock temp = activeTetrisBlock;
            activeTetrisBlock = null;
            OnBlockGrounded?.Invoke(temp);
            Debug.Log("Block grounded");
            grid.HandleCompletedRows(temp);
        }
    }
    private void PlayerMovementControl()
    {
        if (activeTetrisBlock == null) return;

        if (Time.time - lastInputTime > inputInterval)
        {
            Vector2 moveVector = PlayerInputHelper.Instance.GetMovementVector();

            if (moveVector.x == 1)
            {
                if (grid.IsRightValid(activeTetrisBlock))
                {
                    activeTetrisBlock.transform.Translate(Vector2.right * grid.CellWidth, Space.World);
                    AudioUtility.CreateSFX(moveSound, transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
                    lastInputTime = Time.time;
                }
            }
            else if (moveVector.x == -1)
            {
                if (grid.IsLeftValid(activeTetrisBlock))
                {
                    activeTetrisBlock.transform.Translate(-Vector2.right * grid.CellWidth, Space.World);
                    AudioUtility.CreateSFX(moveSound, transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
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
    public void Rotate()
    {
        if (activeTetrisBlock == null) return;

        grid.RotateBlockInGrid(activeTetrisBlock);
        AudioUtility.CreateSFX(moveSound, transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
    }
    public void MoveRight()
    {
        if (grid.IsRightValid(activeTetrisBlock))
        {
            activeTetrisBlock.transform.Translate(Vector2.right * grid.CellWidth, Space.World);
            AudioUtility.CreateSFX(moveSound, transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
        }
    }
    public void MoveLeft()
    {
        if (grid.IsLeftValid(activeTetrisBlock))
        {
            activeTetrisBlock.transform.Translate(-Vector2.right * grid.CellWidth, Space.World);
            AudioUtility.CreateSFX(moveSound, transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
        }
    }
    public void MoveDown()
    {
        if (activeTetrisBlock == null) return;

        if (grid.IsDownValid(activeTetrisBlock))
        {
            activeTetrisBlock.transform.Translate(Vector2.down * grid.CellHeight, Space.World);
            AudioUtility.CreateSFX(moveSound, transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
        }
        else
        {
            TetrisBlock temp = activeTetrisBlock;
            activeTetrisBlock = null;
            OnBlockGrounded?.Invoke(temp);
            grid.HandleCompletedRows(temp);
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
        AudioUtility.CreateSFX(moveSound, transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
    }

}
