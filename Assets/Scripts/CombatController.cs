using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;


public class CombatController : MonoBehaviour
{
    // Reference to the player's attack colliders for light and heavy attacks
    public List<Collider> attackColliders;
    // Attack damage for light and heavy attacks
    public int currentIndex = 0;
    public GameObject player;
    private int parry;
    private int cid;
    public float colorChangeDuration = 1f; // Duration for which the color will remain changed
    private Color originalColor;
    private Rigidbody playerRigidbody;
    public int playerID;
    private int dmg;
    private bool cantakedamage;
    [SerializeField]
    private int fist_rightness;
    [SerializeField]private int atID;
    private Animator animator;
    [Header("Events")]
    [Space]
    public UnityEvent OnJab;
    private bool isBlocking;
    public float upforce=20;
    public float superman_punch_right=2;
    private bool IsCombo =false;
    // Combo matrix where each row represents a combo sequence
    public int[,] comboMatrix = new int[,]
    {
        { 3, 2, 2,-1,-1,-1,-1,-1,},
        { 1, 3, 2,-1,-1,-1,-1,-1,},
        { 1, 1, 2,-1,-1,-1,-1,-1,},
        { 2, 2, 1, 3, 3, 2, 3, -1}

    };
    private HealthController healthController;
    private CombatController cc;

    public string layername; // Layer name to search for
    // Index to keep track of the current combo sequence
    private int comboIndex = 0;
    private List<int> disabledRows = new List<int>();
    private void Start()
    {
         animator = GetComponent<Animator>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        // Find all GameObjects with the specified tag
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(layername);

        // Check if any objects with the specified tag were found
        if (gameObjects.Length > 0)
        {
            // Get the first object with the specified tag
            GameObject objectWithTag = gameObjects[0];

            // Try to get the HealthController component from the object
            HealthController hc = objectWithTag.GetComponent<HealthController>();
            cc = objectWithTag.GetComponent<CombatController>();

            // If the HealthController component is found, assign it
            if (hc != null)
            {
                healthController = hc;
                Debug.Log("assigned");
            }
            else
            {
                Debug.LogError("No object with the specified layer name has the HealthController component.");
            }
        }
        else
        {
            Debug.LogError("No object with the specified tag was found.");
        }
    }
public float lastAttackTime; // Variable to store the time of the last attack

    public void ExecuteAttack(int attackId)
    {
        if(!isBlocking)
        {
            // Check if the time elapsed since the last attack exceeds 1 second
            if (Time.time - lastAttackTime > 0.5f)
            {
                comboIndex = 0; // Reset combo index
                disabledRows.Clear(); // Clear disabled rows
            }

            // Update the time of the last attack
            lastAttackTime = Time.time;

            // If all rows are disabled, reset combo index and disabled rows
            if (disabledRows.Count == comboMatrix.GetLength(0))
            {
                comboIndex = 0;
                disabledRows.Clear();
            }
            else
            {
                // Iterate through each row of the combo matrix
                for (int i = 0; i < comboMatrix.GetLength(0); i++)
                {
                    if (disabledRows.Contains(i))
                        continue;

                    // Check if the current attack matches the combo at the current index
                    if (attackId != comboMatrix[i, comboIndex])
                    {
                        disabledRows.Add(i);
                    }
                }

                // If all rows are disabled for this combo index, reset combo index and disabled rows
                if (disabledRows.Count == comboMatrix.GetLength(0))
                {
                    comboIndex = 0;
                    disabledRows.Clear();
                }
                else
                {
                    // Advance combo index and check if any combo sequence is complete
                    comboIndex++;
                    for (int i = 0; i < comboMatrix.GetLength(0); i++)
                    {
                        if (disabledRows.Contains(i))
                            continue;

                        if (comboMatrix[i, comboIndex] == -1)
                        {
                            IsCombo = true;
                            ComboExecuted(i);
                            comboIndex = 0;
                            disabledRows.Clear();
                        }
                    }
                }
            }
        }
    }
    
