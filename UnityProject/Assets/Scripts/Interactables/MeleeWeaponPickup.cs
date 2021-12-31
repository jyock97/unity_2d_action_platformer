using UnityEngine;

public class MeleeWeaponPickup : Interactable
{
    [SerializeField] private Sprite spriteToChange;

    private PlayerController _playerController;
    private MeleeWeapon _meleeWeapon;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    
    private void Start()
    {
        GameObject player = GameObject.FindWithTag(TagsLayers.PlayerTag);
        _playerController = player.GetComponent<PlayerController>();
        _meleeWeapon = player.GetComponent<MeleeWeapon>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    public override void Interact()
    {
        _spriteRenderer.sprite = spriteToChange;
        _collider.enabled = false;
        _playerController.playerInventory.AddWeapon(_meleeWeapon);
    }
}
