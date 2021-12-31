using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Sprite startMenuButtonImage;
    [SerializeField] private Sprite startMenuButtonHighlightImage;

    public void SetStartMenuHighLightImage(Button button)
    {
        button.image.sprite = startMenuButtonHighlightImage;
    }
    public void SetStartMenuImage(Button button)
    {
        button.image.sprite = startMenuButtonImage;
    }

    public void LoadScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void QuitApplication()
    {
        Application.Quit();
    }
}
