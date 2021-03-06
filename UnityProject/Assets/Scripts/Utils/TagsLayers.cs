using UnityEngine;

public static class TagsLayers
{
    public static readonly string GroundWallTag = "GroundWall";
    public static readonly string PlayerTag = "Player";
    public static readonly string EnemyTag = "Enemy";
    public static readonly string InteractableTag = "Interactable";
    public static readonly string BulletTag = "Bullet";
    public static readonly string BoxTag = "Box";
    public static readonly string HUDKeyInventoryTag = "HUDKeyInventory";
    public static readonly string BossShieldTag = "BossShield";
    

    public static readonly LayerMask GroundWallLayerMask = LayerMask.GetMask("GroundWall");
    public static readonly LayerMask PlayerLayerMask = LayerMask.GetMask("Player");
    public static readonly LayerMask EnemyLayerMask = LayerMask.GetMask("Enemy");
    public static readonly LayerMask InteractableLayerMask = LayerMask.GetMask("Interactable");
    public static readonly LayerMask BoxLayerMask = LayerMask.GetMask("Box");

    public static readonly int PlayerLayerMaskIndex = LayerMask.NameToLayer("Player");
    public static readonly int EnemyLayerMaskIndex = LayerMask.NameToLayer("Enemy");
}
