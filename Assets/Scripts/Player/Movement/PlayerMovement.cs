using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 movementInput;

    [Header("Dash Properties")]
    [SerializeField] private float dashForce;

    private CharacterController controller;
    private PlayerInputs playerInputs;

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Movement.Enable();

        controller = GetComponent<CharacterController>();
    }

    void Update()
    {       
        CheckInput();
        Movement(movementInput);
        Dash();
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
        controller.Move(movementInput * movementSpeed * Time.deltaTime);
    }

    private void Dash()
    {
        if (playerInputs.Movement.Dash.WasPerformedThisFrame())
        {
            Vector3 dashDirection = transform.forward * movementInput.y + -transform.right * movementInput.x;
            controller.Move(dashDirection * dashForce * Time.deltaTime);
        }
    }

    #endregion
}
