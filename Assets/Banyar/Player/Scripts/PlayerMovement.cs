using UnityEngine;
using UnityEngine.InputSystem;

namespace Banyar.Player {

    public class PlayerMovement : MonoBehaviour
    {
        private PlayerControls playerControls;

        [Header("Components")]
        private CharacterController characterController;
        private Animator animator;

        [Header("Movement")]
        [SerializeField] public float moveSpeed = 4f;
        [SerializeField] public float sprintSpeed = 6f;

        private InputAction moveAction;
        private InputAction sprintAction;
        private InputAction attackAction;

        [Header("Player State")]
        [SerializeField] private bool inCombat;

        void Awake() 
        {
            playerControls = new PlayerControls();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            // playerCamera = GetComponentInChildren<Camera>();

            moveAction = playerControls.Player.Move;
            sprintAction = playerControls.Player.Sprint;
            attackAction = playerControls.Player.Attack;

            inCombat = false;
        }

        void OnEnable() 
        {
            playerControls.Enable();
            moveAction.Enable();
            sprintAction.Enable();
            attackAction.Enable();
        }

        void OnDisable()
        {
            playerControls.Disable();
            moveAction.Disable();
            sprintAction.Disable();
            attackAction.Disable();
        }

        void Update()
        {
            movePlayer();
            attackPlayer();
        }

        bool isAttacking()
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(1);

            Debug.Log(state.IsName("OneHandedMelee"));

            return state.IsName("OneHandedMelee") && state.normalizedTime < 1f;
        }

        void attackPlayer()
        {
            if (attackAction.WasPressedThisFrame() && isAttacking())
            {
                animator.SetTrigger("Attack");
                animator.SetLayerWeight(1, 1f);
            }

            //Debug.Log("In Combat : " + inCombat);
        }

        void movePlayer()
        {
            // Movement
            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 movement = new Vector3(input.x, 0, input.y);

            //Debug.Log("Movement Magnitude : " + movement.magnitude);

            float inputMagnitude = Mathf.Clamp01(movement.magnitude); // (1, -1) magnitude is 1.41 so we clamp it to 1
            float speed = moveSpeed;

            //Debug.Log("Movement : " + movement);
            //Debug.Log("Input Magnitude : " + inputMagnitude);

            // Sprint
            bool sprinting = sprintAction.IsPressed();

            if (!sprinting)
            {
                inputMagnitude /= 2;
                speed = moveSpeed;
            } else {
                speed = sprintSpeed;
            }

            //Debug.Log("Sprinting : " + sprinting);
            //Debug.Log("Input Magnitude : " + inputMagnitude);

            animator.SetFloat("Blend", inputMagnitude);

            // Rotate towards movement direction
            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            }

            // velocity.y += gravity * Time.deltaTime;
            // CharacterController.Move(velocity * Time.deltaTime);

            characterController.Move(movement * Time.deltaTime * speed);
        }
    }
}


/*
public InputActionAsset inputActions;

    private InputAction moveAction;

    private CharacterController characterController;
    private Animator animator;

    void Awake()
    {
        var playerMap = inputActions.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f // rotation speed
            );
        }

        characterController.Move(move * Time.deltaTime * 5f);
    }
*/