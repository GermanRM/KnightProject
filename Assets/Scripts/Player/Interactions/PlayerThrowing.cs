using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    [Header("Throwing Properties")]
    public int rocksCount;
    [SerializeField] private Vector2 throwDirection;
    [SerializeField] private float throwForce;
    [SerializeField] private float throwKnockback;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject rockPrefab;

    [Header("Throwing Timer")]
    [SerializeField] private float throwCooldown;
    [SerializeField] private bool canThrow = true;

    #region References
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
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
        playerCombat = GetComponent<PlayerCombat>();
    }

    void Update()
    {
        Throw();
    }

    private void Throw()
    {
        if (playerInputs.Combat.Throw.WasPerformedThisFrame())
        {
            if (!canThrow || rocksCount <= 0 || playerCombat.isDead) return; 

            GameObject go = Instantiate(rockPrefab, throwPoint.position, Quaternion.identity);

            if (playerMovement.GetLastMovDir() == Vector2.zero)
                go.GetComponent<RockMovement>().Initialize(Vector2.right, throwForce, throwKnockback);
            else
                go.GetComponent<RockMovement>().Initialize(playerMovement.GetLastMovDir(), throwForce, throwKnockback);

            rocksCount--;
            StartCoroutine(ThrowCooldown());

            OnPlayerThrow?.Invoke();
        }
    }

    private IEnumerator ThrowCooldown()
    {
        canThrow = false;

        yield return new WaitForSeconds(throwCooldown);

        canThrow = true;
    }
}
