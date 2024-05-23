using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Adjust as needed
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float multyplyer = 10f;// Maximum speed the player can reach
    public bool canMove=true;

    private PlayerInput playerInput;
    public Vector2 moveInput;
    private Animator animator;
    private Rigidbody rb;
    public bool Ai =false;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the player.");
        }
    }

    public void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == playerInput.actions.FindAction("Move").name)
        {
            if(!Ai)
                moveInput = context.ReadValue<Vector2>();
            //animator.SetBool("IsMoving",true);
        }
    }
    private void FixedUpdate()
    {
        if (rb != null)
        {
            // Calculate the movement vector
            Vector3 movement = new Vector3(moveInput.x, 0f, 0f) * moveSpeed;

            // Apply movement using Rigidbody.AddForce
            rb.AddForce(movement * multyplyer);

            // Clamp the velocity to not exceed the maximum speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            // Update the animator speed parameter
            animator.SetFloat("speed", rb.velocity.magnitude);
            //Debug.Log(rb.velocity.magnitude);
        }
    }

    private void OnDestroy()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }
}
