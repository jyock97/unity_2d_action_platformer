using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Sprite startMenuButtonImage;
    [SerializeField] private Sprite startMenuButtonHighlightImage;
    [SerializeField] private Button firstSelectedButton;
    [SerializeField] private Button[] mainButtons;

    private void Start()
    {
        firstSelectedButton.Select();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null &&
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
        {
            firstSelectedButton.Select();
        }

        foreach (Button btn in mainButtons)
        {
            SelectDeselectButton(btn);
        }
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
