using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{

    [SerializeField] private Image healthBar;

    private void Start()
    {
        GetComponentInParent<PlayerStats>().OnHealthPercentChange += UpdateHealthBar;
    }

    private void UpdateHealthBar(float healthPct)
    {
        healthBar.fillAmount = healthPct;
    }
}
