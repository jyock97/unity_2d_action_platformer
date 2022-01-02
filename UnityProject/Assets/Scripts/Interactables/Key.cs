using System;
using UnityEngine;

public class Key : Interactable
{
    [SerializeField] private String keyName;
    
    private PlayerInventory _playerInventory;


    private void Start()
    {
        _playerInventory = GameObject.FindWithTag(TagsLayers.PlayerTag)
            .GetComponent<PlayerController>().playerInventory;
    }

    public override void Interact()
    {
        _playerInventory.AddDoorKey(keyName);
        Destroy(gameObject);
    }
}
