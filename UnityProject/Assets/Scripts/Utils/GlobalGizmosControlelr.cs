using UnityEngine;

public class GlobalGizmosControlelr : MonoBehaviour
{
    [SerializeField] private bool disableAll;
    [SerializeField] private bool enableAll;

    public bool player;
    public static bool Player;
    public bool test;
    public static bool Test;

    private void SyncValues()
    {
        Player = player;
        Test = test;
    }
    
    private void SetAll(bool flag)
    {
        player = flag;
        test = flag;
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
            disableAll = false;
        }

        if (enableAll)
        {
            SetAll(true);
            enableAll = false;
        }
        
        SyncValues();
    }
}
