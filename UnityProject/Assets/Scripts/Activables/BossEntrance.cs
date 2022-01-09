using System;
using UnityEngine;
using UnityEngine.Playables;

public class BossEntrance : Activable
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Vector2 bossSpawnOffset;

    private readonly String _cutScene = "CutScene";
    
    private GameController _gameController;
    private Animator _animator;
    private bool _activated;
    
    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _animator = GetComponent<Animator>();
        
        _activated = false;
    }

    public override void Activate()
    {
        if (!_activated)
        {
            _activated = true;
            _gameController.CutSceneAddStopCallback(_cutScene, CutSceneCallback);
            _gameController.CutScenePlay(_cutScene);
        }
    }

    private void CutSceneCallback(PlayableDirector obj)
    {
        _gameController.CutSceneRemoveStopCallback(_cutScene, CutSceneCallback);
        _animator.SetTrigger("open");
        
        GameObject go = Instantiate(bossPrefab);
        Vector2 newPosition = transform.position;
        newPosition += bossSpawnOffset;
        go.transform.position = newPosition;
    }
}
