using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DragRuller : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private InputDevice _device;
    private bool _isGrabbing = false;

    private float minRotationX = -45f;
    private float maxRotationX = 45f;

    void Awake()
    {
        // Get the XR grab interactable component from the game object
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    // This method is called when the ruler is grabbed by the interactor
    public void OnSelectEnter(XRBaseInteractor interactor)
    {
        // Get the input device from the interactor
        _device = interactor.GetComponent<XRController>().inputDevice;

        // Check if the user is holding the grab button
        if (_device.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue) && gripValue)
        {
            _isGrabbing = true;
        }
        else
        {
            _isGrabbing = false;
        }
    }

    // This method is called when the ruler is released by the interactor
    public void OnSelectExit(XRBaseInteractor interactor)
    {
        // Reset the grabbing state
        _isGrabbing = false;
    }

    void FixedUpdate()
    {
        // If the user is grabbing the ruler
        if (_isGrabbing)
        {
            // Get the interactor from the XRGrabInteractable component
            XRBaseInteractor interactor = _grabInteractable.selectingInteractor;

            if (interactor != null)
            {
                // Get the rotation input from the controller and apply it around the local x-axis
                float rotationInput = _device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 stickValue) ? stickValue.y : 0f;
                float newRotationX = Mathf.Clamp(transform.eulerAngles.x + rotationInput, minRotationX, maxRotationX);

                // Set the new x-axis rotation
                transform.rotation = Quaternion.Euler(newRotationX, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
    }
}