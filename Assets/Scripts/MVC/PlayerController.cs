using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerView playerView; // UI 패널이 어딘가 있다고 가정
    PlayerModel playerModel; // 플레이어이름 등등

    InputAction damageAction;
    InputAction moveAction;
    InputAction jumpAction;

    Vector3 moveDirection;
    private float moveSpeed = 10f;
    private float jumpSpeed = 10f;

    Rigidbody rb;
    private bool isGround = true;

    private void Awake()
    {
        playerModel = new PlayerModel();
        
        if (playerView == null)
        {
            playerView = Object.FindAnyObjectByType<PlayerView>();
        }

        damageAction = InputSystem.actions["Attack"];
        moveAction = InputSystem.actions["Move"];
        jumpAction = InputSystem.actions["Jump"];
    }

    private void Start()
    {
        playerModel.OnHealthChange += playerView.UpdateHealthUI; // 여기가 핵심코드! 연결을 담당하는 컨트롤러가 분리되었음
        playerModel.OnjumpCountChange += playerView.UpdateJumpUI;
        playerView.UpdateHealthUI(playerModel.Health);
        playerView.UpdateJumpUI(playerModel.JumpCount);

        rb = GetComponent<Rigidbody>();

        //이동
        moveAction.performed += (ctx) =>
        {
            Vector2 direction = ctx.ReadValue<Vector2>();
            moveDirection = new Vector3(direction.x, 0, direction.y);
        };
        moveAction.canceled += (ctx) =>
        {
            moveDirection = Vector3.zero;
        };

        //점프
        jumpAction.performed += (ctx) => OnJump();
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
        jumpAction.Enable();
    }
    private void OnDisable()
    {
        damageAction.performed -= OnDamageTriggered;
        jumpAction.Disable();
    }

    void OnDamageTriggered(InputAction.CallbackContext ctx)
    {
        playerModel.TakeDamage(10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGround = true;
        }
    }

    private void OnJump()
    {
        if (isGround)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, rb.linearVelocity.z);
            isGround = false;

            playerModel.AddJumpCount();
        }    
    }
}
