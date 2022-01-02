using System;
using UnityEngine;

public class Key : Interactable
{
    [SerializeField] private GameObject UIKeyHolder;
    [SerializeField] private String keyName;

    private PlayerInventory _playerInventory;
    private GameObject _keyInventory;


    private void Start()
    {
        _playerInventory = GameObject.FindWithTag(TagsLayers.PlayerTag)
            .GetComponent<PlayerController>().playerInventory;
        _keyInventory = GameObject.FindWithTag(TagsLayers.HUDKeyInventoryTag);
    }

    public override void Interact()
    {
        _playerInventory.AddDoorKey(keyName);
        GameObject go = Instantiate(UIKeyHolder, _keyInventory.transform, true);
        Destroy(gameObject);
    }
}
