using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRotation : XRBaseInteractable
{
    [Serializable]
    public class ValueChangeEvent : UnityEvent<float> { }

    [SerializeField] Transform m_Handle = null;
    [SerializeField] [Range(0.0f, 1.0f)] float m_Value = 0.5f;
    [SerializeField] bool m_ClampedMotion = true;
    [SerializeField] float m_MaxAngle = 90.0f;
    [SerializeField] float m_MinAngle = -90.0f;
    [SerializeField] float m_AngleIncrement = 0.0f;
    [SerializeField] float m_PositionTrackedRadius = 0.1f;
    [SerializeField] float m_TwistSensitivity = 1.5f;
    [SerializeField] ValueChangeEvent m_OnValueChange = new ValueChangeEvent();

    IXRSelectInteractor m_Interactor;

    /// <summary>
    /// The object that is visually grabbed and manipulated
    /// </summary>
    public Transform handle
    {
        get => m_Handle;
        set => m_Handle = value;
    }

    public float value
    {
        get => m_Value;
        set { 
            SetValue(value);
            SetHandleAngle(value);
        }
    }

    public bool clampedMotion
    {
        get => m_ClampedMotion;
        set => m_ClampedMotion = value;
    }

    public float maxAngle
    {
        get => m_MaxAngle;
        set => m_MaxAngle = value;
    }

    public float minAngle
    {
        get => m_MinAngle;
        set => m_MinAngle = value;
    }


    private void Start()
    {
        SetValue(m_Value);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(StartGrab);
        selectExited.AddListener(EndGrab);
    }

    protected override void OnDisable()
    {
        selectEntered.RemoveListener(StartGrab);
        selectExited.RemoveListener(EndGrab);
        base.OnDisable();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                UpdateValue();
            }
        }
    }

    Vector3 GetLookDirection()
    {
        Vector3 direction = m_Interactor.GetAttachTransform(this).position - m_Handle.position;
        direction = transform.InverseTransformDirection(direction);
        direction.x = 0;

        return direction.normalized;
    }

    void UpdateValue()
    {
        var lookDirection = GetLookDirection();
        var lookAngle = Mathf.Atan2(lookDirection.z, lookDirection.y) * Mathf.Rad2Deg;

        if (m_MinAngle < m_MaxAngle)
            lookAngle = Mathf.Clamp(lookAngle, m_MinAngle, m_MaxAngle);
        else
            lookAngle = Mathf.Clamp(lookAngle, m_MaxAngle, m_MinAngle);

        var maxAngleDistance = Mathf.Abs(m_MaxAngle - lookAngle);
        var minAngleDistance = Mathf.Abs(m_MinAngle - lookAngle);



        var normalizedMaxDistance = maxAngleDistance / (m_MaxAngle - m_MinAngle);
        var normalizedMinDistance = minAngleDistance / (m_MaxAngle - m_MinAngle);

        var newValue = normalizedMinDistance / (normalizedMinDistance + normalizedMaxDistance);

        SetHandleAngle(lookAngle);

        SetValue(newValue);
    }

    void SetHandleAngle(float angle)
    {
        if (m_Handle != null)
            m_Handle.localRotation = Quaternion.Euler(angle, 0.0f, 0.0f);
    }


    void StartGrab(SelectEnterEventArgs args)
    {
        m_Interactor = args.interactorObject;
    }

    void EndGrab(SelectExitEventArgs args)
    {
        SetValue(m_Value);
        m_Interactor = null;
    }

    void OnDrawGizmosSelected()
    {
        var angleStartPoint = transform.position;

        if (m_Handle != null)
            angleStartPoint = m_Handle.position;

        const float k_AngleLength = 0.25f;

        var angleMaxPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MaxAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;
        var angleMinPoint = angleStartPoint + transform.TransformDirection(Quaternion.Euler(m_MinAngle, 0.0f, 0.0f) * Vector3.up) * k_AngleLength;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(angleStartPoint, angleMaxPoint);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(angleStartPoint, angleMinPoint);
    }

    void SetValue(float value)
    {
        if (m_ClampedMotion)
            value = Mathf.Clamp01(value);

        if (m_AngleIncrement > 0)
        {
            var angleRange = m_MaxAngle - m_MinAngle;
            var angle = Mathf.Lerp(0.0f, angleRange, value);
            angle = Mathf.Round(angle / m_AngleIncrement) * m_AngleIncrement;
            value = Mathf.InverseLerp(0.0f, angleRange, angle);
        }

        m_Value = value;
        m_OnValueChange.Invoke(m_Value);
    }
}
