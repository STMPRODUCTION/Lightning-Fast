using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    // Reference to the player's attack colliders for light and heavy attacks
    public Collider2D lightAttackCollider;
    public Collider2D heavyAttackCollider;

    // Attack damage for light and heavy attacks
    public int lightAttackDamage = 10;
    public int heavyAttackDamage = 20;
    public int currentIndex = 0;
    public GameObject player; // Reference to the GameObject of the player
    public float colorChangeDuration = 1f; // Duration for which the color will remain changed

    private Color originalColor;
    private Rigidbody2D playerRigidbody; // Store the original color of the player


    // Combo matrix where each row represents a combo sequence
    public int[,] comboMatrix = new int[,]
    {
        { 3, 2, 2,-1,-1,-1,-1,-1,},
        { 1, 3, 2,-1,-1,-1,-1,-1,},
        { 1, 1, 2,-1,-1,-1,-1,-1,},
        { 2, 2, 1, 3, 3, 2, 3, -1}

    };

    // Index to keep track of the current combo sequence
    private int comboIndex = 0;
    private List<int> disabledRows = new List<int>();

    

    private void Start()
    {
        originalColor = player.GetComponent<SpriteRenderer>().color;
        playerRigidbody = player.GetComponent<Rigidbody2D>();
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
                    Debug.Log($"bai sa fie");
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
        heavyAttackCollider.enabled = false;
    }

    // Function to be triggered when a combo is executed
    private void ComboExecuted(int comboId)
    {

        if (playerRigidbody != null)
        {
            // Apply a vertical force to make the player go up a little
             playerRigidbody.AddForce(Vector2.up * (comboId + 1) * 5, ForceMode2D.Impulse);
        }
    }

    private void ChangePlayerColor(Color color)
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;

            // Start a coroutine to revert the color change after the specified duration
            StartCoroutine(RevertColorAfterDelay(colorChangeDuration));
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found on player GameObject.");
        }
    }
    public void Jab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ExecuteAttack(1);
            ChangePlayerColor(Color.red); // 1 represents light attack in the combo matrix
        }

    }
    public void Right_Hook(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ExecuteAttack(2);
            ChangePlayerColor(Color.blue); // 1 represents light attack in the combo matrix
        }
    }
    public void Uppercut(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ExecuteAttack(3);
            ChangePlayerColor(Color.green); // 1 represents light attack in the combo matrix
        }
    }


    // Method for dealing damage to enemies
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if collided with enemy and attacking
        if (other.CompareTag("Enemy"))
        {
            int damage = 0;

            // Determine damage based on the attack type
            if (lightAttackCollider.enabled)
            {
                damage = lightAttackDamage;
            }
            else if (heavyAttackCollider.enabled)
            {
                damage = heavyAttackDamage;
            }

            // Deal damage to enemy
            if (damage > 0)
            {
                //EnemyController enemy = other.GetComponent<EnemyController>();
                /*if (enemy != null)
                {
                    //enemy.TakeDamage(damage);
                }*/
            }
        }
    }
        private IEnumerator RevertColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Revert the color change
        player.GetComponent<SpriteRenderer>().color = originalColor;
    }
}
