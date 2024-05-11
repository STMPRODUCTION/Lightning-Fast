using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public float minOrthographicSize = 8f; // Minimum orthographic size of the camera
    public float maxOrthographicSize = 250f; // Maximum orthographic size of the camera
    public float padding = 1f;
    public float minDistanceThreshold = 100f; // Additional padding around the objects
    public float smoothSpeed = 5f; // Speed of camera movement

    private Camera cam;
    private bool isCameraMovementNeeded = true; // Flag to track if camera movement is needed

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("p1");
        foreach (var player in players)
        {
            object1 = player;
            break; // Assuming there's only one object with tag "p1"
        }

        players = GameObject.FindGameObjectsWithTag("p2");
        foreach (var player in players)
        {
            object2 = player;
            break; // Assuming there's only one object with tag "p2"
        }
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Calculate the midpoint between object1 and object2 along the X-axis
        float midpointX = (object1.transform.position.x + object2.transform.position.x) / 2f;

        // Calculate the distance between object1 and object2
        float distance = Mathf.Abs(object1.transform.position.x - object2.transform.position.x);

        // If the distance is below the threshold and the camera is already at the minimum orthographic size,
        // no need to adjust the camera position

        // Calculate the distance between object1 and the midpoint
        float distance1 = Mathf.Abs(object1.transform.position.x - transform.position.x);

        // Calculate the distance between object2 and the midpoint
        float distance2 = Mathf.Abs(object2.transform.position.x - transform.position.x);

        // Determine the larger distance between the two objects
        float maxDistance = Mathf.Max(distance1, distance2);

        // Calculate the required orthographic size to fit the farthest object in the view
        float requiredSize = maxDistance + padding;

        // Clamp the required size within the specified range
        requiredSize = Mathf.Clamp(requiredSize, minOrthographicSize, maxOrthographicSize);

        // Smoothly move the camera position towards the target position
        Vector3 targetPosition = new Vector3(midpointX, transform.position.y, transform.position.z);
        if (isCameraMovementNeeded)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }

        // Set the orthographic size of the camera
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, requiredSize, smoothSpeed * Time.deltaTime);
        if (requiredSize == minOrthographicSize && maxDistance < minDistanceThreshold)
        {
            // If camera movement is not needed, exit the functio

            // Set the flag to indicate that camera movement is not needed
            isCameraMovementNeeded = false;
        }
        else
        {
            // Set the flag to indicate that camera movement is needed
            isCameraMovementNeeded = true;
        }
        Debug.Log("Required Size: " + requiredSize + ", Distance: " + distance +", maxDistance:"+ maxDistance + ", Min Distance Threshold: " + minDistanceThreshold+" _"+isCameraMovementNeeded);
    }
}
