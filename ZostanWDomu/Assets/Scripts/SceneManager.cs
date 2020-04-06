using UnityEngine.SceneManagement;

public static class Loader
{
    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
