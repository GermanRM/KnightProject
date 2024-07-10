using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    [Header("Throwing Properties")]
    [SerializeField] private Vector2 throwDirection;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwKnockback;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject rockPrefab;

    [Header("Throwing Timer")]
    [SerializeField] private float throwCooldown;
    [SerializeField] private float throwTimer;

    #region References
    private PlayerMovement playerMovement;
    private PlayerInputs playerInputs;
    #endregion

    #region Events

    public static event Action OnPlayerThrow;

    #endregion

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Combat.Enable();

        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        ThrowCooldownManager();
        Throw();
    }

    private void ThrowCooldownManager()
    {
        throwTimer -= Time.deltaTime;

        if (throwTimer < 0)
        {
            throwTimer = 0;
        }
    }

    private void Throw()
    {
        if (playerInputs.Combat.Throw.WasPerformedThisFrame())
        {
            if (throwTimer > 0) return; 

            GameObject go = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);
            go.GetComponent<RockMovement>().Initialize(playerMovement.GetLastMovDir(), throwForce, throwKnockback);

            throwTimer = throwCooldown;
            OnPlayerThrow?.Invoke();
        }
    }
}
