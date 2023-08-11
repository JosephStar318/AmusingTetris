using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string _sceneToLoad;
    private static string _previousScene;
    public static void StartLoadScene(string sceneName)
    {
        _sceneToLoad = sceneName;
        _previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

    }
    public static string GetSceneToLoad()
    {
        return _sceneToLoad;
    }
    public static string GetPreviousScene()
    {
        return _previousScene;
    }
}
