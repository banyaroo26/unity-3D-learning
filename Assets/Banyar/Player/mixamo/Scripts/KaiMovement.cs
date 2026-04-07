using UnityEngine;
using UnityEngine.InputSystem;

namespace Banyar.Player {

    public class KaiMovement : MonoBehaviour
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

        [SerializeField] private Collider weaponCollider;

        // [Header("Player State")]
        // [SerializeField] private bool inCombat;

        void Awake() 
        {
            playerControls = new PlayerControls();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            // playerCamera = GetComponentInChildren<Camera>();

            moveAction = playerControls.Player.Move;
            sprintAction = playerControls.Player.Sprint;
            attackAction = playerControls.Player.Attack;
        }

        public void enableWeaponCollider() 
        {
            weaponCollider.enabled = true;
        }

        public void disableWeaponCollider() 
        {
            weaponCollider.enabled = false;
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

        bool canMove()
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsTag("Combat"))
            {
                if (stateInfo.shortNameHash == Animator.StringToHash("Attack1") ||
                    stateInfo.shortNameHash == Animator.StringToHash("Attack2") ||
                    stateInfo.shortNameHash == Animator.StringToHash("Attack3"))
                {
                    return stateInfo.normalizedTime >= 0.6f;
                }
                return false;
            } else {
                return true;
            }
        }

        void attackPlayer()
        {
            if (attackAction.WasPressedThisFrame())
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (stateInfo.IsTag("Locomotion"))
                {
                    // Example: Hit2 stronger damage
                    // Debug.Log("Hit2 active");
                    animator.SetTrigger("Attack1");
                }

                else if (stateInfo.shortNameHash == Animator.StringToHash("Attack1"))
                {
                    animator.SetTrigger("Attack2");
                }

                else if (stateInfo.shortNameHash == Animator.StringToHash("Attack2"))
                {
                    animator.SetTrigger("Attack3");
                }
            }
        }

        void movePlayer()
        {
            if (!canMove()) return;

            // Movement
            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 movement = new Vector3(input.x, 0, input.y);

            float inputMagnitude = Mathf.Clamp01(movement.magnitude); // (1, -1) magnitude is 1.41 so we clamp it to 1
            float speed = moveSpeed;

            // Sprint
            bool sprinting = sprintAction.IsPressed();

            if (movement != Vector3.zero)
            {
                animator.CrossFade("Base Layer.Movement Blend Tree", 0.05f);
            } 

            if (!sprinting)
            {
                inputMagnitude /= 2;
                speed = moveSpeed;
            } else {
                speed = sprintSpeed;
            }

            animator.SetFloat("Blend", inputMagnitude);

            // Rotate towards movement direction
            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            }

            characterController.Move(movement * Time.deltaTime * speed);
        }
    }
}