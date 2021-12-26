using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagsLayers
{
    public static readonly string GroundWallTag = "GroundWall";
    public static readonly string PlayerTag = "Player";
    public static readonly string EnemyTag = "Enemy";
    public static readonly string InteractableTag = "Interactable";

    public static readonly LayerMask GroundWallLayerMask = LayerMask.GetMask("GroundWall");
}
