using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Sprite startMenuButtonImage;
    [SerializeField] private Sprite startMenuButtonHighlightImage;

    private void Start()
    {
        startButton.Select();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null &&
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
        {
            startButton.Select();
        } 
        SelectDeselectButton(startButton);
        SelectDeselectButton(exitButton);
    }

    private void SelectDeselectButton(Button button)
    {
        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            SetStartMenuHighLightImage(button);
        }
        else
        {
            SetStartMenuImage(button);
        }
    }

    public void HoverSelect(Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    private void SetStartMenuHighLightImage(Button button)
    {
        button.image.sprite = startMenuButtonHighlightImage;
    }
    private void SetStartMenuImage(Button button)
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
