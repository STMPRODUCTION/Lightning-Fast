using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameObjectActivator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects;  // List of GameObjects to be activated

    private int currentIndex = 1;  // Keeps track of the current GameObject index

    // Input action asset reference
    public InputActionReference nextLineAction;

    private void OnEnable()
    {
        // Enable the input action and register the callback
        nextLineAction.action.Enable();
        nextLineAction.action.performed += OnNextLinePerformed;
    }

    private void OnDisable()
    {
        // Disable the input action and unregister the callback
        nextLineAction.action.Disable();
        nextLineAction.action.performed -= OnNextLinePerformed;
    }

    private void OnNextLinePerformed(InputAction.CallbackContext context)
    {
        ActivateNext();
    }

    // Function to activate the next GameObject in the list
    public void ActivateNext()
    {
        // Check if the currentIndex is within the bounds of the list
        if (currentIndex < gameObjects.Count)
        {
            // Set all GameObjects to inactive
            foreach (var obj in gameObjects)
            {
                obj.SetActive(false);
            }
            if (currentIndex > 0)
            {
                gameObjects[currentIndex - 1].SetActive(false);
            }
            // Activate the current GameObject
            gameObjects[currentIndex].SetActive(true);
            
            // Move to the next index
            currentIndex++;
        }
        else
        {
            Debug.Log("All GameObjects have been activated.");
        }
    }
}
