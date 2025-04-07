using UnityEngine.SceneManagement;

public static class Loader 
{
    public enum Scenes { 
        MenuScene,
        GameScene,
        LoadingScene
    }

    private static Scenes targetScene;

    public static void Load(Scenes targetScene) {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scenes.LoadingScene.ToString());
    }

    public static void LoaderCallBack() {
        //UnityEngine.Debug.Log("LoaderCallBack ���������! ���������� �����: " + targetScene);

        SceneManager.LoadScene(targetScene.ToString());
    }
}
