using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    public float rotationSpeed = 90f; // Degrees per second
    public Transform keyTransform; // Assign the key's transform here
    public float requiredTurnAngle = 90f; // The angle the key needs to be turned to unlock

    private bool keyInTrigger = false;
    private float initialKeyRotation;
    private bool isUnlocked = false;
    private bool shouldRotate = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter " + isUnlocked);
        if (other.transform == keyTransform)
        {
            keyInTrigger = true;
            initialKeyRotation = keyTransform.eulerAngles.z; // Assuming Z-axis is the turning axis
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit " + isUnlocked);
        if (other.transform == keyTransform)
        {
            keyInTrigger = false;
        }
    }

    void Update()
    {
        if (keyInTrigger && !isUnlocked)
        {
            float currentKeyRotation = keyTransform.eulerAngles.z;
            Debug.Log("cuurent rotation" + currentKeyRotation);
            if (Mathf.Abs(currentKeyRotation - initialKeyRotation) >= requiredTurnAngle)
            {
                shouldRotate = true;
            }
        }
        if (shouldRotate && transform.eulerAngles.z > 310 || transform.eulerAngles.z <= 0)
        {
            // Rotate the chest lid here as needed
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }
}

//using UnityEngine;

//public class RotateOnCollision : MonoBehaviour
//{
//    public float rotationSpeed = 90f;
//    public Vector3 rotationAxis = Vector3.up; // Currently not used, consider incorporating or removing
//    public GameObject keyObject; // Reference to the key GameObject

//    private bool isKeyInserted = false;
//    private Quaternion initialKeyRotation;
//    private float requiredRotation = 45f; // Amount of rotation required to open the chest

//    private void OnCollisionEnter(Collision collision)
//    {
//        Debug.Log("onCollisionEnter");
//        if (collision.gameObject.CompareTag("bottom_chest"))
//        {
//            isKeyInserted = true;
//            initialKeyRotation = keyObject.transform.rotation; // Store the initial rotation of the key
//            Debug.Log("Key inserted, initial rotation stored.");
//        }
//    }

//    void Update()
//    {
//        if (isKeyInserted)
//        {
//            // Calculate the angle difference from the initial rotation
//            float angleDifference = Quaternion.Angle(initialKeyRotation, keyObject.transform.rotation);

//            // Log the current angle difference for debugging
//            Debug.Log("Current Angle Difference: " + angleDifference);

//            // Check if the key has been rotated enough
//            if (angleDifference >= requiredRotation)
//            {
//                Debug.Log("Required rotation achieved, opening chest.");
//                ToggleChest();
//            }
//        }
//    }

//    void ToggleChest()
//    {
//        // Assuming we want to continuously open/close based on the rotation angle
//        float zRotation = transform.eulerAngles.z;

//        // Correcting the logic to continuously open or close the chest
//        if (zRotation > 310 || zRotation < 50) // Adjust 50 as per your closing angle
//        {
//            // Determine direction based on current rotation
//            float direction = zRotation > 310 ? -1 : 1;
//            transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
//            Debug.Log("Chest moving, current z rotation: " + transform.eulerAngles.z);
//        }
//    }
//}
