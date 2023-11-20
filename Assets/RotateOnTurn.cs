using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnTurn : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public Vector3 rotationAxis = Vector3.up; // Currently not used, consider incorporating or removing
    public GameObject topChest; // Reference to the key GameObject

    private bool isKeyInserted = false;
    private Quaternion initialKeyRotation;
    private float requiredRotation = 45f; // Amount of rotation required to open the chest

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("onCollisionEnter");
        if (collision.gameObject.CompareTag("bottom_chest"))
        {
            isKeyInserted = true;
            initialKeyRotation = topChest.transform.rotation; // Store the initial rotation of the key
            Debug.Log("Key inserted, initial rotation stored.");
        }
    }

    void Update()
    {
        if (isKeyInserted)
        {
            ToggleChest();
            // Calculate the angle difference from the initial rotation
            //float angleDifference = Quaternion.Angle(initialKeyRotation, keyObject.transform.rotation);

            //// Log the current angle difference for debugging
            //Debug.Log("Current Angle Difference: " + angleDifference);

            //// Check if the key has been rotated enough
            //if (angleDifference >= requiredRotation)
            //{
            //    Debug.Log("Required rotation achieved, opening chest.");
            //    ToggleChest();
            //}
        }
    }

    void ToggleChest()
    {
        // Assuming we want to continuously open/close based on the rotation angle
        float zRotation = topChest.transform.eulerAngles.z;

        // Correcting the logic to continuously open or close the chest
        if (zRotation > 310 || zRotation < 50) // Adjust 50 as per your closing angle
        {
            // Determine direction based on current rotation
            float direction = zRotation > 310 ? -1 : 1;
            topChest.transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
            Debug.Log("Chest moving, current z rotation: " + topChest.transform.eulerAngles.z);
            isKeyInserted = false;
        }
    }
}
