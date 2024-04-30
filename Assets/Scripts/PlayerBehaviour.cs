using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Adjust as needed

    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Animator animator;
    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
        animator = GetComponent<Animator>();
    }

    public void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == playerInput.actions.FindAction("Move").name)
        {
            moveInput = context.ReadValue<Vector2>();
            //animator.SetBool("IsMoving",true);
        }
        else
        {
            //animator.SetBool("IsMoving",false);
        }
    }

    private void FixedUpdate()
    {
        // Move the player along the X axis
        Vector3 movement = new Vector3(0f, 0f, moveInput.x) * moveSpeed * Time.fixedDeltaTime;
        transform.Translate(movement);
        animator.SetFloat("speed",Mathf.Abs(moveInput.x)*10);
        Debug.Log(moveInput.x);
    }

    private void OnDestroy()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }
}

