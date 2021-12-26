using UnityEngine;

public class MeleeWeaponPickup : Interactable
{
    [SerializeField] private Sprite spriteToChange;

    private PlayerController _playerController;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    
    private void Start()
    {
        _playerController = GameObject.FindWithTag(TagsLayers.PlayerTag).GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    public override void Interact()
    {
        _spriteRenderer.sprite = spriteToChange;
        _playerController.playerInventory.AddWeapon(new MeleeWeapon());
        _collider.enabled = false;
    }
}
