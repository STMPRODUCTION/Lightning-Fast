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
    public Collider lightAttackCollider;
    // Attack damage for light and heavy attacks
    public int currentIndex = 0;
    public GameObject player;
    public GameObject gmg;
    private int cid;
    public float colorChangeDuration = 1f; // Duration for which the color will remain changed
    private Color originalColor;
    private Rigidbody playerRigidbody;
    public int playerID;
    private int dmg;
    private bool cantakedamage;
    [SerializeField]private int atID;
    private Animator animator;
    [Header("Events")]
    [Space]
    public UnityEvent OnJab;
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
    public void ExecuteAttack(int attackId)
    {
        if (disabledRows.Count == comboMatrix.GetLength(0))
        {
            disabledRows.Clear();
            currentIndex =0;
        }
        // Iterate through each row of the combo matrix
        for (int i = 0; i < comboMatrix.GetLength(0); i++)
        {
            if(disabledRows.Contains(i))
                continue;
            if(attackId != comboMatrix[i,comboIndex])
            {
                disabledRows.Add(i);
            }
        }
        if(disabledRows.Count == comboMatrix.GetLength(0))
        {
            comboIndex=0;
            disabledRows.Clear();
        }
        else
        {
            comboIndex++;
            for (int i = 0; i < comboMatrix.GetLength(0); i++)
            {

                if(disabledRows.Contains(i))
                    continue;
                if(comboMatrix[i,comboIndex]== -1)
                {
                    IsCombo =true;
                    ComboExecuted(i);
                    comboIndex=0;
                    disabledRows.Clear();
                }
            }
        }
        
    }
    // Method for handling light attack logic

    // Method called when attack animation ends
    public void OnAttackAnimationEnd()
    {
        // Deactivate attack colliders
        lightAttackCollider.enabled = false;
    }

    // Function to be triggered when a combo is executed
    private void ComboExecuted(int comboId)
    {
        cid=comboId;
        lightAttackCollider.enabled = true;
        if(comboId == 0)
        {
            animator.Play("=overhand");
            StartCoroutine(LeaveCombo(0.1f));
        }
        if(comboId == 1)
        {
           StartCoroutine(LeaveCombo(0.1f));
        }
        if(comboId == 2)
        {         
            StartCoroutine(LeaveCombo(0.1f));
        }
        if(comboId == 3)
        {
            StartCoroutine(LeaveCombo(0.1f));
        }
        if (playerRigidbody != null)
        {
             //playerRigidbody.AddForce(Vector2.up * (comboId + 1) * 5, ForceMode2D.Impulse);
        }
    
        lightAttackCollider.enabled = true;
        Invoke("DeactivateCollider", 1f);
    }
    // Method to deactivate the collider
    private void DeactivateCollider()
    {
        lightAttackCollider.enabled = false;
    }
    private void ChangePlayerColor(Color color)
    {
        Debug.Log("_");
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
                lightAttackCollider.enabled = true;
                Invoke("DeactivateCollider", 1f);
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
                lightAttackCollider.enabled = true;
                Invoke("DeactivateCollider", 0.2f);
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
                lightAttackCollider.enabled = true;
                Invoke("DeactivateCollider", 1f);
            }
        }
    }
    // Method for dealing damage to enemies
    private void OnTriggerEnter(Collider other)
    {
        // Check if collided with enemy and attacking
        if (other.CompareTag("Enemy") || other.CompareTag(layername))
        {
           
            if(atID ==1)
                healthController.TakeDamage(1);
            else if(atID ==2)
                healthController.TakeDamage(5);
            else if(atID ==3)
                healthController.TakeDamage(3);
            if(cid == 0 && atID ==2)
            {
                healthController.TakeDamage(7);
                cid=-1;
            }
             else if(cid == 1 && atID ==2)
            {   
                healthController.TakeDamage(5);
                cid=-1;
            }
            else if(cid == 2 && atID ==2)
            {
                healthController.TakeDamage(4);
                cid=-1;
            }
            else if(cid == 3 && atID ==3)
            {
                healthController.TakeDamage(20);
                cid=-1;
            }
            cantakedamage =true;

        }
        else{
            cantakedamage =false;
        }
    }
    private IEnumerator LeaveCombo(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsCombo =false;
    }

}