using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    [Header("Throwing Properties")]
    [SerializeField] private Vector2 throwDirection;
    [SerializeField] private float throwForce;
    [SerializeField] private GameObject rockPrefab;

    [Header("Throwing Timer")]
    [SerializeField] private float throwCooldown;
    [SerializeField] private float throwTimer;

    #region References
    private PlayerMovement playerMovement;
    private PlayerInputs playerInputs;
    #endregion

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Combat.Enable();

        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerInputs.Combat.Throw.WasPerformedThisFrame())
        {
            GameObject go = Instantiate(rockPrefab, transform.position, Quaternion.identity);
            go.GetComponent<RockMovement>().Initialize(playerMovement.GetLastMovDir(), throwForce);
        }
    }
}
