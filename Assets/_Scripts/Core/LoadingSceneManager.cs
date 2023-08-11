using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string sceneToLoad;
    private string previousSceneName;
    private void Start()
    {
        sceneToLoad = SceneLoader.GetSceneToLoad();
        previousSceneName = SceneLoader.GetPreviousScene();

        if (string.IsNullOrEmpty(sceneToLoad) == false)
        {
            animator.SetTrigger("Loading");
            AsyncOperation unloadingOp = SceneManager.UnloadSceneAsync(previousSceneName);
            unloadingOp.completed += UnloadingOp_completed;

        }
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
    public void Unload()
    {
        SceneManager.UnloadSceneAsync("LoadingScene");
    }
}
