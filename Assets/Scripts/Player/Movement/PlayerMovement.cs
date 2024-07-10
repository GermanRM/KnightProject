using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector2 lastMovementDir;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    private PlayerCombat playerCombat;
    private PlayerInputs playerInputs;

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Movement.Enable();

        rb = GetComponent<Rigidbody2D>();
        playerCombat = GetComponentInChildren<PlayerCombat>();
    }

    #region Getter / Setter

    public Vector2 GetMovementInput()
    {
        return movementInput;
    }

    public Vector2 GetLastMovDir()
    {
        return lastMovementDir;
    }

    #endregion

    void Update()
    {       
        CheckInput();       
    }

    private void FixedUpdate()
    {
        Movement(movementInput);
    }

    #region Inputs

    private void CheckInput()
    {
       movementInput = playerInputs.Movement.Movement.ReadValue<Vector2>().normalized;
    }

    #endregion

    #region Movement

    private void Movement(Vector2 movementInput)
    {
        if (movementInput != Vector2.zero && !playerCombat.isDamaged)
        {
            rb.velocity = (movementInput * movementSpeed);
            lastMovementDir = movementInput;
        }
        else rb.velocity = Vector2.zero;
    }

    #endregion
}
