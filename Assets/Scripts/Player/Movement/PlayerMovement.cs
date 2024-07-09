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

    private CharacterController controller;
    private PlayerCombat playerCombat;
    private PlayerInputs playerInputs;

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Movement.Enable();

        controller = GetComponent<CharacterController>();
        playerCombat = GetComponent<PlayerCombat>();
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
            controller.Move(movementInput * movementSpeed * Time.deltaTime);
            lastMovementDir = movementInput;
        }
    }

    #endregion
}
