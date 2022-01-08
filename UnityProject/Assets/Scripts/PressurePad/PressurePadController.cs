using UnityEngine;

public class PressurePadController : MonoBehaviour
{

    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Activable _activable;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TagsLayers.PlayerTag))
        {
            _spriteRenderer.sprite = pressedSprite;
            _activable.Activate();
        }
    }

    private void OnDrawGizmos()
    {
        if (GlobalGizmosController.PressurePad)
        {
            Color color = Color.green;
            color.a = 0.5f;
            Gizmos.color = color;
            
            Gizmos.DrawLine(transform.position, _activable.transform.position);
        }
    }
}
