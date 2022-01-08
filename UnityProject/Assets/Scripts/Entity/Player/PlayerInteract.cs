using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private GameController _gameController;
    private PlayerController _playerController;
    private Interactable _interactable;

    private void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _playerController = GetComponent<PlayerController>();
    }
    
    private void Update()
    {
        if (_gameController.currentGameMode != GameMode.Gameplay && _playerController.isDead)
        {
            return;
        }
        
        if (_interactable != null && Input.GetKeyDown(KeyCode.E))
        {
            _interactable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TagsLayers.InteractableTag))
        {
            _interactable = other.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TagsLayers.InteractableTag))
        {
            _interactable = null;
        }
    }
}
