using UnityEngine;

public class GlobalGizmosController : MonoBehaviour
{
    [SerializeField] private bool disableAll;
    [SerializeField] private bool enableAll;

    //------------Player Gizmos------------------
    public bool player;
    public static bool Player;
    
    // Melee Attack
    public bool meleeAttackAlways;
    public static bool MeleeAttackAlways;
    public bool meleeAttackOnHit;
    public static bool MeleeAttackOnHit;


    private void SyncValues()
    {
        Player = player;
        MeleeAttackAlways = meleeAttackAlways;
        MeleeAttackOnHit = meleeAttackOnHit;
    }
    
    private void SetAll(bool flag)
    {
        player = flag;
        meleeAttackAlways = flag;
        meleeAttackOnHit = flag;
    }
    
    private void OnValidate()
    {
        ProcessFlags();
    }

    private void Update()
    {
        ProcessFlags();
    }

    private void ProcessFlags()
    {
        if (disableAll)
        {
            SetAll(false);
            disableAll = false; // TODO make this Buttons with Custom Editor
        }

        if (enableAll)
        {
            SetAll(true);
            enableAll = false; // TODO make this Buttons with Custom Editor
        }
        
        SyncValues();
    }
}
