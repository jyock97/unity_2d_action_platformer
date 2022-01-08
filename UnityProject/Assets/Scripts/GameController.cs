using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum GameMode
{
    Gameplay,
    CutScene,
    UI
}
public class GameController : MonoBehaviour
{
    public GameMode currentGameMode;

    private Dictionary<String, PlayableDirector> _cutScenes;

    private void Start()
    {
        _cutScenes = new Dictionary<string, PlayableDirector>();
        foreach (Transform cutScene in transform.Find("CutScenes"))
        {
            PlayableDirector cutSceneDirector = cutScene.GetComponent<PlayableDirector>();
            cutSceneDirector.stopped += (director) =>
            {
                currentGameMode = GameMode.Gameplay;
                Time.timeScale = 1;
            };
            _cutScenes.Add(cutScene.name, cutSceneDirector);
        }
    }

    public void CutScenePlay(string cutScene)
    {
        currentGameMode = GameMode.CutScene;
        Time.timeScale = 0;
        _cutScenes[cutScene]?.Play();
    }

    public void CutSceneAddStopCallback(string cutScene, Action<PlayableDirector> callback)
    {
        if (cutScene.Contains(cutScene))
        {
            _cutScenes[cutScene].stopped += callback;
        }
    }
    public void CutSceneRemoveStopCallback(string cutScene, Action<PlayableDirector> callback)
    {
        if (cutScene.Contains(cutScene))
        {
            _cutScenes[cutScene].stopped -= callback;
        }
    }
}
