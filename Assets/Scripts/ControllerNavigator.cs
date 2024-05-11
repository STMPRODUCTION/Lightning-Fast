using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerNavigator : MonoBehaviour
{
    public float navigationThreshold = 0.5f; // Threshold for stick movement to register as navigation
    public float selectedScaleMultiplier = 1.4f; // Multiplier for selected UI scale

    public Selectable currentSelectable; // Currently selected UI element
    private bool isFindingSelectable = false; // Flag to control if script is finding a new selectable

    void Start()
    {
        // Set the first selectable UI element as the initial selection
        currentSelectable = EventSystem.current.firstSelectedGameObject.GetComponent<Selectable>();
        UpdateSelectedUI();
    }

    void Update()
    {
        if (!isFindingSelectable)
        {
            // Get movement input from the gamepad
            float verticalInput = Input.GetAxis("Vertical");

            // Check vertical movement
            if (Mathf.Abs(verticalInput) > navigationThreshold)
            {
                int direction = verticalInput > 0 ? 1 : -1;
                Selectable nextSelectable = GetNextSelectable(currentSelectable, direction);
                if (nextSelectable != null)
                {
                    currentSelectable = nextSelectable;
                    currentSelectable.Select();
                    UpdateSelectedUI();
                    Debug.Log("Selected: " + currentSelectable.gameObject.name); // Log the name of the selected GameObject
                    isFindingSelectable = true; // Start finding selectable
                }
            }
        }
        else
        {
            // Check if there's no longer any input
            if (Mathf.Abs(Input.GetAxis("Vertical")) < navigationThreshold)
            {
                isFindingSelectable = false; // Stop finding selectable
            }
        }
    }

    Selectable GetNextSelectable(Selectable current, int direction)
    {
        Selectable nextSelectable = null;
        float closestDistance = float.MaxValue;

        foreach (var selectable in FindObjectsOfType<Selectable>())
        {
            if (selectable != current)
            {
                RectTransform currentTransform = current.GetComponent<RectTransform>();
                RectTransform selectableTransform = selectable.GetComponent<RectTransform>();
                float distance = Mathf.Abs(selectableTransform.localPosition.y - currentTransform.localPosition.y);
                if (distance < closestDistance && (direction == 1 ? selectableTransform.localPosition.y > currentTransform.localPosition.y : selectableTransform.localPosition.y < currentTransform.localPosition.y))
                {
                    closestDistance = distance;
                    nextSelectable = selectable;
                }
            }
        }

        return nextSelectable;
    }

    void UpdateSelectedUI()
    {
        // Reset the scale of all UI elements
        foreach (var selectable in FindObjectsOfType<Selectable>())
        {
            selectable.transform.localScale = Vector3.one;
        }

        // Set the scale of the currently selected UI element
        RectTransform rectTransform = currentSelectable.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one * selectedScaleMultiplier;
    }
}
