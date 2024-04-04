using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    public int playerID;
    [SerializeField]
    private int currentHealth;
    private int maxHealth=100;
    public Slider healthSlider;
    [SerializeField] private string name;
      void Awake()
    {
        // Assigning the Slider named "P1Health" from the Unity Inspector to the variable
        healthSlider = GameObject.Find(name).GetComponent<Slider>();

        if (healthSlider == null)
        {
            Debug.LogError("Slider named P1Health not found in the scene!");
        }
        else
        {
            Debug.Log("Slider P1Health assigned successfully!");
        }
    }
    private void Start() 
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    public void TakeDamage(int damage)
    {
        UpdateHealthUI();
        currentHealth-=damage;
    }
    void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }
    void OnWin(int playerID) // OnWin function with playerID argument
    {

        Debug.Log("Player " + playerID + " won!");
    }
}
