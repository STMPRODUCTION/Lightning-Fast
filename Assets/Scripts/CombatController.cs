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
    [SerializeField]
    private int AttackDelayMultiplier;
    public GameObject player;
    private int parry;
    [SerializeField]
    private bool legattacks;
    private int cid;
    
    private Rigidbody playerRigidbody;
    private int dmg;
    private bool cantakedamage;
    [SerializeField]
    private int fist_rightness;
    [SerializeField]private int atID;
    private Animator animator;
    private bool isBlocking;
    public float upforce=20;
    public float superman_punch_right=2;
    [SerializeField]
    private bool IsCombo =false;
    private bool canAttack =true;
    private int combo_Index_check=0;//checks if all the attacks landed;
    // Combo matrix where each row represents a combo sequence
    public int[,] comboMatrix = new int[,]
    {
        { 3, 2, 2,-1,-1,-1,-1,-1,},
        { 1, 3, 2,-1,-1,-1,-1,-1,},
        { 1, 1, 2,-1,-1,-1,-1,-1,},
        { 2, 2, 1, 3, 3, 2, 3,-1,},
        { 2, 4, 6, 5,-1,-1,-1,-1,},
        { 2, 1, 5, 6,-1,-1,-1,-1,},
        { 1, 2,-1,-1,-1,-1,-1,-1,},
        { 4, 4, 5, 6,-1,-1,-1,-1,},


    };
    private HealthController healthController;
    private SoundManager SM;
    private CombatController cc;

    public string layername; // Layer name to search for
    // Index to keep track of the current combo sequence
    private int comboIndex = 0;
    private List<int> disabledRows = new List<int>();
    private void Start()
    {
         animator = GetComponent<Animator>();
         SM = GetComponent<SoundManager>();
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
public float lastAttackTime;
// Variable to store the time of the last attack
    
    // Function to be triggered when a combo is executed
    private void ComboExecuted(int comboId)
    {
        cid = comboId;
        switch (comboId)
        {
            case 0:
                // Trigger the "Overhand" animation
                animator.SetTrigger("overhand");
                ActivateColliderWithDelay(2,0.2f,0.06f);
                StartCoroutine(LeaveCombo(0.2f));
                break;
            case 1:
                // Trigger the "SpinningBackFist" animation
                animator.SetTrigger("spin");
                ActivateColliderWithDelay(0,0.25f,0.06f);
                playerRigidbody.AddForce(Vector3.right * fist_rightness, ForceMode.Impulse);
                StartCoroutine(LeaveCombo(0.2f));
                break;
            case 2:
                // Trigger the "Chop" animation
                animator.SetTrigger("chop");
                ActivateColliderWithDelay(0,0.25f,0.06f);
                playerRigidbody.AddForce(Vector3.right * fist_rightness, ForceMode.Impulse);
                StartCoroutine(LeaveCombo(0.2f));
                break;
            case 3:
                // Trigger the "Superman" animation
                animator.SetTrigger("superman");
                playerRigidbody.AddForce(Vector3.up * upforce, ForceMode.Impulse);
                playerRigidbody.AddForce(Vector3.right * superman_punch_right, ForceMode.Impulse);
                StartCoroutine(LeaveCombo(0.5f));
                ActivateColliderWithDelay(0,1f,0.1f);
                break;
            case 4:
                // Trigger the "Superman" animation
                animator.SetTrigger("webster");
                playerRigidbody.AddForce(Vector3.up * upforce/2, ForceMode.Impulse);
                playerRigidbody.AddForce(Vector3.right * superman_punch_right/2, ForceMode.Impulse);
                StartCoroutine(LeaveCombo(1f));
                ActivateColliderWithDelay(4,0.5f,0.1f);
                break;
            case 5:
                // Trigger the "Superman" animation
                animator.SetTrigger("tornado");
                playerRigidbody.AddForce(Vector3.up * upforce/2, ForceMode.Impulse);
                playerRigidbody.AddForce(Vector3.right * superman_punch_right/2, ForceMode.Impulse);
                StartCoroutine(LeaveCombo(1f));
                ActivateColliderWithDelay(4,0.5f,0.1f);
                break;
            case 6:
                // Trigger the "Superman" animation
                animator.SetTrigger("cot");
                StartCoroutine(LeaveCombo(0.3f));
                ActivateColliderWithDelay(0,0.08f,0.07f);
                break;
            case 7:
                // Trigger the "Superman" animation
                animator.SetTrigger("jack");
                StartCoroutine(LeaveCombo(1.5f));
                ActivateColliderWithDelay(4,0.7f,0.2f);
                break;
        }
    }
     public void Legs(InputAction.CallbackContext context)//checks if the player goes to the leg attacks, if so all attaks become leg attacks
     {
        if (context.started)
        {
            legattacks=true;
        }
        else if(context.performed)
        {
            legattacks =true; 
        }
        else if(context.canceled)
        {
            legattacks =false; 
        }
     }
    public void Jab(InputAction.CallbackContext context)

    {
        if (context.started)
        {
            if(legattacks)
            {
                Push_kick();
            }
            else if(!IsCombo && canAttack)
            {
                dmg = 1;
                atID=1;
                ExecuteAttack(1);
                animator.Play("jab");
                ActivateColliderWithDelay(0,0.01f,0.01f);
                SM.PlaySound(0);
            }
        }

    }

    public void AI_Jab()
    {
        
            if(!IsCombo && canAttack)
            {
                dmg = 1;
                atID=1;
                ExecuteAttack(1);
                animator.Play("jab");
                ActivateColliderWithDelay(0,0.01f,0.01f);
                SM.PlaySound(0);
            }
    }
    public void Right_Hook(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(legattacks)
            {
                Side_kick();
            }
            else if(!IsCombo && canAttack)
            {
                atID=2;
                dmg = 5;
                ExecuteAttack(2);
                SM.PlaySound(1);
                animator.Play("hook");
                ActivateColliderWithDelay(1,0.1f,0.04f);
            }
        }
    }
    public void AI_Right_Hook()
    {
            if(!IsCombo && canAttack)
            {
                atID=2;
                dmg = 5;
                ExecuteAttack(2);
                SM.PlaySound(1);
                animator.Play("hook");
                ActivateColliderWithDelay(1,0.1f,0.04f);
            }
    } 
    public void Uppercut(InputAction.CallbackContext context)
    {
        if (context.started)
        {
             if(legattacks)
            {
                Roundhouse();
            }
            else if(!IsCombo  && canAttack == true)
            {
                ExecuteAttack(3);
                SM.PlaySound(2);
                dmg = 3;
                atID=3;
                animator.Play("uppercut");
                ActivateColliderWithDelay(2,0.1f,0.06f);
            }
        }
    }
    public void AI_Uppercut()
    {
            if(!IsCombo  && canAttack == true)
            {
                ExecuteAttack(3);
                SM.PlaySound(2);
                dmg = 3;
                atID=3;
                animator.Play("uppercut");
                ActivateColliderWithDelay(2,0.1f,0.06f);
                Debug.Log($"coud attack :" + canAttack);
            }
            else
            {
                Debug.Log($"doudn't attack" + canAttack);
            }
    }
    public void Push_kick()
    {
        
            if(!IsCombo && canAttack)
            {
                dmg = 7;
                atID=4;
                ExecuteAttack(4);
                animator.Play("push_kick");
                ActivateColliderWithDelay(3,0.2f,0.1f);
                SM.PlaySound(0);
                StartCoroutine(LeaveCombo(0.6f));
            }

    }
    public void Side_kick()
    {

            if(!IsCombo && canAttack)
            {
                dmg = 11;
                atID=5;
                ExecuteAttack(5);
                animator.Play("side_kick");
                ActivateColliderWithDelay(3,0.3f,0.1f);
                SM.PlaySound(1);
                StartCoroutine(LeaveCombo(1f));
            }

    }
    public void Roundhouse()
    {
        
            if(!IsCombo && canAttack)
            {
                dmg = 8;
                atID=6;
                ExecuteAttack(6);
                animator.Play("roundhouse");
                ActivateColliderWithDelay(4,0.25f,0.1f);
                SM.PlaySound(2);
                StartCoroutine(LeaveCombo(0.7f));
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
    [SerializeField]
    private bool IsDoging=false;
    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCoroutine(DogingTime());
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
    public void AI_Dodge()
    {
            StartCoroutine(DogingTime());
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
                StartCoroutine(Set_Parry(0.3f,2));
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
                StartCoroutine(Set_Parry(0.1f,1));
                animator.SetTrigger("p_jab");
            }
        }
    }
    void On_parryed()
    {
        cc.ActivateColliderWithDelay(2,0.1f,0.06f);
        cc.healthController.TakeDamage(20);
        SM.PlaySound(3);
        animator.SetTrigger("hurt");
        cc.animator.Play("=overhand");
        StartCoroutine(LeaveCombo(1f));
        playerRigidbody.AddForce(Vector3.right * -1 * fist_rightness/3, ForceMode.Impulse);
    }
    void On_hurt( int damage , float attakDelay , int coefciient)
    {
        healthController.TakeDamage(damage);
        cc.animator.SetTrigger("hurt");
        SM.PlaySound(3);
        cc.StartCoroutine(AttackDelay(attakDelay));
        cc.playerRigidbody.AddForce(Vector3.right * fist_rightness/coefciient, ForceMode.Impulse);
        Debug.Log(combo_Index_check);
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
        else
        {
            // Logic for taking damage
            if (other.CompareTag("Enemy") || other.CompareTag(layername))
            {
                combo_Index_check+=1;
                if (atID == 1)
                {
                    if(cc.parry != 1 && cc.IsDoging==false)
                    {
                        On_hurt(1,0.3f,4);
                    }
                    else if (cc.parry==1)
                    {
                        On_parryed();
                    }
                }
                else if (atID == 2)
                {
                    if(cc.parry != 2 && cc.IsDoging==false)
                        {
                            On_hurt(5,0.3f,4);
                        }
                    else if (cc.parry==2)
                    {
                        On_parryed();
                    }
                }
                else if (atID == 3)
                {   
                    if(cc.parry != 3  && cc.IsDoging==false)
                        {
                        On_hurt(3,0.3f,4);
                        }
                    else if (cc.parry==3)
                    {
                        On_parryed();
                    }
                    if(IsDoging)
                    {
                        Debug.Log($"doged");
                    }

                }
                else if (atID == 4)
                {   
                    if(cc.parry != 1)
                        {
                        On_hurt(8,0.8f,4);
                        }
                    else if (cc.parry==1)
                    {
                        On_parryed();
                    }

                }
                else if (atID == 5)
                {   
                    if(cc.parry != 2)
                        {
                        On_hurt(11,1f,4);
                        }
                    else if (cc.parry==2)
                    {
                         On_parryed();
                    }

                }
                else if (atID == 6)
                {   
                    if(cc.parry != 3 && cc.IsDoging==false)
                        {
                        On_hurt(8,1f,6);
                        }
                    else if (cc.parry==3)
                    {
                        On_parryed();
                    }

                }

                if (cid == 0 && atID == 2)
                {
                    if(cc.parry != 2  && cc.IsDoging==false)
                        {
                        On_hurt(7,0.6f,1);
                        }
                    else if (cc.parry==2)
                    {
                        On_parryed();
                    }
                    cid = -1;
                }
                else if (cid == 1 && atID == 2)
                {
                    if(cc.parry != 2  && cc.IsDoging==false)
                       {
                        On_hurt(5,0.6f,1);
                        }
                    else if (cc.parry==2)
                    {
                        On_parryed();
                    }
                    cid = -1;
                }
                else if (cid == 2 && atID == 2)
                {
                    if(cc.parry != 2  && !IsDoging)
                        
                    {
                        On_hurt(5,0.5f,2);
                    }
                    else if (cc.parry==2)
                    {
                        On_parryed();
                    }
                    cid = -1;
                }
                else if (cid == 3 && atID == 3 )
                {
                    if(cc.parry != 3  && !IsDoging)
                        {
                        On_hurt(50,1f,10);
                        cc.playerRigidbody.AddForce(Vector3.right * fist_rightness*4, ForceMode.Impulse);
                        }
                    else if (cc.parry==3)
                    {
                        On_parryed();
                    }
                    cid = -1;
                }
                else if (cid == 4 && atID == 6 )
                {
                    if(cc.parry != 3  && !IsDoging)
                        {
                        On_hurt(20,2f,5);
                        cc.playerRigidbody.AddForce(Vector3.right * fist_rightness*3, ForceMode.Impulse);
                        }
                    else if (cc.parry==3)
                    {
                        On_parryed();
                    }
                    cid = -1;
                }
                else if (cid == 5 && atID == 6 )
                {
                    if(cc.parry != 3  && !IsDoging)
                        {
                        On_hurt(15,1.5f,5);
                        cc.playerRigidbody.AddForce(Vector3.right * fist_rightness*2, ForceMode.Impulse);
                        }
                    else if (cc.parry==3)
                    {
                        On_parryed();
                    }
                    cid = -1;
                }
                else if (cid == 6 && atID == 2 )
                {
                    if(cc.parry != 3  && !IsDoging)
                    {
                        On_hurt(4,1.5f,10);
                    }
                    else if (cc.parry==3)
                    {
                        On_parryed();
                    }
                    cid = -1;
                }
                else if (cid == 7 && atID == 6 )
                {
                    if(cc.parry != 3  && !IsDoging)
                        {
                        On_hurt(20,1.5f,10);
                        }
                    else if (cc.parry==3)
                    {
                        On_parryed();
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

        public void ExecuteAttack(int attackId)
    {
        if(!isBlocking)
        {
            // Check if the time elapsed since the last attack exceeds 1 second
            if (Time.time - lastAttackTime > 1.5f)
            {
                comboIndex = 0; // Reset combo index
                disabledRows.Clear();
                combo_Index_check=0; // Clear disabled rows
            }

            // Update the time of the last attack
            lastAttackTime = Time.time;

            // If all rows are disabled, reset combo index and disabled rows
            if (disabledRows.Count == comboMatrix.GetLength(0))
            {
                comboIndex = 0;
                disabledRows.Clear();
                combo_Index_check=0;
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
                    combo_Index_check=0;
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
                            combo_Index_check=0;
                            disabledRows.Clear();
                        }
                    }
                }
            }
        }
        Debug.Log("CIndex: "+ comboIndex + "Ccheck: " + combo_Index_check);
    }
    private IEnumerator ActivateColliderCoroutine(int colliderIndex, float activateDelay, float deactivateDelay)
    {
        canAttack =false;
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
        canAttack=true;
    }

    private IEnumerator LeaveCombo(float delay)//how much the playr cannot attack for
    {
        IsCombo =true;
        yield return new WaitForSeconds(delay);
        IsCombo =false;
    }
    private IEnumerator AttackDelay(float delay) // how much the other player can not attack for
    {
        cc.canAttack =false;
        yield return new WaitForSeconds(delay*AttackDelayMultiplier);
        cc.canAttack =true;
    }    
    private IEnumerator Set_Parry(float waitTime, int parryID)
    {
        parry = parryID;
        yield return new WaitForSeconds(waitTime);
        parry = 0;
    }
    private IEnumerator DogingTime()
    {
        Debug.Log($"doging!");
        IsDoging=true;
        yield return new WaitForSeconds(0.6f);
        IsDoging=false;
    }
}