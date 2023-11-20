using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject door;
    public float openAngle = 90.0f; // Adjust the angle to how far you want the door to open
    public float openSpeed = 2.0f; // Adjust the speed of the door opening

    private bool isOpening = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        initialRotation = door.transform.rotation;
        targetRotation = Quaternion.Euler(0, openAngle, 0) * initialRotation;
    }

    // Function to open the door with animation
    public void Open()
    {
        if (!isOpening)
        {
            StartCoroutine(OpenDoorAnimation());
        }
    }

    private IEnumerator OpenDoorAnimation()
    {
        isOpening = true;
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            door.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * openSpeed;
            yield return null;
        }

        // Ensure the door is fully opened
        door.transform.rotation = targetRotation;

        isOpening = false;
    }
}
