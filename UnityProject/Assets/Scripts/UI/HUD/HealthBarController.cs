using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    public void SetValue(float val)
    {
        healthBar.fillAmount = val;
    }
}