using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Text healthText;

    public void UpdateHealthUI(int currentHealth)
    {
        healthText.text = $"HP: {currentHealth}";
    }
}
