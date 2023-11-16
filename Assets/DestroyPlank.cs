using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlank : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("anchor"))
        {
            Destroy(gameObject);
        }
    }
}