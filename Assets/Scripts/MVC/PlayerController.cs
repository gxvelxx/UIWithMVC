using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerView playerView; // UI 패널이 어딘가 있다고 가정
    PlayerModel playerModel; // 플레이어이름 등등
    InputAction damageAction; // 예제를 위해 만들었을 뿐

    private void Awake()
    {
        playerModel = new PlayerModel();
        
        if (playerView == null)
        {
            playerView = Object.FindAnyObjectByType<PlayerView>();
        }

        damageAction = InputSystem.actions["Attack"];
    }

    private void Start()
    {
        playerModel.OnHealthChange += playerView.UpdateHealthUI; // 여기가 핵심코드! 연결을 담당하는 컨트롤러가 분리되었음
        playerView.UpdateHealthUI(playerModel.Health);
    }

    private void OnEnable()
    {
        damageAction.performed += OnDamageTriggered;
    }
    private void OnDisable()
    {
        damageAction.performed -= OnDamageTriggered;
    }

    void OnDamageTriggered(InputAction.CallbackContext ctx)
    {
        playerModel.TakeDamage(10);
    }
}
