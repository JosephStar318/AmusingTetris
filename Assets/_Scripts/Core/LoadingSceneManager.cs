using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip popSound;

    private string sceneToLoad;
    private string previousSceneName;
    private void Start()
    {
        sceneToLoad = SceneLoader.GetSceneToLoad();
        previousSceneName = SceneLoader.GetPreviousScene();
    }
    private void LoadScene()
    {
        AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        loadingOp.completed += LoadingOp_completed;
    }

    private void UnloadingOp_completed(AsyncOperation operation)
    {
        StartCoroutine(Delay.Seconds(2f, () => LoadScene()));
    }

    private void LoadingOp_completed(AsyncOperation operation)
    {
        animator.SetTrigger("Loaded");
    }
    public void UnloadPreviousScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad) == false)
        {
            AsyncOperation unloadingOp = SceneManager.UnloadSceneAsync(previousSceneName);
            unloadingOp.completed += UnloadingOp_completed;
        }
    }
    public void UnloadLoadingScene()
    {
        SceneManager.UnloadSceneAsync("LoadingScene");
    }
    public void PlaySoundEffects()
    {
        AudioUtility.CreateSFX(popSound, transform.position, AudioUtility.AudioGroups.SFX, 0.8f);
    }
}
