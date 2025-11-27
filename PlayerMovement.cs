using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator; 
    AudioSource m_AudioSource;

    public InputAction MoveAction;
    public InputAction BoostAction;      // Input System action for Left Shift
    public PlayerBoost playerBoost;      // Drag PlayerBoost component here

    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        BoostAction.Enable();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // --- Handle Boost in Update() ---
        if (BoostAction != null && BoostAction.IsPressed() && playerBoost != null)
        {
            playerBoost.StartBoost();
        }
    }

    void FixedUpdate()
    {
        // --- Movement Input ---
        var pos = MoveAction.ReadValue<Vector2>();
        float horizontal = pos.x;
        float vertical = pos.y;
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
        m_Rigidbody.MoveRotation(m_Rotation);

        // --- Apply movement with boost multiplier ---
        float currentSpeed = walkSpeed;
        if (playerBoost != null)
            currentSpeed *= playerBoost.CurrentMultiplier();

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * currentSpeed * Time.deltaTime);

        // --- Audio ---
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
                m_AudioSource.Play();
        }
        else
        {
            m_AudioSource.Stop();
        }
    }
}
