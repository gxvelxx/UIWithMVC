using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Text jumpText;

    public void UpdateHealthUI(int currentHealth)
    {
        healthText.text = $"HP: {currentHealth}";
    }

    public void UpdateJumpUI(int currentJump)
    {
        jumpText.text = $"Jump: {currentJump}";
    }
}
