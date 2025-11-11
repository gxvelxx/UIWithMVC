using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerView playerView; // UI 패널이 어딘가 있다고 가정
    PlayerModel playerModel; // 플레이어이름 등등
    InputAction damageAction;

    InputAction moveAction;
    Vector3 moveDirection;
    private float moveSpeed = 10f;

    private void Awake()
    {
        playerModel = new PlayerModel();
        
        if (playerView == null)
        {
            playerView = Object.FindAnyObjectByType<PlayerView>();
        }

        damageAction = InputSystem.actions["Attack"];

        moveAction = InputSystem.actions["Move"];
    }

    private void Start()
    {
        playerModel.OnHealthChange += playerView.UpdateHealthUI; // 여기가 핵심코드! 연결을 담당하는 컨트롤러가 분리되었음
        playerView.UpdateHealthUI(playerModel.Health);

        moveAction.performed += (ctx) =>
        {
            Vector2 direction = ctx.ReadValue<Vector2>();
            moveDirection = new Vector3(direction.x, 0, direction.y);
        };

        moveAction.canceled += (ctx) =>
        {
            moveDirection = Vector3.zero;
        };
    }

    private void Update()
    {
        if (moveDirection != Vector3.zero)
        {            
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
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
