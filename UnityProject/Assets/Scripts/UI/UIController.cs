using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static bool UIActive;
    [SerializeField] private bool uiStartActive;
    [SerializeField] private bool canPauseGame;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Sprite startMenuButtonImage;
    [SerializeField] private Sprite startMenuButtonHighlightImage;
    [SerializeField] private Sprite closeButtonImage;
    [SerializeField] private Sprite closeButtonHighlightImage;
    [SerializeField] private GameObject firstSelectedObj;
    [SerializeField] private GameObject[] selectables;
    [SerializeField] private GameObject[] closeBtnSelectables;
    [SerializeField] private TMP_Dropdown dropdownResolutions;
    [SerializeField] private TMP_Dropdown dropdownFullScreen;

//    private Animator _animator;
//    private bool _isCreditsOpen;

    private EventSystem _eventSystem;
    private Resolution[] _resolutions;
    private int _currentResolution;
    private FullScreenMode[] _fullScreenModes;
    private int _currentFullScreenMode;
    
    private void Start()
    {
        UIActive = uiStartActive;
        _eventSystem = EventSystem.current;
        _eventSystem.SetSelectedGameObject(firstSelectedObj);

//        _animator = GetComponent<Animator>();

        if (dropdownResolutions != null) SetResolutions();
        if (dropdownFullScreen != null) SetFullScreenModes();
    }

    private void Update()
    {
        if (UIActive)
        {
            if (_eventSystem.currentSelectedGameObject == null &&
                Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            {
                _eventSystem.SetSelectedGameObject(firstSelectedObj);
            }

            foreach (GameObject selectableObj in selectables)
            {
                SelectDeselectButton(selectableObj);
            }
            foreach (GameObject selectableObj in closeBtnSelectables)
            {
                SelectDeselectCloseButton(selectableObj);
            }
            
            if (canPauseGame && Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetResolutions();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        UIActive = true;
        if (pauseMenu != null)
        {
            pauseMenu.GetComponent<Animator>().SetTrigger("show");
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        UIActive = false;
        if (pauseMenu != null)
        {
            pauseMenu.GetComponent<Animator>().SetTrigger("hide");
        }
    }

    private void SelectDeselectButton(GameObject selectableObj)
    {
        if (_eventSystem.currentSelectedGameObject == selectableObj)
        {
            SetSelectableMenuSprite(selectableObj, startMenuButtonHighlightImage);
        }
        else
        {
            SetSelectableMenuSprite(selectableObj, startMenuButtonImage);
        }
    }
    
    private void SelectDeselectCloseButton(GameObject selectableObj)
    {
        if (_eventSystem.currentSelectedGameObject == selectableObj)
        {
            SetSelectableMenuSprite(selectableObj, closeButtonHighlightImage);
        }
        else
        {
            SetSelectableMenuSprite(selectableObj, closeButtonImage);
        }
    }

    public void HoverSelect(GameObject obj)
    {
        _eventSystem.SetSelectedGameObject(obj);
    }

    private void SetSelectableMenuSprite(GameObject selectableObj, Sprite sprite)
    {
        Image image = selectableObj.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
        }
    }

    public void SetFirstSelectableObject(GameObject selectableObj)
    {
        firstSelectedObj = selectableObj;
    }

    public void LoadScene(String sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
    
    
    //===== Options =====s
    public void ApplyOptions()
    {
        int resolutionSelection = dropdownResolutions.value;
        int fullScreenSelection = dropdownFullScreen.value;
        Screen.SetResolution(_resolutions[resolutionSelection].width,
            _resolutions[resolutionSelection].height,
            _fullScreenModes[fullScreenSelection],
            _resolutions[resolutionSelection].refreshRate);
    }
    
    private void SetResolutions()
    {
        _resolutions = Screen.resolutions;
        Array.Reverse(_resolutions);
        _currentResolution = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (Screen.width == _resolutions[i].width &&
                Screen.height == _resolutions[i].height &&
                (Screen.currentResolution.refreshRate == _resolutions[i].refreshRate || Screen.currentResolution.refreshRate == _resolutions[i].refreshRate + 1))
            {
                _currentResolution = i;
            }

            dropdownResolutions.options.Add(new TMP_Dropdown.OptionData(_resolutions[i].ToString()));
        }

        dropdownResolutions.value = _currentResolution;
    }

    private void SetFullScreenModes()
    {
        _fullScreenModes = new[]
        {
            FullScreenMode.Windowed,
            FullScreenMode.MaximizedWindow,
            FullScreenMode.ExclusiveFullScreen,
            FullScreenMode.FullScreenWindow
        };
        _currentFullScreenMode = 0;
        
        
        for (int i = 0; i < _fullScreenModes.Length; i++)
        {
            if (Screen.fullScreenMode.Equals(_fullScreenModes[i]))
            {
                _currentResolution = i;
            }

            dropdownFullScreen.options.Add(new TMP_Dropdown.OptionData(_fullScreenModes[i].ToString()));
        }

        dropdownFullScreen.value = _currentResolution;
    }

//    // TEST!
//    private int index = 0;
//    public TextMeshProUGUI tmp;
//    public void changeScreenResolution()
//    {
//        Debug.Log(index);
//        Resolution sr = Screen.resolutions[index];
//        tmp.text = sr.ToString();
//        Screen.SetResolution(sr.width, sr.height, Screen.fullScreenMode);
//        index++;
//        index %= Screen.resolutions.Length;
//    }
}