    // Function to be triggered when a combo is executed
    private void ComboExecuted(int comboId)
{
    cid = comboId;
    switch (comboId)
    {
        case 0:
            // Trigger the "Overhand" animation
            animator.SetTrigger("overhand");
            ActivateColliderWithDelay(2,0.1f,0.06f);
            StartCoroutine(LeaveCombo(0.15f));
            break;
        case 1:
            // Trigger the "SpinningBackFist" animation
            animator.SetTrigger("spin");
            ActivateColliderWithDelay(0,0.1f,0.06f);
            playerRigidbody.AddForce(Vector3.right * fist_rightness, ForceMode.Impulse);
            StartCoroutine(LeaveCombo(0.15f));
            break;
        case 2:
            // Trigger the "Chop" animation
            animator.SetTrigger("chop");
            StartCoroutine(LeaveCombo(0.15f));
            break;
        case 3:
            // Trigger the "Superman" animation
            animator.SetTrigger("superman");
            playerRigidbody.AddForce(Vector3.up * upforce, ForceMode.Impulse);
            playerRigidbody.AddForce(Vector3.right * superman_punch_right, ForceMode.Impulse);
            StartCoroutine(LeaveCombo(0.3f));
            ActivateColliderWithDelay(0,0.7f,0.03f);
            break;
    }
}
    public void Jab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(!IsCombo)
            {
                dmg = 1;
                atID=1;
                ExecuteAttack(1);
                if(!IsCombo)
                {
                    animator.Play("jab");
                }
                ActivateColliderWithDelay(0,0,0.01f);
            }
        }

    }
    public void Right_Hook(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(!IsCombo)
            {
                atID=2;
                dmg = 5;
                ExecuteAttack(2);
                animator.Play("hook");
                ActivateColliderWithDelay(1,0.04f,0.04f);
            }
        }
    }
    public void Uppercut(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(!IsCombo)
            {
                ExecuteAttack(3);
                dmg = 3;
                atID=3;
                animator.Play("uppercut");
                ActivateColliderWithDelay(2,0.02f,0.06f);
            }
        }
    }
    public void Block(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!isBlocking)
            {
                isBlocking = true;
                animator.SetBool("block",true);
            }
        }
        if (context.performed)
        {
            isBlocking = false;
            animator.SetBool("block",false);
        }
    }
    private int dodgeCount = 0;
    private bool doge =true;
    private bool IsDoging=false;
    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(doge)
            {
                animator.SetTrigger("doge1");
                doge=false;
            }
            else if(!doge)
            {
                animator.SetTrigger("doge2");
                doge=true;
            }

        }
    }
    public void p_Uppercut(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(parry == 0)
                {
                    StartCoroutine(Set_Parry(0.5f,3));
                    animator.SetTrigger("p_uppercut");
                }
        }
    }
    public void p_Hook(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(parry == 0)
            {
                StartCoroutine(Set_Parry(0.5f,2));
                animator.SetTrigger("p_hook");
            }
        }
    }
    public void p_Jab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(parry == 0)
            {
                StartCoroutine(Set_Parry(0.2f,1));
                animator.SetTrigger("p_jab");
            }
        }
    }

    // Method for dealing damage to enemies
    private void OnTriggerEnter(Collider other)
    {
        if (isBlocking)
        {
            // Logic for blocking specific attacks
            if ((atID == 1 || atID == 3 || cid == 2 || cid == 3))
            {
                // Logic for blocking
                return;
            }
        }
        else if (IsDoging)
        {
            // Logic for dodging specific attacks
            if (( IsDoging && (atID == 1 || atID == 2 || cid == 0 || cid == 1 || cid == 3)) ||
                ( IsDoging && (atID == 1 || atID == 2 || cid == 0 || cid == 1 || cid == 3)))
            {
                // Logic for dodging
                return;
            }
        }
        else
        {
            // Logic for taking damage
            if (other.CompareTag("Enemy") || other.CompareTag(layername))
            {
                if (atID == 1)
                {
                    if(cc.parry != 1)
                        healthController.TakeDamage(1);
                    else if (cc.parry==1)
                    {
                        playerRigidbody.AddForce(Vector3.right * -1 * fist_rightness, ForceMode.Impulse);
                    }
                }
                else if (atID == 2)
                {
                    if(cc.parry != 2)
                        healthController.TakeDamage(5);
                    else if (cc.parry==2)
                    {
                        playerRigidbody.AddForce(Vector3.right * -1 * fist_rightness, ForceMode.Impulse);
                    }
                }
                else if (atID == 3)
                {   
                    if(cc.parry != 3)
                        healthController.TakeDamage(3);
                    else if (cc.parry==3)
                    {
                        playerRigidbody.AddForce(Vector3.right * -1 * fist_rightness, ForceMode.Impulse);
                    }

                }

                if (cid == 0 && atID == 2)
                {
                    if(cc.parry != 2)
                        healthController.TakeDamage(7);
                    else if (cc.parry==2)
                    {
                        playerRigidbody.AddForce(Vector3.right * -1 * fist_rightness, ForceMode.Impulse);
                    }
                    cid = -1;
                }
                else if (cid == 1 && atID == 2)
                {
                    if(cc.parry != 2)
                        healthController.TakeDamage(5);
                    else if (cc.parry==2)
                    {
                        playerRigidbody.AddForce(Vector3.right * -1 * fist_rightness, ForceMode.Impulse);
                    }
                    cid = -1;
                }
                else if (cid == 2 && atID == 2)
                {
                    if(cc.parry != 2)
                        healthController.TakeDamage(4);
                    else if (cc.parry==2)
                    {
                        playerRigidbody.AddForce(Vector3.right * -1 * fist_rightness, ForceMode.Impulse);
                    }
                    cid = -1;
                }
                else if (cid == 3 && atID == 3)
                {
                    if(cc.parry != 3)
                        healthController.TakeDamage(50);
                    else if (cc.parry==3)
                    {
                        playerRigidbody.AddForce(Vector3.right * -1 * fist_rightness, ForceMode.Impulse);
                    }
                    cid = -1;
                }
                cantakedamage = true;
                atID=0;
            }
            else
            {
                cantakedamage = false;
            }
        }
    }

    public void ActivateColliderWithDelay(int colliderIndex, float activateDelay, float deactivateDelay)
    {
        StartCoroutine(ActivateColliderCoroutine(colliderIndex, activateDelay, deactivateDelay));
    }

    private IEnumerator ActivateColliderCoroutine(int colliderIndex, float activateDelay, float deactivateDelay)
    {
        IsCombo =true;
        yield return new WaitForSeconds(activateDelay);

        // Activate the specific collider
        if (colliderIndex >= 0 && colliderIndex < attackColliders.Count)
        {
            attackColliders[colliderIndex].enabled = true;

        }

        yield return new WaitForSeconds(deactivateDelay);

        // Deactivate the specific collider
        if (colliderIndex >= 0 && colliderIndex < attackColliders.Count)
        {
            attackColliders[colliderIndex].enabled = false;
        }
        IsCombo=false;
    }

    private IEnumerator LeaveCombo(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsCombo =false;
    }    
    private IEnumerator Set_Parry(float waitTime, int parryID)
    {
        parry = parryID;
        yield return new WaitForSeconds(waitTime);
        parry = 0;
    }

}