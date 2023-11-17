using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnCollision : MonoBehaviour
{
    public float rotationSpeed = 90f; // Degrees per second
    public Vector3 rotationAxis = Vector3.up; // Axis of rotation

    private bool shouldRotate = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("chest_key"))
        {
            shouldRotate = true;
        }
    }

    void Update()
    {
        // Rotate the GameObject if shouldRotate is true and hasn't reached -50 degrees on Z-axis
        if (shouldRotate && transform.eulerAngles.z > 310 || transform.eulerAngles.z <= 0) // 310 degrees in Unity's rotation system is equivalent to -50 degrees
        {
            // Rotate around the Z-axis
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }
}
