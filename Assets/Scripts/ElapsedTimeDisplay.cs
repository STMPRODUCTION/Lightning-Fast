using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElapsedTimeDisplay : MonoBehaviour
{
    public TMP_Text elapsedTimeText; // Reference to the TMP text element
    public float elapsedTime;
    void Update()
    {
        if(elapsedTime>=0)
    {
        elapsedTime -= Time.deltaTime;
    }
        float milliseconds = elapsedTime; // Convert elapsed time to milliseconds
        elapsedTimeText.text = milliseconds.ToString("F2");// Display the elapsed time in milliseconds with two decimal places // Display the elapsed time in seconds
    }
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
    }
}
