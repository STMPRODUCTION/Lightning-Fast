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
    [SerializeField]
    private int maxHealth = 500;
    public Slider healthSlider;
    public GameObject WinScreen;
    public GameObject Win_Text_1;
    public GameObject Win_Text_2;
    [SerializeField] private string sliderName;
    [SerializeField] private string winScreenTag = "WinScreen";
    [SerializeField] private string p1WinsTag = "P2Wins";
    [SerializeField] private string p2WinsTag = "P1Wins";
    public List<GameObject> RoundPoints;
    void Awake()
    {
        // Assigning the Slider named from the Unity Inspector to the variable
        healthSlider = GameObject.Find(sliderName).GetComponent<Slider>();
        FindAndAddRoundPoints();
        if (healthSlider == null)
        {
            Debug.LogError("Slider named " + sliderName + " not found in the scene!");
        }
        else
        {
            Debug.Log("Slider " + sliderName + " assigned successfully!");
        }

        // Finding the WinScreen, Win_Text_1, and Win_Text_2 GameObjects by tag
        WinScreen = FindInactiveObjectByTag(winScreenTag);
        Win_Text_1 = FindInactiveObjectByTag(p1WinsTag);
        Win_Text_2 = FindInactiveObjectByTag(p2WinsTag);

        // Checking if the GameObjects are found
        if (WinScreen == null)
        {
            Debug.LogError("GameObject with tag 'WinScreen' not found in the scene!");
        }
        if (Win_Text_1 == null)
        {
            Debug.LogError("GameObject with tag 'P1Wins' not found in the scene!");
        }
        if (Win_Text_2 == null)
        {
            Debug.LogError("GameObject with tag 'P2Wins' not found in the scene!");
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void FindAndAddRoundPoints()
    {
        GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objs)
        {
            if (obj.CompareTag("rounds") && obj.hideFlags == HideFlags.None)
            {
                RoundPoints.Add(obj);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
           OnWin(playerID);
        }
    }

    void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }

    public void OnWin(int playerID)
    {
        Debug.Log("Player " + playerID + " won!");
        WinScreen.SetActive(true);

        // Activate the text based on the player ID
        if (playerID == 0)
        {
            Win_Text_2.SetActive(true);
        }
        else if (playerID == 1)
        {
            Win_Text_1.SetActive(true);
        }

    StartCoroutine(FadeTimeScale());
}

IEnumerator FadeTimeScale()
{
    float duration = 0.15f; // Duration of the fade
    float currentTime = 0f;

    while (currentTime < duration)
    {
        Time.timeScale = Mathf.Lerp(0.06f, 0f, currentTime / duration);
        currentTime += Time.deltaTime;
        yield return null;
    }

    Time.timeScale = 0f; // Ensure the time scale is set to 0 at the end
}
    // Helper function to find inactive objects by tag
    GameObject FindInactiveObjectByTag(string tag)
    {
        GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objs)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }
        return null;
        Debug.LogWarning("Objexct with tag: was now found " + tag);
    }
}
