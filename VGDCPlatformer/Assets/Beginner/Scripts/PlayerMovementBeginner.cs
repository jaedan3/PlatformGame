﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBeginner : MonoBehaviour
{

    public Rigidbody2D m_RigidBody2D;
    public Animator animator;

    [Header("Movement Logic")]
    //=========== Moving Logic ============
    private bool inInverse;
    private string horizontalInput;
    public float runSpeed = 0f;
    public float speedMultiplierDecayRate;
    public float speedMultiplier;
    private float horizontalMove = 0f;
    private Vector3 m_Velocity;
    private SpriteRenderer flip;
    private float currentVerticalMove = 0f;
    GameObject uiShit2;



    [Space]
    [Header("Jump Logic")]

    //============ Jump Logic ============
    public float m_JumpForce;
    private bool m_Grounded;
    private bool m_DoubleJump = false;
    private bool canDoubleJump = false;
    public Transform m_GroundCheck;
    public LayerMask m_GroundLayer;
    GameObject uiShit;
    GameObject uiShit3;
    GameObject uiShit4;


    // Use this for initialization
    void Start()
    {
        //m_RigidBody2D = GetComponent<Rigidbody2D>(); //Instead of manually putting the RigidBody2D Component we can get the component from the Object
        inInverse = false;
        horizontalInput = "Horizontal";
        speedMultiplierDecayRate = 0.99f;
        speedMultiplier = 1;
        m_Velocity = Vector3.zero; //same as new Vector3(0,0,0)
        flip = gameObject.GetComponent<SpriteRenderer>();

    }

    private void Awake()
    {
        uiShit2 = GameObject.Find("SpeedUI");
        uiShit = GameObject.Find("DoubleJumpUI");
        uiShit3 = GameObject.Find("IncreasedGravityUI");
        uiShit4 = GameObject.Find("DecreasedGravityUI");
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw(horizontalInput) * runSpeed * speedMultiplier;
        speedMultiplier = Mathf.Max(1, speedMultiplier * speedMultiplierDecayRate);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        currentVerticalMove = m_RigidBody2D.velocity.y;
        animator.SetFloat("VerticalSpeed", currentVerticalMove);
        if(currentVerticalMove != 0)
            Debug.Log(currentVerticalMove);

        if (m_RigidBody2D.gravityScale == 5)
        {
            uiShit3.SetActive(false);
            uiShit4.SetActive(false);
        }

        if (currentVerticalMove == 0 && canDoubleJump == false)
        {
            uiShit.SetActive(false);
        }

        if (m_Grounded && Input.GetButtonDown("Jump") && canDoubleJump == false)
        {
            uiShit.SetActive(false);
            m_Grounded = false;
            m_RigidBody2D.velocity = new Vector2(m_RigidBody2D.velocity.x, m_JumpForce);
            m_DoubleJump = false;
        }
        else if (m_Grounded && Input.GetButtonDown("Jump") && canDoubleJump)
        {
            m_DoubleJump = true;
            canDoubleJump = false;
            m_Grounded = false;
            m_RigidBody2D.velocity = new Vector2(m_RigidBody2D.velocity.x, m_JumpForce);
        }
        else if (m_DoubleJump && Input.GetButtonDown("Jump"))
        {
            m_DoubleJump = false;
            m_RigidBody2D.velocity = new Vector2(m_RigidBody2D.velocity.x, m_JumpForce);
            uiShit.SetActive(false);

        }
        if (speedMultiplier == 1)
        {
            uiShit2.SetActive(false);
        }
    }

    public bool getInverse()
    {
        return inInverse;
    }

    public void changeInverse(bool newInverse)
    {
        inInverse = newInverse;
    }

    public void changeHInput(string newInput)
    {
        horizontalInput = newInput;
    }
    //Changes Double Jump Value
    public void changeDoubleJump(bool newDoubleJump)
    {
        canDoubleJump = newDoubleJump;
    }

    // FixedUpdate is called multiple times per frame at different rates
    void FixedUpdate()
    {

        
        Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, m_RigidBody2D.velocity.y);
        m_RigidBody2D.velocity = targetVelocity;

        m_Grounded = Physics2D.Linecast(transform.position, m_GroundCheck.position, m_GroundLayer);

        if(!m_Grounded)
        {
            animator.SetBool("Jumping", true);
        }
        if(m_Grounded)
        {
            animator.SetBool("Jumping", false);
        }

        if (Input.GetAxisRaw(horizontalInput) > 0 && m_RigidBody2D.velocity.x > 0)
        {
            flip.flipX = false;
        }
        else if (Input.GetAxisRaw(horizontalInput) < 0 && m_RigidBody2D.velocity.x < 0)
        {
            flip.flipX = true;
        }
    }

    //Changes position of the player
    public void changePosition(float newX, float newY, float newZ)
    {
        Vector3 newPos = new Vector3(newX, newY, newZ);
        transform.position = newPos;
    }

    //Changes RunSpeed
    public void setSpeedMultiplier(float newSpeed)
    {
        speedMultiplier = newSpeed;
    }

    //Changes the JumpForce
    public void changeJumpForce(float newForce)
    {
        m_JumpForce = newForce;
    }
}
