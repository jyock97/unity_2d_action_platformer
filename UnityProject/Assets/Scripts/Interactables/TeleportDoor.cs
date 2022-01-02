using System;
using UnityEngine;

public class TeleportDoor : Interactable
{
    [SerializeField] private String doorKeyName;
    [SerializeField] private GameObject linkedDoor;

    private Animator _animator;
    private TeleportDoor _linkedDoorTeleportDoor;
    private GameObject _player;
    private PlayerInventory _playerInventory;
    private bool _isOpen;

    private void OnValidate()
    {
        _linkedDoorTeleportDoor = linkedDoor.GetComponent<TeleportDoor>();
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _linkedDoorTeleportDoor = linkedDoor.GetComponent<TeleportDoor>();
        _player = GameObject.FindWithTag(TagsLayers.PlayerTag);
        _playerInventory = _player.GetComponent<PlayerController>().playerInventory;
    }
    
    public override void Interact()
    {
        if (!_isOpen && _playerInventory.ContainsDoorKey(doorKeyName))
        {
            Open();
            _linkedDoorTeleportDoor.Open();
        } 
        else if (_isOpen)
        {
            _player.transform.position = linkedDoor.transform.position;
        }
    }

    private void Open()
    {
        _isOpen = true;
        _animator.SetTrigger("open");
    }

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.5f;
        Gizmos.color = color;

        if (linkedDoor == _linkedDoorTeleportDoor.gameObject && _linkedDoorTeleportDoor.linkedDoor == gameObject)
        {
            Gizmos.DrawLine(transform.position, linkedDoor.transform.position);
        }
    }
}
