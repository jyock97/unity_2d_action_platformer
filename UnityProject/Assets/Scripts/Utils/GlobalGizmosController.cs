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

    //-------------Enemy Gizmos------------------
    public bool enemies;
    public static bool Enemies;
    public bool allEnemyStates;
    public static bool AllEnemyStates;
    public bool enemyAttackAlways;
    public static bool EnemyAttackAlways;
    public bool enemyAttackOnHit;
    public static bool EnemyAttackOnHit;
    public bool bossFightArea;
    public static bool BossFightArea;
    
    //----------Interactable Gizmos--------------
    public bool linkedDoors;
    public static bool LinkedDoors;
    
    //------------Activable Gizmos---------------
    public bool pressurePad;
    public static bool PressurePad;

    private void SyncValues()
    {
        Player = player;
        MeleeAttackAlways = meleeAttackAlways;
        MeleeAttackOnHit = meleeAttackOnHit;
        Enemies = enemies;
        AllEnemyStates = allEnemyStates;
        EnemyAttackAlways = enemyAttackAlways;
        EnemyAttackOnHit = enemyAttackOnHit;
        BossFightArea = bossFightArea;
        LinkedDoors = linkedDoors;
        PressurePad = pressurePad;
    }
    
    private void SetAll(bool flag)
    {
        player = flag;
        meleeAttackAlways = flag;
        meleeAttackOnHit = flag;
        enemies = flag;
        allEnemyStates = flag;
        enemyAttackAlways = flag;
        enemyAttackOnHit = flag;
        bossFightArea = flag;
        linkedDoors = flag;
        pressurePad = flag;
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
