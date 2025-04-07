using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            Debug.Log("Pressed start game");
            Loader.Load(Loader.Scenes.GameScene);
        });

        quitButton.onClick.AddListener(QuitClick);

        Time.timeScale = 1f;
    }

    //just for example
    private void QuitClick() { 
        Application.Quit();
    }
}
